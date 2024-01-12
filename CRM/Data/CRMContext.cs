using Microsoft.EntityFrameworkCore;
using CRM.Models;

namespace CRM.Data
{
    public class CRMContext : DbContext
    {

        protected readonly IConfiguration Configuration;
        public CRMContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        //public void ConfigureServices(IServiceCollection services)
        //{
        //    services.AddDbContext<CRMContext>(options =>
        //    options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));
        //}
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection"));
        }

        public DbSet<User> User { get; set; }
        public DbSet<CRM.Models.Role> Role { get; set; }
        public DbSet<CRM.Models.Business> Business { get; set; }
        public DbSet<CRM.Models.Company> Company { get; set; }
        public DbSet<CRM.Models.Note> Note { get; set; }
        public DbSet<CRM.Models.Contact> Contact { get; set; }

    }
}