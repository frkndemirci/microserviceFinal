using CORE.APP.Models;
using CORE.APP.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Movies.APP.Domain;
using System.ComponentModel.DataAnnotations;

namespace Movies.APP.Features.Movies
{
    public class MovieUpdateRequest : Request, IRequest<CommandResponse>
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

    public class MovieUpdateHandler : Service<Movie>, IRequestHandler<MovieUpdateRequest, CommandResponse>
    {
        public MovieUpdateHandler(DbContext db) : base(db)
        {
        }

        protected override IQueryable<Movie> DbSet()
        {
            return base.DbSet().Include(m => m.MovieGenres);
        }

        public async Task<CommandResponse> Handle(MovieUpdateRequest request, CancellationToken cancellationToken)
        {
            if (await DbSet().AnyAsync(m => m.Id != request.Id && m.Name == request.Name.Trim() && m.DirectorId == request.DirectorId, cancellationToken))
                return Error("Movie with the same name and director exists!");

            var entity = await DbSet().SingleOrDefaultAsync(m => m.Id == request.Id, cancellationToken);
            if (entity is null)
                return Error("Movie not found!");

            Delete(entity.MovieGenres);

            entity.Name = request.Name.Trim();
            entity.ReleaseDate = request.ReleaseDate;
            entity.TotalRevenue = request.TotalRevenue;
            entity.DirectorId = request.DirectorId;
            entity.GenreIds = request.GenreIds;

            await UpdateAsync(entity, cancellationToken);

            return Success("Movie updated successfully.", entity.Id);
        }
    }
}
