using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Autry.DfsMovieDb.Models
{
    public class Actor
    {
        public int ActorId { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [DataType(DataType.Date)] 
        public DateTime Birthdate { get; set; }
        public List<MovieActor> MovieActors { get; set; }
    }
}
