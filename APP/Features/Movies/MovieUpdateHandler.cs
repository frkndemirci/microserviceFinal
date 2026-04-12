using APP.Domain;
using APP.Infrastructure;
using APP.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace APP.Features.Movies
{
    public class MovieUpdateRequest : IRequest<CommandResponse>
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        public DateTime ReleaseDate { get; set; }

        public decimal TotalRevenue { get; set; }

        public int DirectorId { get; set; }

        public List<int>? GenreIds { get; set; }
    }

    public class MovieUpdateHandler : IRequestHandler<MovieUpdateRequest, CommandResponse>
    {
        private readonly AppDbContext _db;

        public MovieUpdateHandler(AppDbContext db)
        {
            _db = db;
        }

        public async Task<CommandResponse> Handle(MovieUpdateRequest request, CancellationToken cancellationToken)
        {
            var movie = await _db.Movies
                .Include(m => m.MovieGenres)
                .FirstOrDefaultAsync(m => m.Id == request.Id, cancellationToken);

            if (movie == null)
                return new CommandResponse(false, "Movie not found.");

            movie.Name = request.Name;
            movie.ReleaseDate = request.ReleaseDate;
            movie.TotalRevenue = request.TotalRevenue;
            movie.DirectorId = request.DirectorId;

            _db.MovieGenres.RemoveRange(movie.MovieGenres);
            if (request.GenreIds != null && request.GenreIds.Any())
            {
                foreach (var genreId in request.GenreIds)
                {
                    _db.MovieGenres.Add(new MovieGenre { MovieId = movie.Id, GenreId = genreId });
                }
            }

            _db.Movies.Update(movie);
            await _db.SaveChangesAsync(cancellationToken);
            return new CommandResponse(true, "Movie updated successfully.", movie.Id);
        }
    }
}
