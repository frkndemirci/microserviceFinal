using CORE.APP.Domain;
using System.ComponentModel.DataAnnotations;

namespace Movies.APP.Domain
{
    public class Director : Entity
    {
        [Required, StringLength(100)]
        public string FirstName { get; set; }

        [Required, StringLength(100)]
        public string LastName { get; set; }

        public bool IsRetired { get; set; }

        public List<Movie> Movies { get; set; } = new List<Movie>();
    }
}
