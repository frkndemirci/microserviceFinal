using CORE.APP.Models;
using CORE.APP.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Users.APP.Domain;

namespace Users.APP.Features.Users
{
    public class UserQueryRequest : Request, IRequest<IQueryable<UserQueryResponse>>
    {
        public string UserName { get; set; }
        public bool? IsActive { get; set; }
        public int? CountryId { get; set; }
        public int? CityId { get; set; }
        public int? GroupId { get; set; }
        public List<int> RoleIds { get; set; } = new List<int>();
    }

    public class UserQueryResponse : Response
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Genders Gender { get; set; }
        public DateTime? BirthDate { get; set; }
        public DateTime RegistrationDate { get; set; }
        public decimal Score { get; set; }
        public bool IsActive { get; set; }
        public string Address { get; set; }
        public int? CountryId { get; set; }
        public int? CityId { get; set; }
        public int? GroupId { get; set; }
        public List<int> RoleIds { get; set; }

        public string FullName { get; set; }
        public string GenderF { get; set; }
        public string BirthDateF { get; set; }
        public string RegistrationDateF { get; set; }
        public string ScoreF { get; set; }
        public string IsActiveF { get; set; }
        public string GroupF { get; set; }
        public List<string> RolesF { get; set; }
    }

    public class UserQueryHandler : Service<User>, IRequestHandler<UserQueryRequest, IQueryable<UserQueryResponse>>
    {
        public UserQueryHandler(DbContext db) : base(db)
        {
        }

        protected override IQueryable<User> DbSet()
        {
            return base.DbSet()
                .AsNoTracking()
                .Include(u => u.Group)
                .Include(u => u.UserRoles).ThenInclude(ur => ur.Role)
                .OrderByDescending(u => u.IsActive)
                .ThenBy(u => u.RegistrationDate)
                .ThenBy(u => u.UserName);
        }

        public Task<IQueryable<UserQueryResponse>> Handle(UserQueryRequest request, CancellationToken cancellationToken)
        {
            var entityQuery = DbSet();

            if (!string.IsNullOrWhiteSpace(request.UserName))
                entityQuery = entityQuery.Where(u => u.UserName == request.UserName);

            if (request.IsActive.HasValue)
                entityQuery = entityQuery.Where(u => u.IsActive == request.IsActive.Value);

            if (request.CountryId.HasValue)
                entityQuery = entityQuery.Where(u => u.CountryId == request.CountryId.Value);

            if (request.CityId.HasValue)
                entityQuery = entityQuery.Where(u => u.CityId == request.CityId.Value);

            if (request.GroupId.HasValue)
                entityQuery = entityQuery.Where(u => u.GroupId == request.GroupId.Value);

            if (request.RoleIds.Count > 0)
                entityQuery = entityQuery.Where(u => u.UserRoles.Any(ur => request.RoleIds.Contains(ur.RoleId)));

            var query = entityQuery.Select(u => new UserQueryResponse
            {
                Id = u.Id,
                UserName = u.UserName,
                Password = u.Password,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Gender = u.Gender,
                BirthDate = u.BirthDate,
                RegistrationDate = u.RegistrationDate,
                Score = u.Score,
                IsActive = u.IsActive,
                Address = u.Address,
                CountryId = u.CountryId,
                CityId = u.CityId,
                GroupId = u.GroupId,
                RoleIds = u.RoleIds,

                FullName = u.FirstName + " " + u.LastName,
                GenderF = u.Gender.ToString(),
                BirthDateF = u.BirthDate.HasValue ? u.BirthDate.Value.ToString("MM/dd/yyyy") : string.Empty,
                RegistrationDateF = u.RegistrationDate.ToShortDateString(),
                ScoreF = u.Score.ToString("N1"),
                IsActiveF = u.IsActive ? "Active" : "Inactive",
                GroupF = u.Group != null ? u.Group.Title : null,
                RolesF = u.UserRoles.Select(ur => ur.Role.Name).ToList()
            });

            return Task.FromResult(query);
        }
    }
}
