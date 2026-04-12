using System.ComponentModel.DataAnnotations;

namespace APP.Domain
{
    public class Movie
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        public DateTime ReleaseDate { get; set; }

        public decimal TotalRevenue { get; set; }

        public int DirectorId { get; set; }

        public Director Director { get; set; }

        public List<MovieGenre> MovieGenres { get; set; } = new List<MovieGenre>();
    }
}
