using APP.Infrastructure;
using APP.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace APP.Features.Genres
{
    public class GenreUpdateRequest : IRequest<CommandResponse>
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }
    }

    public class GenreUpdateHandler : IRequestHandler<GenreUpdateRequest, CommandResponse>
    {
        private readonly AppDbContext _db;

        public GenreUpdateHandler(AppDbContext db)
        {
            _db = db;
        }

        public async Task<CommandResponse> Handle(GenreUpdateRequest request, CancellationToken cancellationToken)
        {
            var genre = await _db.Genres.FirstOrDefaultAsync(g => g.Id == request.Id, cancellationToken);
            if (genre == null)
                return new CommandResponse(false, "Genre not found.");

            genre.Name = request.Name;
            _db.Genres.Update(genre);
            await _db.SaveChangesAsync(cancellationToken);
            return new CommandResponse(true, "Genre updated successfully.", genre.Id);
        }
    }
}
