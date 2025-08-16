using Microsoft.EntityFrameworkCore;
using Swim_Feedback.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

namespace Swim_Feedback_Tests
{
    internal class MyDbContextFactory : IDbContextFactory<ApplicationDbContext>
    {
        private DbContextOptionsBuilder<ApplicationDbContext> optionsBuilder = new();

        public MyDbContextFactory()
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder();
            string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            optionsBuilder.UseSqlServer(connectionString);
        }

        public ApplicationDbContext CreateDbContext()
        {
            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}
