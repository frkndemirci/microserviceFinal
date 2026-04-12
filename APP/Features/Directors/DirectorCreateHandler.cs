using APP.Domain;
using APP.Infrastructure;
using APP.Models;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace APP.Features.Directors
{
    public class DirectorCreateRequest : IRequest<CommandResponse>
    {
        [Required]
        [StringLength(100)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(100)]
        public string LastName { get; set; }

        public bool IsRetired { get; set; }
    }

    public class DirectorCreateHandler : IRequestHandler<DirectorCreateRequest, CommandResponse>
    {
        private readonly AppDbContext _db;

        public DirectorCreateHandler(AppDbContext db)
        {
            _db = db;
        }

        public async Task<CommandResponse> Handle(DirectorCreateRequest request, CancellationToken cancellationToken)
        {
            var director = new Director
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                IsRetired = request.IsRetired
            };
            _db.Directors.Add(director);
            await _db.SaveChangesAsync(cancellationToken);
            return new CommandResponse(true, "Director created successfully.", director.Id);
        }
    }
}
