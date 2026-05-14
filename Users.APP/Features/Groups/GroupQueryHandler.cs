using CORE.APP.Models;
using CORE.APP.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Users.APP.Domain;

namespace Users.APP.Features.Groups
{
    public class GroupQueryRequest : Request, IRequest<IQueryable<GroupQueryResponse>>
    {
    }

    public class GroupQueryResponse : Response
    {
        public string Title { get; set; }
        public int UserCount { get; set; }
    }

    public class GroupQueryHandler : Service<Group>, IRequestHandler<GroupQueryRequest, IQueryable<GroupQueryResponse>>
    {
        public GroupQueryHandler(DbContext db) : base(db)
        {
        }

        protected override IQueryable<Group> DbSet()
        {
            return base.DbSet().Include(g => g.Users).OrderBy(g => g.Title);
        }

        public Task<IQueryable<GroupQueryResponse>> Handle(GroupQueryRequest request, CancellationToken cancellationToken)
        {
            var query = DbSet().Select(g => new GroupQueryResponse
            {
                Id = g.Id,
                Title = g.Title,
                UserCount = g.Users.Count
            });
            return Task.FromResult(query);
        }
    }
}
