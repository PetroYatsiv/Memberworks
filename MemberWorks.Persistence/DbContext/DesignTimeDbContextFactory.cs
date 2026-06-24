using MemberWorks.Application.Common.Interfaces;
using MemberWorks.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace MemberWorks.Persistence.Migrations;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var connectionString = Environment.GetEnvironmentVariable("MEMBERWORKS_DB")
                               ?? "Host=localhost;Database=memberworks;Username=memberworks;Password=memberworks";

        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseNpgsql(connectionString, b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName))
            .Options;

        return new ApplicationDbContext(options, new DesignTimeCurrentUser());
    }
    
    private sealed class DesignTimeCurrentUser : ICurrentUserService
    {
        public Guid? UserId => null;
        public Guid? OrganizationId => null;
        public ApplicationRole? Role => null;
        public bool IsAuthenticated => false;
    }
}