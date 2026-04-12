using APP.Infrastructure;
using MediatR;

namespace APP.Features.Directors
{
    public class DirectorQueryRequest : IRequest<IQueryable<DirectorQueryResponse>>
    {
    }

    public class DirectorQueryResponse
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsRetired { get; set; }
    }

    public class DirectorQueryHandler : IRequestHandler<DirectorQueryRequest, IQueryable<DirectorQueryResponse>>
    {
        private readonly AppDbContext _db;

        public DirectorQueryHandler(AppDbContext db)
        {
            _db = db;
        }

        public Task<IQueryable<DirectorQueryResponse>> Handle(DirectorQueryRequest request, CancellationToken cancellationToken)
        {
            var query = _db.Directors
                .Select(d => new DirectorQueryResponse
                {
                    Id = d.Id,
                    FirstName = d.FirstName,
                    LastName = d.LastName,
                    IsRetired = d.IsRetired
                });
            return Task.FromResult(query);
        }
    }
}
