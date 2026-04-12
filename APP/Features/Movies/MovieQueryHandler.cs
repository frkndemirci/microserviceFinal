using APP.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace APP.Features.Movies
{
    public class MovieQueryRequest : IRequest<IQueryable<MovieQueryResponse>>
    {
    }

    public class MovieQueryResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime ReleaseDate { get; set; }
        public decimal TotalRevenue { get; set; }
        public string DirectorFullName { get; set; }
        public List<string> GenreNames { get; set; }
    }

    public class MovieQueryHandler : IRequestHandler<MovieQueryRequest, IQueryable<MovieQueryResponse>>
    {
        private readonly AppDbContext _db;

        public MovieQueryHandler(AppDbContext db)
        {
            _db = db;
        }

        public Task<IQueryable<MovieQueryResponse>> Handle(MovieQueryRequest request, CancellationToken cancellationToken)
        {
            var query = _db.Movies
                .Include(m => m.Director)
                .Include(m => m.MovieGenres)
                .ThenInclude(mg => mg.Genre)
                .Select(m => new MovieQueryResponse
                {
                    Id = m.Id,
                    Name = m.Name,
                    ReleaseDate = m.ReleaseDate,
                    TotalRevenue = m.TotalRevenue,
                    DirectorFullName = m.Director.FirstName + " " + m.Director.LastName,
                    GenreNames = m.MovieGenres.Select(mg => mg.Genre.Name).ToList()
                });
            return Task.FromResult(query);
        }
    }
}
