using CORE.APP.Models;
using CORE.APP.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Movies.APP.Domain;

namespace Movies.APP.Features.Movies
{
    public class MovieQueryRequest : Request, IRequest<IQueryable<MovieQueryResponse>>
    {
        public string Name { get; set; }
        public int? DirectorId { get; set; }
        public List<int> GenreIds { get; set; } = new List<int>();
    }

    public class MovieQueryResponse : Response
    {
        public string Name { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public decimal TotalRevenue { get; set; }
        public int DirectorId { get; set; }
        public List<int> GenreIds { get; set; }

        public string ReleaseDateF { get; set; }
        public string TotalRevenueF { get; set; }
        public string DirectorF { get; set; }
        public List<string> GenresF { get; set; }
    }

    public class MovieQueryHandler : Service<Movie>, IRequestHandler<MovieQueryRequest, IQueryable<MovieQueryResponse>>
    {
        public MovieQueryHandler(DbContext db) : base(db)
        {
        }

        protected override IQueryable<Movie> DbSet()
        {
            return base.DbSet()
                .AsNoTracking()
                .Include(m => m.Director)
                .Include(m => m.MovieGenres).ThenInclude(mg => mg.Genre)
                .OrderByDescending(m => m.ReleaseDate)
                .ThenBy(m => m.Name);
        }

        public Task<IQueryable<MovieQueryResponse>> Handle(MovieQueryRequest request, CancellationToken cancellationToken)
        {
            var entityQuery = DbSet();

            if (!string.IsNullOrWhiteSpace(request.Name))
                entityQuery = entityQuery.Where(m => m.Name.Contains(request.Name.Trim()));

            if (request.DirectorId.HasValue)
                entityQuery = entityQuery.Where(m => m.DirectorId == request.DirectorId.Value);

            if (request.GenreIds.Count > 0)
                entityQuery = entityQuery.Where(m => m.MovieGenres.Any(mg => request.GenreIds.Contains(mg.GenreId)));

            var query = entityQuery.Select(m => new MovieQueryResponse
            {
                Id = m.Id,
                Name = m.Name,
                ReleaseDate = m.ReleaseDate,
                TotalRevenue = m.TotalRevenue,
                DirectorId = m.DirectorId,
                GenreIds = m.MovieGenres.Select(mg => mg.GenreId).ToList(),

                ReleaseDateF = m.ReleaseDate.HasValue ? m.ReleaseDate.Value.ToString("MM/dd/yyyy") : string.Empty,
                TotalRevenueF = m.TotalRevenue.ToString("C2"),
                DirectorF = m.Director != null ? m.Director.FirstName + " " + m.Director.LastName : null,
                GenresF = m.MovieGenres.Select(mg => mg.Genre.Name).ToList()
            });

            return Task.FromResult(query);
        }
    }
}
