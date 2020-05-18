using System;
using System.ComponentModel.DataAnnotations;

namespace Autry.DfsMovieDb.DTOs
{
    public class ActorCreateDto
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [DataType(DataType.Date)]
        public DateTime Birthdate { get; set; }
    }
}
