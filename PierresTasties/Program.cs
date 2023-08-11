using AspNetCoreRateLimit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using PierresTasties.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Read configuration for rate limiter.
var configuration = builder.Configuration;

// Configure AspNetCoreRateLimit.
builder.Services.AddMemoryCache();
builder.Services.Configure<IpRateLimitOptions>(configuration.GetSection("IpRateLimiting"));
builder.Services.Configure<IpRateLimitPolicies>(configuration.GetSection("IpRateLimitPolicies"));
builder.Services.AddInMemoryRateLimiting();
builder.Services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
builder.Services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

// Context builder.
builder.Services.AddDbContext<PierresTastiesContext>(
  dbContextOptions => dbContextOptions
    .UseMySql(
      builder.Configuration["ConnectionStrings:DefaultConnection"], ServerVersion.AutoDetect(builder.Configuration["ConnectionStrings:DefaultConnection"]
    )
  )
);

// Auth builder.
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
          .AddEntityFrameworkStores<PierresTastiesContext>()
          .AddDefaultTokenProviders()
          .AddRoles<IdentityRole>();

// DEVELOPMENT ONLY removes pw requirements to make account creation easier for development.
// IN PRODUCTION UNCOMMENT PW annotation in `RegisterViewModel.cs`.
builder.Services.Configure<IdentityOptions>(options =>
{
  options.Password.RequireDigit = false;
  options.Password.RequireLowercase = false;
  options.Password.RequireNonAlphanumeric = false;
  options.Password.RequireUppercase = false;
  options.Password.RequiredLength = 0;
  options.Password.RequiredUniqueChars = 0;
});

// Define auth tags for controller routes.
// Example tags below:
// [Authorize(Policy = "RequireAdministratorRole")]
// [Authorize(Roles = "Admin,Patron")]
builder.Services.AddAuthorization(options =>
{
  options.AddPolicy("RequireAdministratorRole",
    policy => policy.RequireRole("Admin"));
  options.AddPolicy("RequirePatronRole",
    policy => policy.RequireRole("Patron"));
});


var app = builder.Build();

// FOR DEVELOPMENT ONLY.
DataInitializer.InitializeData(app);

// Initialize auth roles.
RolesInitializer.InitializeRoles(app);

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

// Add Auth to app.
app.UseAuthentication();
app.UseAuthorization();

// AspNetCoreRateLimit.
app.UseIpRateLimiting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
