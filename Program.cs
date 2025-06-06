using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Utterly.Areas.Identity.Data;
using Utterly.DAL;
namespace Utterly;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        var connectionString = builder.Configuration.GetConnectionString("UtterlyContextConnection") ?? throw new InvalidOperationException("Connection string 'UtterlyContextConnection' not found.");

        builder.Services.AddDbContext<UtterlyContext>(options => options.UseSqlServer(connectionString));

        builder.Services.AddDefaultIdentity<UtterlyUser>(options => options.SignIn.RequireConfirmedAccount = true)
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<UtterlyContext>();

        builder.Services.AddHttpClient();
        builder.Services.AddTransient<UtterlyPostAPIManager>();

        // Add services to the container.
        builder.Services.AddRazorPages();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment() || app.Environment.IsStaging())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Error");
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
