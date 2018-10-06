using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using University.Data;

[assembly: HostingStartup(typeof(University.Web.Areas.Identity.IdentityHostingStartup))]
namespace University.Web.Areas.Identity
{
	public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<UniversityContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("UniversityWebContextConnection")));

                services.AddDefaultIdentity<IdentityUser>()
                    .AddEntityFrameworkStores<UniversityContext>();
            });
        }
    }
}