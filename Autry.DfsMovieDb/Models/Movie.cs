using Autry.DfsMovieDb.Validations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Autry.DfsMovieDb.Models
{
    public class Movie
    {
        public int MovieId { get; set; }
        public string Title { get; set; }
        [YearNotInFuture] //TODO additional validation on year needed but not in scope (null/0, negative)
        public int Year { get; set; }
        public string Genre { get; set; }
        public List<MovieActor> MovieActors { get; set; }
    }
}
