using System.Collections.Generic;

namespace Autry.DfsMovieDb.DTOs
{
    public class MovieDto
    {
        public int MovieId { get; set; }
        public string Title { get; set; }
        public int Year { get; set; }
        public string Genre { get; set; }
        public List<ActorDto> Actors { get; set; }
    }
}
