using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using University.Data;
using University.Web.Areas.Identity.Pages.Account.Manage;

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
                        context.Configuration.GetConnectionString("UniversityContextConnection")));

				services.AddTransient<IEmailSender, EmailSender>();

				services.AddIdentity<IdentityUser, IdentityRole>()
					.AddEntityFrameworkStores<UniversityContext>().AddDefaultTokenProviders();
			});
        }
    }
}