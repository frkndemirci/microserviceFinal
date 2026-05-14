using CORE.APP.Models;
using CORE.APP.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Users.APP.Domain;

namespace Users.APP.Features.Users
{
    public class UserCreateRequest : Request, IRequest<CommandResponse>
    {
        [Required, StringLength(30, MinimumLength = 4)]
        public string UserName { get; set; }

        [Required, StringLength(15, MinimumLength = 3)]
        public string Password { get; set; }

        [StringLength(50)]
        public string FirstName { get; set; }

        [StringLength(50)]
        public string LastName { get; set; }

        public Genders Gender { get; set; }

        public DateTime? BirthDate { get; set; }

        [Range(0, 5)]
        public decimal Score { get; set; }

        public bool IsActive { get; set; }

        public string Address { get; set; }

        public int? CountryId { get; set; }

        public int? CityId { get; set; }

        public int? GroupId { get; set; }

        public List<int> RoleIds { get; set; } = new List<int>();
    }

    public class UserCreateHandler : Service<User>, IRequestHandler<UserCreateRequest, CommandResponse>
    {
        public UserCreateHandler(DbContext db) : base(db)
        {
        }

        public async Task<CommandResponse> Handle(UserCreateRequest request, CancellationToken cancellationToken)
        {
            if (await DbSet().AnyAsync(u => u.IsActive && u.UserName == request.UserName, cancellationToken))
                return Error("Active user with the same user name exists!");

            var entity = new User
            {
                UserName = request.UserName,
                Password = request.Password,
                FirstName = request.FirstName?.Trim(),
                LastName = request.LastName?.Trim(),
                Gender = request.Gender,
                BirthDate = request.BirthDate,
                RegistrationDate = DateTime.Now,
                Score = request.Score,
                IsActive = request.IsActive,
                Address = request.Address?.Trim(),
                CountryId = request.CountryId,
                CityId = request.CityId,
                GroupId = request.GroupId,
                RoleIds = request.RoleIds
            };

            await CreateAsync(entity, cancellationToken);

            return Success("User created successfully.", entity.Id);
        }
    }
}
