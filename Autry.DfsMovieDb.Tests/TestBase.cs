using AutoMapper;
using Autry.DfsMovieDb.Helpers;
using Autry.DfsMovieDb.Models;
using Castle.Core.Logging;
using Microsoft.EntityFrameworkCore;
using System;

namespace Autry.DfsMovieDb.Tests
{
    public class TestBase
    {
        
        protected DfsMovieDbContext BuildDbContext(string dbName, bool seedData)
        {
            if (seedData) 
            {
                var dbContext = BuildDbContext(dbName);
                SeedData(dbContext);
                //return a new dbcontext to make sure the entities aren't in memory
                var result = BuildDbContext(dbName);
                return result;
            }
            return BuildDbContext(dbName);
        }
        
        protected DfsMovieDbContext BuildDbContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<DfsMovieDbContext>()
                .UseInMemoryDatabase(dbName).Options;
            var dbContext = new DfsMovieDbContext(options);

            return dbContext;
        }

        protected IMapper BuildMapper()
        {
            var config = new MapperConfiguration(options =>
            {
                options.AddProfile(new AutoMapperProfiles());
            });

            return config.CreateMapper();
        }

        private void SeedData(DfsMovieDbContext context)
        {
            context.Actors.Add(new Actor()
            {
                FirstName = "First1",
                LastName = "Last1",
                Birthdate = new DateTime(2000, 01, 01)
            });
            context.Actors.Add(new Actor()
            {
                FirstName = "First2",
                LastName = "Last2",
                Birthdate = new DateTime(2000, 01, 01)
            });
            context.Actors.Add(new Actor()
            {
                FirstName = "First3",
                LastName = "Last3",
                Birthdate = new DateTime(2000, 01, 01)
            });
            context.Movies.Add(new Movie()
            {
                Title = "MovieA",
                Year = 0,
                Genre = "Genre2"
            });
            context.Movies.Add(new Movie()
            {
                Title = "MovieC",
                Year = 0,
                Genre = "Genre1"
            });
            context.Movies.Add(new Movie()
            {
                Title = "MovieB",
                Year = 1,
                Genre = "Genre2"
            });
            context.MovieActor.Add(new MovieActor()
            {
                MovieId = 1,
                ActorId = 1
            });
            context.MovieActor.Add(new MovieActor()
            {
                MovieId = 2,
                ActorId = 2
            });
            context.MovieActor.Add(new MovieActor()
            {
                MovieId = 1,
                ActorId = 2
            });
            context.MovieActor.Add(new MovieActor()
            {
                MovieId = 3,
                ActorId = 1
            });
            
            context.SaveChanges();
        }
    }
}
