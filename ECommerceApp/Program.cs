using App.DataAccess.Data;
using App.DataAccess.Repository;
using App.DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using App.Utility;
using Microsoft.AspNetCore.Identity.UI.Services;
using App.DataAccess.DBInitializer;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        var connectionStirng = builder.Configuration.GetConnectionString("DefaultConnection");

        // Add services to the container.
        builder.Services.AddControllersWithViews();

        builder.Services.AddDbContext<ApplicationDBContext>(options => options.UseSqlServer(connectionStirng));

        builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDBContext>().AddDefaultTokenProviders();

        builder.Services.ConfigureApplicationCookie(options =>
        {
            options.LoginPath = $"/Identity/Account/Login";
            options.LogoutPath = $"/Identity/Account/Logout";
            options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
        });

        builder.Services.AddAuthentication().AddFacebook(option =>
        {
            option.AppId = "654256874258271";
            option.AppSecret = "6b464c204e2de816e3bb87366dc2a3cf";
        });

        builder.Services.AddDistributedMemoryCache();
        builder.Services.AddSession(options =>
        {
            options.IdleTimeout = TimeSpan.FromMinutes(100);
            options.Cookie.HttpOnly = true;
            options.Cookie.IsEssential = true;
        });

        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
        builder.Services.AddScoped<IEmailSender, EmailSender>();
        builder.Services.AddScoped<IDBInitializer, DBInitializer>();
        builder.Services.AddRazorPages();

        var app = builder.Build();

        #region Request Pipeline
        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();
        app.UseAuthentication(); //must add before authorization.
        app.UseAuthorization();
        app.UseSession();
        await SeedDatabaseAsync();

        app.MapRazorPages();
        app.MapControllerRoute(
            name: "default",
            pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");

        app.Run();
        #endregion

        async Task SeedDatabaseAsync()
        {
            using (var scope = app.Services.CreateScope())
            {
                var dbInitializer = scope.ServiceProvider.GetRequiredService<IDBInitializer>();
                await dbInitializer.InitializeAsync();
            }
        }
    }
}


