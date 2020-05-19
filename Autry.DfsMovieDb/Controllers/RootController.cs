using Autry.DfsMovieDb.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Autry.DfsMovieDb.Controllers
{
    [ApiController]
    [Route("api")]
    public class RootController : ControllerBase
    {
        [HttpGet(Name = "getRoot")]
        public ActionResult<IEnumerable<Link>> GetRootLinks()
        {
            List<Link> links = new List<Link>();

            links.Add(new Link(href: Url.Link("getRoot", new { }), rel: "self", method: "GET"));
            //actors
            //links.Add(new Link(href: Url.Link("getActor", new { }), rel: "get-actor", method: "GET"));
            //links.Add(new Link(href: Url.Link("getActorMovies", new { }), rel: "get-actor-movies", method: "GET"));
            links.Add(new Link(href: Url.Link("createActor", new { }), rel: "create-actor", method: "POST"));

            //movies
            links.Add(new Link(href: Url.Link("getMovies", new { }), rel: "get-movies", method: "GET"));
            //links.Add(new Link(href: Url.Link("getMovieById", new { }), rel: "get-movie", method: "GET"));
            //links.Add(new Link(href: Url.Link("getMovieActors", new { }), rel: "get-movie-actors", method: "GET"));
            links.Add(new Link(href: Url.Link("createMovie", new { }), rel: "create-movie", method: "POST"));
            //links.Add(new Link(href: Url.Link("addActorToMovie", new { }), rel: "add-actor-to-movie", method: "POST"));
            //links.Add(new Link(href: Url.Link("updateMovie", new { }), rel: "update-movie", method: "PUT"));
            //links.Add(new Link(href: Url.Link("deleteMovie", new { }), rel: "delete-movie", method: "DELETE"));



            return links;

        }
    }
}
