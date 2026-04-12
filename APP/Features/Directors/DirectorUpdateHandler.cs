using APP.Infrastructure;
using APP.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace APP.Features.Directors
{
    public class DirectorUpdateRequest : IRequest<CommandResponse>
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(100)]
        public string LastName { get; set; }

        public bool IsRetired { get; set; }
    }

    public class DirectorUpdateHandler : IRequestHandler<DirectorUpdateRequest, CommandResponse>
    {
        private readonly AppDbContext _db;

        public DirectorUpdateHandler(AppDbContext db)
        {
            _db = db;
        }

        public async Task<CommandResponse> Handle(DirectorUpdateRequest request, CancellationToken cancellationToken)
        {
            var director = await _db.Directors.FirstOrDefaultAsync(d => d.Id == request.Id, cancellationToken);
            if (director == null)
                return new CommandResponse(false, "Director not found.");

            director.FirstName = request.FirstName;
            director.LastName = request.LastName;
            director.IsRetired = request.IsRetired;
            _db.Directors.Update(director);
            await _db.SaveChangesAsync(cancellationToken);
            return new CommandResponse(true, "Director updated successfully.", director.Id);
        }
    }
}
