using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Autry.DfsMovieDb.Models;
using Autry.DfsMovieDb.Controllers;
using Microsoft.Extensions.Logging.Abstractions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Autry.DfsMovieDb.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Autry.DfsMovieDb.Tests.UnitTests
{
    [TestClass]
    public class ActorsControllerTests : TestBase
    {
        [TestMethod]
        public void GetAllActors_NotImplemented()
        {
            var dbName = Guid.NewGuid().ToString();
            var context = BuildDbContext(dbName);
            var mapper = BuildMapper();
            
            var controller = new ActorsController(new NullLogger<ActorsController>(), context, mapper);
            var result = controller.GetAllActors() as StatusCodeResult;
            
            Assert.AreEqual(StatusCodes.Status404NotFound, result.StatusCode);
        }

        [TestMethod]
        public async Task GetActor_NotFound()
        {
            var dbName = Guid.NewGuid().ToString();
            var context = BuildDbContext(dbName);
            var mapper = BuildMapper();
            var controller = new ActorsController(new NullLogger<ActorsController>(), context, mapper);
            
            var response = await controller.GetActor(1);
            var result = response.Result as StatusCodeResult;
            
            Assert.AreEqual(StatusCodes.Status404NotFound, result.StatusCode);
        }

        [TestMethod]
        public async Task GetActor_ReturnsCorrectActor()
        {
            var dbName = Guid.NewGuid().ToString();
            var context = BuildDbContext(dbName);
            var mapper = BuildMapper();
            
            context.Actors.Add(new Actor()
            {
                FirstName = "First1",
                LastName = "Last1",
                Birthdate = new DateTime(2000, 01, 01)
            });
            context.Actors.Add(new Actor()
            {
                FirstName = "First1",
                LastName = "Last1",
                Birthdate = new DateTime(2000, 01, 01)
            });
            context.SaveChanges();

            var context2 = BuildDbContext(dbName);
            var controller = new ActorsController(new NullLogger<ActorsController>(), context2, mapper);
            var id = 1;
            var response = await controller.GetActor(id);
            var result = response.Value;
            
            Assert.AreEqual(id, result.ActorId);
        }

        [TestMethod]
        public async Task GetActorMovies_NotFound()
        {
            var dbName = Guid.NewGuid().ToString();
            var context = BuildDbContext(dbName);
            var mapper = BuildMapper();
            var controller = new ActorsController(new NullLogger<ActorsController>(), context, mapper);

            var response = await controller.GetActorMovies(1);
            var result = response.Result as StatusCodeResult;

            Assert.AreEqual(StatusCodes.Status404NotFound, result.StatusCode);
        }

        [TestMethod]
        public async Task GetActorMovies_ReturnsCorrectMovies()
        {
            var dbName = Guid.NewGuid().ToString();
            var context = BuildDbContext(dbName, true);
            var mapper = BuildMapper();

            var controller = new ActorsController(new NullLogger<ActorsController>(), context, mapper);
            var response = await controller.GetActorMovies(1);
            var result = response.Value;

            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(3, result[0].MovieId);
            Assert.AreEqual(1, result[1].MovieId);
        }

        [TestMethod]
        public async Task GetActorMovies_IsSorted()
        {
            var dbName = Guid.NewGuid().ToString();
            var context = BuildDbContext(dbName, true);
            var mapper = BuildMapper();
            
            var controller = new ActorsController(new NullLogger<ActorsController>(), context, mapper);
            var response = await controller.GetActorMovies(1);
            var result = response.Value;

            Assert.AreEqual(3, result[0].MovieId); 
            Assert.AreEqual(1, result[1].MovieId); 
        }

        [TestMethod]
        public async Task CreateActor_Success()
        {
            var dbName = Guid.NewGuid().ToString();
            var context = BuildDbContext(dbName);
            var mapper = BuildMapper();
            var actorCreateDto = new ActorCreateDto()
            {
                FirstName = "First1",
                LastName = "Last1",
                Birthdate = new DateTime(2000, 01, 01)
            };

            var controller = new ActorsController(new NullLogger<ActorsController>(), context, mapper);
            var response = await controller.CreateActor(actorCreateDto);
            var result = response as CreatedAtRouteResult;

            Assert.AreEqual(StatusCodes.Status201Created, result.StatusCode);

            var context2 = BuildDbContext(dbName);
            var count = await context2.Actors.CountAsync();
            
            Assert.AreEqual(1, count);
        }

        [TestMethod]
        public void UpdateActor_NotImplemented()
        {
            var dbName = Guid.NewGuid().ToString();
            var context = BuildDbContext(dbName);
            var mapper = BuildMapper();
            context.Actors.Add(new Actor()
            {
                FirstName = "First1",
                LastName = "Last1",
                Birthdate = new DateTime(2000, 01, 01)
            });
            context.SaveChanges();

            var context2 = BuildDbContext(dbName);
            var controller = new ActorsController(new NullLogger<ActorsController>(), context2, mapper);
            var result = controller.UpdateActor(1) as StatusCodeResult;

            Assert.AreEqual(StatusCodes.Status404NotFound, result.StatusCode);
        }

        [TestMethod]
        public void DeleteActor_NotImplemented()
        {
            var dbName = Guid.NewGuid().ToString();
            var context = BuildDbContext(dbName);
            var mapper = BuildMapper();
            context.Actors.Add(new Actor()
            {
                FirstName = "First1",
                LastName = "Last1",
                Birthdate = new DateTime(2000, 01, 01)
            });
            context.SaveChanges();

            var context2 = BuildDbContext(dbName);
            var controller = new ActorsController(new NullLogger<ActorsController>(), context2, mapper);
            var result = controller.DeleteActor(1) as StatusCodeResult;

            Assert.AreEqual(StatusCodes.Status404NotFound, result.StatusCode);
        }
    }
}
