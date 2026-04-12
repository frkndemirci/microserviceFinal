using APP.Infrastructure;
using APP.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace APP.Features.Directors
{
    public class DirectorDeleteRequest : IRequest<CommandResponse>
    {
        public int Id { get; set; }
    }

    public class DirectorDeleteHandler : IRequestHandler<DirectorDeleteRequest, CommandResponse>
    {
        private readonly AppDbContext _db;

        public DirectorDeleteHandler(AppDbContext db)
        {
            _db = db;
        }

        public async Task<CommandResponse> Handle(DirectorDeleteRequest request, CancellationToken cancellationToken)
        {
            var director = await _db.Directors.FirstOrDefaultAsync(d => d.Id == request.Id, cancellationToken);
            if (director == null)
                return new CommandResponse(false, "Director not found.");

            _db.Directors.Remove(director);
            await _db.SaveChangesAsync(cancellationToken);
            return new CommandResponse(true, "Director deleted successfully.", director.Id);
        }
    }
}
