using APP.Infrastructure;
using MediatR;

namespace APP.Features.Genres
{
    public class GenreQueryRequest : IRequest<IQueryable<GenreQueryResponse>>
    {
    }

    public class GenreQueryResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class GenreQueryHandler : IRequestHandler<GenreQueryRequest, IQueryable<GenreQueryResponse>>
    {
        private readonly AppDbContext _db;

        public GenreQueryHandler(AppDbContext db)
        {
            _db = db;
        }

        public Task<IQueryable<GenreQueryResponse>> Handle(GenreQueryRequest request, CancellationToken cancellationToken)
        {
            var query = _db.Genres
                .Select(g => new GenreQueryResponse
                {
                    Id = g.Id,
                    Name = g.Name
                });
            return Task.FromResult(query);
        }
    }
}
