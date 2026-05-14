using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Users.APP.Domain
{
    public class UsersDbFactory : IDesignTimeDbContextFactory<UsersDb>
    {
        const string CONNECTIONSTRING = "data source=UsersDB";

        public UsersDb CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<UsersDb>();
            optionsBuilder.UseSqlite(CONNECTIONSTRING);
            return new UsersDb(optionsBuilder.Options);
        }
    }
}
