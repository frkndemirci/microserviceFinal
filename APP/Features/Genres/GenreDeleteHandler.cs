using APP.Infrastructure;
using APP.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace APP.Features.Genres
{
    public class GenreDeleteRequest : IRequest<CommandResponse>
    {
        public int Id { get; set; }
    }

    public class GenreDeleteHandler : IRequestHandler<GenreDeleteRequest, CommandResponse>
    {
        private readonly AppDbContext _db;

        public GenreDeleteHandler(AppDbContext db)
        {
            _db = db;
        }

        public async Task<CommandResponse> Handle(GenreDeleteRequest request, CancellationToken cancellationToken)
        {
            var genre = await _db.Genres.FirstOrDefaultAsync(g => g.Id == request.Id, cancellationToken);
            if (genre == null)
                return new CommandResponse(false, "Genre not found.");

            _db.Genres.Remove(genre);
            await _db.SaveChangesAsync(cancellationToken);
            return new CommandResponse(true, "Genre deleted successfully.", genre.Id);
        }
    }
}
