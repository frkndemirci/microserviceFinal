using APP.Domain;
using APP.Infrastructure;
using APP.Models;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace APP.Features.Genres
{
    public class GenreCreateRequest : IRequest<CommandResponse>
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
    }

    public class GenreCreateHandler : IRequestHandler<GenreCreateRequest, CommandResponse>
    {
        private readonly AppDbContext _db;

        public GenreCreateHandler(AppDbContext db)
        {
            _db = db;
        }

        public async Task<CommandResponse> Handle(GenreCreateRequest request, CancellationToken cancellationToken)
        {
            var genre = new Genre { Name = request.Name };
            _db.Genres.Add(genre);
            await _db.SaveChangesAsync(cancellationToken);
            return new CommandResponse(true, "Genre created successfully.", genre.Id);
        }
    }
}
