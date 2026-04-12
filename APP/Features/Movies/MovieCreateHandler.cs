using APP.Domain;
using APP.Infrastructure;
using APP.Models;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace APP.Features.Movies
{
    public class MovieCreateRequest : IRequest<CommandResponse>
    {
        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        public DateTime ReleaseDate { get; set; }

        public decimal TotalRevenue { get; set; }

        public int DirectorId { get; set; }

        public List<int>? GenreIds { get; set; }
    }

    public class MovieCreateHandler : IRequestHandler<MovieCreateRequest, CommandResponse>
    {
        private readonly AppDbContext _db;

        public MovieCreateHandler(AppDbContext db)
        {
            _db = db;
        }

        public async Task<CommandResponse> Handle(MovieCreateRequest request, CancellationToken cancellationToken)
        {
            var movie = new Movie
            {
                Name = request.Name,
                ReleaseDate = request.ReleaseDate,
                TotalRevenue = request.TotalRevenue,
                DirectorId = request.DirectorId
            };

            _db.Movies.Add(movie);
            await _db.SaveChangesAsync(cancellationToken);

            if (request.GenreIds != null && request.GenreIds.Any())
            {
                foreach (var genreId in request.GenreIds)
                {
                    _db.MovieGenres.Add(new MovieGenre { MovieId = movie.Id, GenreId = genreId });
                }
                await _db.SaveChangesAsync(cancellationToken);
            }

            return new CommandResponse(true, "Movie created successfully.", movie.Id);
        }
    }
}
