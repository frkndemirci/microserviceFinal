using CORE.APP.Models;
using CORE.APP.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Users.APP.Domain;

namespace Users.APP.Features.Groups
{
    public class GroupDeleteRequest : Request, IRequest<CommandResponse>
    {
    }

    public class GroupDeleteHandler : Service<Group>, IRequestHandler<GroupDeleteRequest, CommandResponse>
    {
        public GroupDeleteHandler(DbContext db) : base(db)
        {
        }

        protected override IQueryable<Group> DbSet()
        {
            return base.DbSet().Include(g => g.Users);
        }

        public async Task<CommandResponse> Handle(GroupDeleteRequest request, CancellationToken cancellationToken)
        {
            var entity = await DbSet().SingleOrDefaultAsync(g => g.Id == request.Id, cancellationToken);
            if (entity is null)
                return Error("Group not found!");

            if (entity.Users.Any())
                return Error("Group can't be deleted because it has relational users!");

            await DeleteAsync(entity, cancellationToken);

            return Success("Group deleted successfully.", entity.Id);
        }
    }
}
