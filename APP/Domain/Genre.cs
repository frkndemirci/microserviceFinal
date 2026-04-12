using System.ComponentModel.DataAnnotations;

namespace APP.Domain
{
    public class Genre
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public List<MovieGenre> MovieGenres { get; set; } = new List<MovieGenre>();
    }
}
