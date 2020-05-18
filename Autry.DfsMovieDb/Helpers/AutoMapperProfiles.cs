using AutoMapper;
using Autry.DfsMovieDb.DTOs;
using Autry.DfsMovieDb.Models;
using System.Collections.Generic;
using System.Linq;

namespace Autry.DfsMovieDb.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            //Actor
            CreateMap<Actor, ActorDto>().ReverseMap();
            CreateMap<ActorCreateDto, Actor>();

            //Movie
            CreateMap<Movie, MovieDto>()
                .ForMember(dto => dto.Actors, m => m.MapFrom(m => m.MovieActors.Select(ma => ma.Actor)));
            CreateMap<MovieDto, Movie>();
            CreateMap<Movie, MovieListDto>();
            CreateMap<MovieCreateDto, Movie>()
                .ForMember(m => m.MovieActors, options => options.MapFrom(MapMovieActors));
            CreateMap<MovieUpdateDto, Movie>();
        }

        private List<MovieActor> MapMovieActors(MovieCreateDto movieCreateDto, Movie movie)
        {
            var movieActors = new List<MovieActor>();
            foreach (var actorId in movieCreateDto.ActorIds)
            {
                movieActors.Add(new MovieActor() { ActorId = actorId });
            }

            return movieActors;
        }
    }
}
