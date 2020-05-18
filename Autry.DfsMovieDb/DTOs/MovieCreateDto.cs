using Autry.DfsMovieDb.Validations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Autry.DfsMovieDb.DTOs
{
    public class MovieCreateDto
    {
        public string Title { get; set; }
        [YearNotInFuture]
        public int Year { get; set; }
        public string Genre { get; set; }
        [Required]
        public List<int> ActorIds { get; set; }

        //TODO it doesn't make sense to allow a list of actors on create but
        //  not on update, need to make this more intuitive
    }
}
