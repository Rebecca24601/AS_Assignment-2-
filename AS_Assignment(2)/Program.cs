using AS_Assignment_2_.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using AS_Assignment_2_;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddDbContext<AuthDbContext>();
builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<AuthDbContext>();
builder.Services.AddDataProtection();

builder.Services.AddAuthentication("MyCookieAuth").AddCookie("MyCookieAuth", options
=>
{
    options.Cookie.Name = "MyCookieAuth";

    options.AccessDeniedPath = "/Account/AccessDenied";
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("MustBelongToHRDepartment",
        policy => policy.RequireClaim("Department", "HR"));
});

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddDistributedMemoryCache(); //save session in memory
builder.Services.AddSingleton<SessionManager>();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(3);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.ConfigureApplicationCookie(Config =>
{
	Config.LoginPath = "/Login";
});

builder.Services.AddLogging(builder =>
{
    builder.AddConsole();
});





var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseStatusCodePagesWithRedirects("/errors/{0}");



app.UseRouting();

app.UseSession();



app.UseAuthentication();

app.UseAuthorization();

//causing redirect error

//app.Use(async (context, next) =>
//{
    // Check if there is an active session
    //if (context.Session.IsAvailable)
    //{
        //DateTime lastActivity = context.Session.GetString("LastActivity") != null
            //? DateTime.Parse(context.Session.GetString("LastActivity"))
            //: DateTime.MinValue;

        //TimeSpan sessionTimeout = TimeSpan.FromMinutes(3);

        //if (DateTime.Now - lastActivity > sessionTimeout)
        //{
            // Session has expired, redirect to login
            //context.Response.Redirect("/Login");
            //return;
        //}
        //else
        //{
            // Update last activity time in the session
            //context.Session.SetString("LastActivity", DateTime.Now.ToString("O"));
        //}
    //}

    //await next();
//});





app.MapRazorPages();

app.Run();


