using AutoMapper;
using Autry.DfsMovieDb.DTOs;
using Autry.DfsMovieDb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Autry.DfsMovieDb.Controllers
{
    [Route("api/actors")]
    [ApiController]
    public class ActorsController : ControllerBase
    {
        private readonly ILogger<ActorsController> _logger;
        private readonly DfsMovieDbContext _context;
        private readonly IMapper _mapper;

        public ActorsController(ILogger<ActorsController> logger, 
            DfsMovieDbContext context,
            IMapper mapper)
        {
            _logger = logger;
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// Getting all actors is not implemented.
        /// </summary>
        /// <returns></returns>
        [HttpGet] //api/actors
        public ActionResult GetAllActors()
        {
            _logger.LogDebug("Action not implemented");

            return NotFound(); //TODO should be 405 and return Allow header field
        }

        /// <summary>
        /// Get an actor.
        /// </summary>
        /// <param name="id">ActorId</param>
        /// <returns></returns>
        [HttpGet("{id:int}", Name = "getActor")] //api/actors/20
        public async Task<ActionResult<ActorDto>> GetActor(int id)
        {
            _logger.LogInformation($"GetActor for ActorID:{id}");

            var actor = await _context.Actors
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.ActorId == id);

            if (actor == null)
            {
                _logger.LogWarning($"ActorId:{id} not found");

                return NotFound();
            }

            var actorDto = _mapper.Map<ActorDto>(actor);

            return actorDto;
        }

        /// <summary>
        /// Get all the movies for an actor.
        /// </summary>
        /// <param name="id">ActorId</param>
        /// <returns></returns>
        [HttpGet("{id:int}/movies")] //api/actors/20/movies
        public async Task<ActionResult<List<MovieListDto>>> GetActorMovies(int id)
        {
            _logger.LogInformation($"GetActorMovies for ActorID:{id}");

            var exists = await _context.Actors.AnyAsync(a => a.ActorId == id);

            if (!exists)
            {
                _logger.LogWarning($"ActorId:{id} not found");

                return NotFound();
            }

            var result = await
               (from ma in _context.MovieActor
                join m in _context.Movies
                     on ma.MovieId equals m.MovieId
                where ma.ActorId == id
                select new MovieListDto
                {
                    MovieId = m.MovieId,
                    Title = m.Title,
                    Year = m.Year,
                    Genre = m.Genre
                })
               .OrderByDescending(m => m.Year)
               .ThenBy(m => m.Title)
               .AsNoTracking()
               .ToListAsync();

            var moviesListDto = _mapper.Map<List<MovieListDto>>(result);

            return moviesListDto;
        }

        /// <summary>
        /// Create a new actor.  Note: First and last names are required.
        /// </summary>
        /// <param name="actorCreateDto">ActorCreateDto</param>
        /// <returns></returns>
        [HttpPost] //api/actors
        public async Task<ActionResult> CreateActor(ActorCreateDto actorCreateDto)
        {
            _logger.LogInformation($"Create actor: {actorCreateDto}");
            
            var actor = _mapper.Map<Actor>(actorCreateDto);
            
            _context.Add(actor);
            await _context.SaveChangesAsync();
            var actorDto = _mapper.Map<ActorDto>(actor);

            return new CreatedAtRouteResult("getActor", new { id = actorDto.ActorId }, actorDto);
        }

        /// <summary>
        /// Updating an actor is not implented.
        /// </summary>
        /// <param name="id">ActorId</param>
        /// <returns></returns>
        [HttpPut("{id}")] //api/actors/20
        public ActionResult UpdateActor(int id)
        {
            _logger.LogDebug("Action not implemented");

            return NotFound(); //TODO should be 405 and return Allow header field
        }

        /// <summary>
        /// Deleting an actor is not implemented.
        /// </summary>
        /// <param name="id">ActorId</param>
        /// <returns></returns>
        [HttpDelete("{id}")] //api/actors/20
        public ActionResult DeleteActor(int id)
        {
            _logger.LogDebug("Action not implemented");

            return NotFound(); //TODO should be 405 and return Allow header field
        }
    }
}
