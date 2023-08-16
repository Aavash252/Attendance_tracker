using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using FinalProject.Areas.Identity.Data;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var connectionString = builder.Configuration.GetConnectionString("FinalProjectDbContextConnection") ?? throw new InvalidOperationException("Connection string 'FinalProjectDbContextConnection' not found.");

        builder.Services.AddDbContext<FinalProjectDbContext>(options =>
            options.UseSqlServer(connectionString));



        builder.Services.AddControllersWithViews();


        builder.Services.AddDefaultIdentity<FinalProjectUser>(options => options.SignIn.RequireConfirmedAccount = false)
             .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<FinalProjectDbContext>();


        // Add services to the container.
        


        builder.Services.AddRazorPages();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
        }
        app.UseStaticFiles();

        app.UseRouting();
        app.UseAuthentication(); ;

        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");


        app.MapRazorPages();



        using (var scope = app.Services.CreateScope())
        {
            var roleManager =
                scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            var roles = new[] { "Admin", "Manager", "Member" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));
            }

        }

        using (var scope = app.Services.CreateScope())
        {
            var userManager =
                scope.ServiceProvider.GetRequiredService<UserManager<FinalProjectUser>>();

            string email = "admin@admin.com";
            string password = "Imatesp@3";
            


            if (await userManager.FindByEmailAsync(email) == null)
            {
                var user = new FinalProjectUser();
                user.Email = email;
                user.UserName = email;


                await userManager.CreateAsync(user, password);

                await userManager.AddToRoleAsync(user, "Admin");


            }



        }


        app.Run();
    }
}