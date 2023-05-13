using Amazon.S3;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using UsluzniOglasi.Application.Services;
using UsluzniOglasi.Domain.Models;
using UsluzniOglasi.Infrastructure.Context;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddSingleton<IUserImageService, UserImageService>();

//Autofac
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

//Configure AmazonS3
var s3Config = new AmazonS3Config
{
    ServiceURL = builder.Configuration.GetSection("AWS:ServiceURL").Value,
    ForcePathStyle = true,
    RegionEndpoint = Amazon.RegionEndpoint.EUNorth1
};
builder.Host.ConfigureContainer<ContainerBuilder>(builder =>
{
    builder.RegisterType<AmazonS3Client>()
    .As<IAmazonS3>()
    .WithParameter(new TypedParameter(typeof(AmazonS3Config), s3Config))
    .InstancePerLifetimeScope();
});


//Database connection
builder.Services.AddDbContext<UserDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("UserDbConnection")));

//Configuring Identity
builder.Services.AddIdentity<AppUser, IdentityRole>()
    .AddEntityFrameworkStores<UserDbContext>()
    .AddDefaultTokenProviders();

//Without this configuration API calls that need authorization return 404 
builder.Services.ConfigureApplicationCookie(o =>
{
    o.Events = new CookieAuthenticationEvents()
    {
        OnRedirectToLogin = (ctx) =>
        {
            if (ctx.Request.Path.StartsWithSegments("/api") && ctx.Response.StatusCode == 200)
            {
                ctx.Response.StatusCode = 401;
            }

            return Task.CompletedTask;
        },
        OnRedirectToAccessDenied = (ctx) =>
        {
            if (ctx.Request.Path.StartsWithSegments("/api") && ctx.Response.StatusCode == 200)
            {
                ctx.Response.StatusCode = 403;
            }

            return Task.CompletedTask;
        }
    };
});

builder.Services.AddCors(options =>
    options.AddPolicy("DevelopPolicy", options =>
        options.WithOrigins("http://localhost:4200")
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials()
        ));

var app = builder.Build();

if (args.Length == 1 && args[0].ToLower() == "seeddata")
{
    await Seed.SeedUsersAndRolesAsync(app);
    //Seed.SeedData(app);
}
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();
app.UseCors("DevelopPolicy");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
