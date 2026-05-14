using CORE.APP.Models;
using CORE.APP.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Movies.APP.Domain;
using System.ComponentModel.DataAnnotations;

namespace Movies.APP.Features.Movies
{
    public class MovieCreateRequest : Request, IRequest<CommandResponse>
    {
        [Required, StringLength(200)]
        public string Name { get; set; }

        public DateTime? ReleaseDate { get; set; }

        [Range(0, double.MaxValue)]
        public decimal TotalRevenue { get; set; }

        [Required]
        public int DirectorId { get; set; }

        public List<int> GenreIds { get; set; } = new List<int>();
    }

    public class MovieCreateHandler : Service<Movie>, IRequestHandler<MovieCreateRequest, CommandResponse>
    {
        public MovieCreateHandler(DbContext db) : base(db)
        {
        }

        public async Task<CommandResponse> Handle(MovieCreateRequest request, CancellationToken cancellationToken)
        {
            if (await DbSet().AnyAsync(m => m.Name == request.Name.Trim() && m.DirectorId == request.DirectorId, cancellationToken))
                return Error("Movie with the same name and director exists!");

            var entity = new Movie
            {
                Name = request.Name.Trim(),
                ReleaseDate = request.ReleaseDate,
                TotalRevenue = request.TotalRevenue,
                DirectorId = request.DirectorId,
                GenreIds = request.GenreIds
            };

            await CreateAsync(entity, cancellationToken);

            return Success("Movie created successfully.", entity.Id);
        }
    }
}
