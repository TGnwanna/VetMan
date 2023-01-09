using Core.Config;
using Core.Db;
using Core.Models;
using Hangfire;
using Hangfire.Dashboard;
using Hangfire.SqlServer;
using Logic;
using Logic.Helpers;
using Logic.Helpers.Treatments;
using Logic.IHelpers;
using Logic.IHelpers.Treatments;
using Logic.Services;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IUserHelper, UserHelper>();
builder.Services.AddScoped<IAdminHelper, AdminHelper>();
builder.Services.AddScoped<ICompanyHelper, CompanyHelper>();
builder.Services.AddScoped<IEmailHelper, EmailHelper>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<ISalesHelper, SalesHelper>();
builder.Services.AddScoped<IBookingHelper, BookingHelper>();
builder.Services.AddScoped<ISuperAdminHelper, SuperAdminHelper>();
builder.Services.AddScoped<IVaccineHelper, VaccineHelper>();
builder.Services.AddScoped<ICustomerPaymentHelper, CustomerPaymentHelper>();
builder.Services.AddScoped<IWalletHelper, WalletHelper>();
builder.Services.AddScoped<IDropdownHelper, DropdownHelper>();
builder.Services.AddScoped<IPaystackHelper, PaystackHelper>();
builder.Services.AddScoped<IRobotHelper, RobotHelper>();
builder.Services.AddScoped<ISubscriptionHelper, SubscriptionHelper>();
builder.Services.AddScoped<IPatientHelper, PatientHelper>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddSingleton<IEmailConfiguration>(builder.Configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>());
builder.Services.AddSingleton<IGeneralConfiguration>(builder.Configuration.GetSection("GeneralConfiguration").Get<GeneralConfiguration>());
builder.Services.AddHangfire(x => x.UseSqlServerStorage(builder.Configuration.GetConnectionString("VetManHangFire")));
builder.Services.AddTransient<IAppVersionService, AppVersionService>();
builder.Services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Latest);

builder.Services.AddSession(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.IdleTimeout = TimeSpan.FromMinutes(43800);
});

builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("VetMan")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 3;
    options.Password.RequiredUniqueChars = 0;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;

}).AddEntityFrameworkStores<AppDbContext>();

builder.Services.Configure<FormOptions>(x =>
{
    x.ValueLengthLimit = int.MaxValue;
    x.MultipartBodyLengthLimit = int.MaxValue;
    x.MultipartHeadersLengthLimit = int.MaxValue;
});

builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;

});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}




app.UseStatusCodePages(async context =>
{
    var response = context.HttpContext.Response;

    if (response.StatusCode == (int)HttpStatusCode.Unauthorized)
        response.Redirect("/Account/Login");

    if (response.StatusCode == (int)HttpStatusCode.NotFound)
        response.Redirect("/Home/Error");

    if (response.StatusCode == (int)HttpStatusCode.Forbidden)
        response.Redirect("/Home/Error");

    if (response.StatusCode == (int)HttpStatusCode.BadGateway)
        response.Redirect("/Home/Error");

    if (response.StatusCode == (int)HttpStatusCode.InternalServerError)
        response.Redirect("/Home/Error");
});

app.UseForwardedHeaders();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthentication();
app.UseSession();
UpdateDatabase(app);
app.UseRouting();
HangFireConfiguration(app);
app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var _robot = services.GetRequiredService<IRobotHelper>();
        _robot.Start2();
        _robot.NotifySubscribers();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while see the database.");
    }
}
app.Run();
static void UpdateDatabase(IApplicationBuilder app)
{
    using (var serviceScope = app.ApplicationServices
        .GetRequiredService<IServiceScopeFactory>()
        .CreateScope())
    {
        using (var context = serviceScope.ServiceProvider.GetService<AppDbContext>())
        {
            context?.Database.Migrate();
        }
    }
}
void HangFireConfiguration(IApplicationBuilder app)
{
    var robotDashboardOptions = new DashboardOptions { Authorization = new[] { new MyAuthorizationFilter() } };
    //Enable Session.

    AppHttpContext.Services = app.ApplicationServices;
    var robotOptions = new BackgroundJobServerOptions
    {
        ServerName = String.Format(
        "{0}.{1}",
        Environment.MachineName,
        Guid.NewGuid().ToString())
    };
    app.UseHangfireServer(robotOptions);
    var RobotStorage = new SqlServerStorage(builder.Configuration.GetConnectionString("VetManHangFire"));
    JobStorage.Current = RobotStorage;
    app.UseHangfireDashboard("/VetManEmails", robotDashboardOptions, RobotStorage);
}

class MyAuthorizationFilter : IDashboardAuthorizationFilter
{
    public bool Authorize(DashboardContext context)
    {
        return true;
    }
}

