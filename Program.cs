using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Utterly.Areas.Identity.Data;
using Utterly.Data;
namespace Utterly
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var connectionString = builder.Configuration.GetConnectionString("UtterlyContextConnection") ?? throw new InvalidOperationException("Connection string 'UtterlyContextConnection' not found."); ;

            builder.Services.AddDbContext<UtterlyContext>(options => options.UseSqlServer(connectionString));

            builder.Services.AddDefaultIdentity<UtterlyUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<UtterlyContext>();

            // Add services to the container.
            builder.Services.AddRazorPages();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapRazorPages()
               .WithStaticAssets();

            app.Run();
        }
    }
}
