using Autry.DfsMovieDb.Validations;

namespace Autry.DfsMovieDb.DTOs
{
    public class MovieUpdateDto
    {
        public string Title { get; set; }
        [YearNotInFuture]
        public int Year { get; set; }
        public string Genre { get; set; }
    }
}
