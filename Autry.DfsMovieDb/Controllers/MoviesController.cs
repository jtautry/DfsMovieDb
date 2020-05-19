using AutoMapper;
using Autry.DfsMovieDb.DTOs;
using Autry.DfsMovieDb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Autry.DfsMovieDb.Controllers
{
    [Route("api/movies")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly ILogger<MoviesController> _logger;
        private readonly DfsMovieDbContext _context;
        private readonly IMapper _mapper;

        public MoviesController(ILogger<MoviesController> logger,
            DfsMovieDbContext context,
            IMapper mapper)
        {
            _logger = logger;
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// Get all movies ordered by Year (descending) and Title.  Note: Pagination is not implemented at this time.
        /// </summary>
        /// <returns></returns>
        [HttpGet(Name = "getMovies")] //api/movies
        public async Task<ActionResult<List<MovieListDto>>> GetAllMovies()
        {
            var movies = await _context.Movies
                .OrderByDescending(m => m.Year)
                .ThenBy(m => m.Title)
                .AsNoTracking()
                .ToListAsync();
            var movieListDto = _mapper.Map<List<MovieListDto>>(movies);

            return movieListDto;
        }

        /// <summary>
        /// Get a movie.
        /// </summary>
        /// <param name="id">MovieId</param>
        /// <returns></returns>
        [HttpGet("{id:int}", Name = "getMovieById")] //api/movies/10
        public async Task<ActionResult<MovieDto>> GetMovie(int id)
        {
            var movie = await _context.Movies
                            .Include(m => m.MovieActors).ThenInclude(ma => ma.Actor)
                            .AsNoTracking()
                            .FirstOrDefaultAsync(m => m.MovieId == id);

            if (movie == null)
            {
                return NotFound();
            }

            var movieDto = _mapper.Map<MovieDto>(movie);

            return movieDto;
        }

        /// <summary>
        /// Get the actors in a movie.
        /// </summary>
        /// <param name="id">MovieId</param>
        /// <returns></returns>
        [HttpGet("{id:int}/actors", Name = "getMovieActors")] //api/movies/10/actors
        public async Task<ActionResult<List<ActorDto>>> GetMovieActors(int id)
        {
            var exists = await _context.Movies.AnyAsync(m => m.MovieId == id);

            if (!exists)
            {
                _logger.LogWarning($"MovieId:{id} not found");

                return NotFound();
            }

            return await GetActorsByMovieId(id);
        }

        /// <summary>
        /// Create a new movie.  Note: The year cannot be greater than the current year and least one actor is required.
        /// </summary>
        /// <param name="movieCreateDto">MovieCreateDto</param>
        /// <returns></returns>
        [HttpPost(Name = "createMovie")] //api/movies
        public async Task<ActionResult<MovieDto>> CreateMovie(MovieCreateDto movieCreateDto)
        {
            for (int i = movieCreateDto.ActorIds.Count - 1; i >= 0; i--)
            {
                //validate actor exists
                var exists = await _context.Actors.AnyAsync(a => a.ActorId == movieCreateDto.ActorIds[i]);

                if (!exists)
                {
                    movieCreateDto.ActorIds.RemoveAt(i);
                    //TODO need to let the caller know an item was removed
                }
            }

            if (movieCreateDto.ActorIds.Count == 0)
            {
                return BadRequest("A valid ActorId is required to create a movie.");
            };

            //TODO movie creation needs to be seperated into to two steps in a transaction. 
            //  This way we can have a single process for creating the relation.
            var movie = _mapper.Map<Movie>(movieCreateDto);
            _context.Add(movie);
            
            //TODO this can still throw an exception if the actor is deleted before the insert happens.
            //  Not handling it now because delete on actor is not implemented.
            await _context.SaveChangesAsync();
            
            var result = await _context.Movies
                            .Include(m => m.MovieActors).ThenInclude(ma => ma.Actor)
                            .AsNoTracking()
                            .FirstOrDefaultAsync(m => m.MovieId == movie.MovieId);

            var movieDto = _mapper.Map<MovieDto>(result);

            return movieDto;
        }

        /// <summary>
        /// Add an actor to this movie.
        /// </summary>
        /// <param name="id">MovieId</param>
        /// <param name="movieActorDto">MovieActorDto</param>
        /// <returns></returns>
        [HttpPost("{id}/actors", Name = "addActorToMovie")] //api/movies/10/actors
        public async Task<ActionResult<List<ActorDto>>> CreateMovieActor(int id, MovieActorDto movieActorDto)
        {
            //TODO find a better way to do this, it results in 3 db calls and no guarantee
            //  that the entities still exist by the time the relation is created
            var movieExists = await _context.Movies.AnyAsync(m => m.MovieId == id);

            if (!movieExists)
            {
                _logger.LogWarning($"MovieId:{id} not found");

                return NotFound();
            }

            var actorExists = await _context.Actors.AnyAsync(a => a.ActorId == movieActorDto.ActorId);

            if (!actorExists)
            {
                _logger.LogWarning($"ActorId:{id} not found");

                return NotFound();
            }

            var movieActor = new MovieActor()
            {
                MovieId = id,
                ActorId = movieActorDto.ActorId
            };

            _context.Add(movieActor);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                return NotFound("This movie has been deleted.");
            }
            
            return await GetActorsByMovieId(id);
        }

        /// <summary>
        /// Update a movie.
        /// </summary>
        /// <param name="id">MovieId</param>
        /// <param name="movieUpdateDto">MovieUpdateDto</param>
        /// <returns></returns>
        [HttpPut("{id}", Name = "updateMovie")] //api/movies/10
        public async Task<ActionResult> UpdateMovie(int id, MovieUpdateDto movieUpdateDto)
        {
            var movie = _mapper.Map<Movie>(movieUpdateDto);
            movie.MovieId = id;
            _context.Entry(movie).State = EntityState.Modified;
            
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound("The movie no longer exists.");
            }
            
            return NoContent();
            //TODO return location in header
        }

        /// <summary>
        /// Delete a movie.
        /// </summary>
        /// <param name="id">MovieId</param>
        /// <returns></returns>
        [HttpDelete("{id}", Name = "deleteMovie")] //api/movies/10
        public async Task<ActionResult> DeleteMovie(int id)
        {
            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }

            _context.Movies.Remove(movie);
            
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private async Task<ActionResult<List<ActorDto>>> GetActorsByMovieId(int id)
        {
            var result = await 
                (from ma in _context.MovieActor
                join a in _context.Actors
                    on ma.ActorId equals a.ActorId
                where ma.MovieId == id
                select new ActorDto
                {
                    ActorId = a.ActorId,
                    FirstName = a.FirstName,
                    LastName = a.LastName,
                    Birthdate = a.Birthdate
                })
                .AsNoTracking()
                .ToListAsync();

            var actorsDto = _mapper.Map<List<ActorDto>>(result);

            return actorsDto;
        }
    }
}
