using CORE.APP.Models;
using CORE.APP.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Users.APP.Domain;

namespace Users.APP.Features.Roles
{
    public class RoleQueryRequest : Request, IRequest<IQueryable<RoleQueryResponse>>
    {
    }

    public class RoleQueryResponse : Response
    {
        public string Name { get; set; }
        public int UserCount { get; set; }
    }

    public class RoleQueryHandler : Service<Role>, IRequestHandler<RoleQueryRequest, IQueryable<RoleQueryResponse>>
    {
        public RoleQueryHandler(DbContext db) : base(db)
        {
        }

        protected override IQueryable<Role> DbSet()
        {
            return base.DbSet().Include(r => r.UserRoles).OrderBy(r => r.Name);
        }

        public Task<IQueryable<RoleQueryResponse>> Handle(RoleQueryRequest request, CancellationToken cancellationToken)
        {
            var query = DbSet().Select(r => new RoleQueryResponse
            {
                Id = r.Id,
                Name = r.Name,
                UserCount = r.UserRoles.Count
            });
            return Task.FromResult(query);
        }
    }
}
