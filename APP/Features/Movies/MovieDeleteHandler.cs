using APP.Infrastructure;
using APP.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace APP.Features.Movies
{
    public class MovieDeleteRequest : IRequest<CommandResponse>
    {
        public int Id { get; set; }
    }

    public class MovieDeleteHandler : IRequestHandler<MovieDeleteRequest, CommandResponse>
    {
        private readonly AppDbContext _db;

        public MovieDeleteHandler(AppDbContext db)
        {
            _db = db;
        }

        public async Task<CommandResponse> Handle(MovieDeleteRequest request, CancellationToken cancellationToken)
        {
            var movie = await _db.Movies
                .Include(m => m.MovieGenres)
                .FirstOrDefaultAsync(m => m.Id == request.Id, cancellationToken);

            if (movie == null)
                return new CommandResponse(false, "Movie not found.");

            _db.MovieGenres.RemoveRange(movie.MovieGenres);
            _db.Movies.Remove(movie);
            await _db.SaveChangesAsync(cancellationToken);
            return new CommandResponse(true, "Movie deleted successfully.", movie.Id);
        }
    }
}
