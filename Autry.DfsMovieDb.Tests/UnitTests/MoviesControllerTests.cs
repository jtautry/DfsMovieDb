using Autry.DfsMovieDb.Controllers;
using Autry.DfsMovieDb.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Autry.DfsMovieDb.Tests.UnitTests
{
    [TestClass]
    public class MoviesControllerTests : TestBase
    {

        [TestMethod]
        public async Task GetAllMovies_IsSorted()
        {
            var dbName = Guid.NewGuid().ToString();
            var context = BuildDbContext(dbName, true);
            var mapper = BuildMapper();

            var controller = new MoviesController(new NullLogger<MoviesController>(), context, mapper);
            var response = await controller.GetAllMovies();
            var result = response.Value;

            Assert.AreEqual(3, result[0].MovieId);
            Assert.AreEqual(1, result[1].MovieId);
            Assert.AreEqual(2, result[2].MovieId);
        }
        
        [TestMethod]
        public async Task GetAllMovies_CorrectCount()
        {
            var dbName = Guid.NewGuid().ToString();
            var context = BuildDbContext(dbName, true);
            var mapper = BuildMapper();

            var controller = new MoviesController(new NullLogger<MoviesController>(), context, mapper);
            var response = await controller.GetAllMovies();
            var result = response.Value;

            Assert.AreEqual(3, result.Count);
        }

        [TestMethod]
        public async Task GetMovie_NotFound()
        {
            var dbName = Guid.NewGuid().ToString();
            var context = BuildDbContext(dbName);
            var mapper = BuildMapper();
            var controller = new MoviesController(new NullLogger<MoviesController>(), context, mapper);

            var response = await controller.GetMovie(1);
            var result = response.Result as StatusCodeResult;

            Assert.AreEqual(StatusCodes.Status404NotFound, result.StatusCode);
        }

        [TestMethod]
        public async Task GetMovie_ReturnsCorrectMovie()
        {
            var dbName = Guid.NewGuid().ToString();
            var context = BuildDbContext(dbName, true);
            var mapper = BuildMapper();
            
            var controller = new MoviesController(new NullLogger<MoviesController>(), context, mapper);
            var id = 1;
            var response = await controller.GetMovie(id);
            var result = response.Value;

            Assert.AreEqual(id, result.MovieId);
        }

        [TestMethod]
        public async Task GetMovieActors_MovieNotFound()
        {
            var dbName = Guid.NewGuid().ToString();
            var context = BuildDbContext(dbName);
            var mapper = BuildMapper();
            var controller = new MoviesController(new NullLogger<MoviesController>(), context, mapper);

            var response = await controller.GetMovieActors(1);
            var result = response.Result as StatusCodeResult;

            Assert.AreEqual(StatusCodes.Status404NotFound, result.StatusCode);
        }

        [TestMethod]
        public async Task GetMovieActors_ReturnsCorrectActors()
        {
            var dbName = Guid.NewGuid().ToString();
            var context = BuildDbContext(dbName, true);
            var mapper = BuildMapper();

            var controller = new MoviesController(new NullLogger<MoviesController>(), context, mapper);
            var id = 1;
            var response = await controller.GetMovieActors(id);
            var result = response.Value;

            Assert.AreEqual(1, result[0].ActorId);
            Assert.AreEqual(2, result[1].ActorId);
        }

        [TestMethod]
        public async Task CreateMovie_InvalidActorReturnsBadRequest()
        {
            var dbName = Guid.NewGuid().ToString();
            var context = BuildDbContext(dbName, true);
            var mapper = BuildMapper();
            var controller = new MoviesController(new NullLogger<MoviesController>(), context, mapper);
            var movieCreateDto = new MovieCreateDto()
            {
                Title = "Title1",
                Year = 0,
                Genre = "Genre1",
                ActorIds = new List<int>() { 10 }
            };

            var response = await controller.CreateMovie(movieCreateDto);
            var result = response.Result as BadRequestObjectResult;

            Assert.AreEqual(StatusCodes.Status400BadRequest, result.StatusCode);
        }

        [TestMethod]
        public async Task CreateMovie_ActorListWithValidAndInvalidActorSucceeds()
        {
            var dbName = Guid.NewGuid().ToString();
            var context = BuildDbContext(dbName, true);
            var mapper = BuildMapper();
            var controller = new MoviesController(new NullLogger<MoviesController>(), context, mapper);
            var movieCreateDto = new MovieCreateDto()
            {
                Title = "Title1",
                Year = 0,
                Genre = "Genre1",
                ActorIds = new List<int>() { 10, 1 }
            };

            var response = await controller.CreateMovie(movieCreateDto);
            var result = response.Value;

            Assert.AreEqual(4, result.MovieId);
        }
        
        [TestMethod]
        public async Task CreateMovie_ActorNotSuppliedIsBadRequest()
        {
            var dbName = Guid.NewGuid().ToString();
            var context = BuildDbContext(dbName);
            var mapper = BuildMapper();
            var controller = new MoviesController(new NullLogger<MoviesController>(), context, mapper);
            var movieCreateDto = new MovieCreateDto()
            {
                Title = "Title1",
                Year = 0,
                Genre = "Genre1",
                ActorIds = new List<int>()
            };

            var response = await controller.CreateMovie(movieCreateDto);
            var result = response.Result as BadRequestObjectResult;

            Assert.AreEqual(StatusCodes.Status400BadRequest, result.StatusCode);
        }

        [TestMethod]
        public async Task CreateMovie_FutureYearReturnsNull()
        {
            //TODO This doesn't seem like the best place to test the year validation because
            //  I can't test the response code. But I want to have at least one test on the year
            //  and it's already done so I'll leave it for now.

            var dbName = Guid.NewGuid().ToString();
            var context = BuildDbContext(dbName, true);
            var mapper = BuildMapper();
            var controller = new MoviesController(new NullLogger<MoviesController>(), context, mapper);
            var movieCreateDto = new MovieCreateDto()
            {
                Title = "Title1",
                Year = DateTime.Now.Year + 2, 
                Genre = "Genre1",
                ActorIds = new List<int> { 1 }
            };

            var response = await controller.CreateMovie(movieCreateDto);
            Assert.IsNull(response.Result);
        }

        [TestMethod]
        public async Task CreateMovie_ReturnsCorrectCreatedMovie()
        {
            var dbName = Guid.NewGuid().ToString();
            var context = BuildDbContext(dbName, true);
            var mapper = BuildMapper();
            var controller = new MoviesController(new NullLogger<MoviesController>(), context, mapper);
            var movieCreateDto = new MovieCreateDto()
            {
                Title = "Title4",
                Year = 2000,
                Genre = "Genre4",
                ActorIds = new List<int> { 1 }
            };

            var response = await controller.CreateMovie(movieCreateDto);
            var result = response.Value;

            Assert.AreEqual(4, result.MovieId);
            Assert.AreEqual("Title4", result.Title);
            Assert.AreEqual(2000, result.Year);
            Assert.AreEqual("Genre4", result.Genre);
            Assert.AreEqual(1, result.Actors[0].ActorId);
        }

        [TestMethod]
        public async Task CreateMovieActors_MovieNotFound()
        {
            var dbName = Guid.NewGuid().ToString();
            var context = BuildDbContext(dbName, true);
            var mapper = BuildMapper();
            var controller = new MoviesController(new NullLogger<MoviesController>(), context, mapper);
            var movieActorDto = new MovieActorDto()
            {
                ActorId = 3
            };

            var response = await controller.CreateMovieActor(10, movieActorDto);
            var result = response.Result as StatusCodeResult;

            Assert.AreEqual(StatusCodes.Status404NotFound, result.StatusCode);
        }
        
        [TestMethod]
        public async Task CreateMovieActors_ActorNotFound()
        {
            var dbName = Guid.NewGuid().ToString();
            var context = BuildDbContext(dbName, true);
            var mapper = BuildMapper();
            var controller = new MoviesController(new NullLogger<MoviesController>(), context, mapper);
            var movieActorDto = new MovieActorDto()
            {
                ActorId = 10
            };

            var response = await controller.CreateMovieActor(1, movieActorDto);
            var result = response.Result as StatusCodeResult;

            Assert.AreEqual(StatusCodes.Status404NotFound, result.StatusCode);
        }

        [TestMethod]
        public async Task CreateMovieActors_RelationCreated()
        {
            var dbName = Guid.NewGuid().ToString();
            var context = BuildDbContext(dbName, true);
            var mapper = BuildMapper();
            var controller = new MoviesController(new NullLogger<MoviesController>(), context, mapper);
            var movieActorDto = new MovieActorDto()
            {
                ActorId = 3
            };

            var response = await controller.CreateMovieActor(1, movieActorDto); 
            var result = response.Value;

            Assert.AreEqual(3, result.Count);
        }

        [TestMethod]
        public async Task UpdateMovie_NotFound()
        {
            var dbName = Guid.NewGuid().ToString();
            var context = BuildDbContext(dbName);
            var mapper = BuildMapper();
            var controller = new MoviesController(new NullLogger<MoviesController>(), context, mapper);
            
            var movieUpdateDto = new MovieUpdateDto()
            {
                Title = "TitleUp",
                Year = 0,
                Genre = "Genre1"
            };

            var response = await controller.UpdateMovie(10, movieUpdateDto);
            var result = response as NotFoundObjectResult;
            
            Assert.AreEqual(StatusCodes.Status404NotFound, result.StatusCode);
        }

        [TestMethod]
        public async Task UpdateMovie_IsUpdated()
        {
            var dbName = Guid.NewGuid().ToString();
            var context = BuildDbContext(dbName, true);
            var mapper = BuildMapper();
            var controller = new MoviesController(new NullLogger<MoviesController>(), context, mapper);

            var movieUpdateDto = new MovieUpdateDto()
            {
                Title = "TitleUp",
                Year = 0,
                Genre = "Genre1"
            };

            var response = await controller.UpdateMovie(1, movieUpdateDto);
            var result = response as NoContentResult;

            Assert.AreEqual(StatusCodes.Status204NoContent, result.StatusCode);

            //check that the update was actually made
            var context2 = BuildDbContext(dbName);
            var exists = await context2.Movies.AnyAsync(m => m.Title == "TitleUp");
            
            Assert.IsTrue(exists);
        }

        [TestMethod]
        public async Task DeleteMovie_NotFound()
        {
            var dbName = Guid.NewGuid().ToString();
            var context = BuildDbContext(dbName);
            var mapper = BuildMapper();
            var controller = new MoviesController(new NullLogger<MoviesController>(), context, mapper);

            var response = await controller.DeleteMovie(1);
            var result = response as NotFoundResult;

            Assert.AreEqual(StatusCodes.Status404NotFound, result.StatusCode);
        }

        [TestMethod]
        public async Task DeleteMovie_IsDeleted()
        {
            var dbName = Guid.NewGuid().ToString();
            var context = BuildDbContext(dbName, true);
            var mapper = BuildMapper();
            var controller = new MoviesController(new NullLogger<MoviesController>(), context, mapper);

            var response = await controller.DeleteMovie(1);
            var result = response as NoContentResult;

            Assert.AreEqual(StatusCodes.Status204NoContent, result.StatusCode);

            //check that the delete was actually made
            var context2 = BuildDbContext(dbName);
            var exists = await context2.Movies.AnyAsync(m => m.MovieId == 1);

            Assert.IsFalse(exists);
        }
    }

}
