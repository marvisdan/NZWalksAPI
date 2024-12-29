using Microsoft.EntityFrameworkCore;
using NZWalksAPI.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Identity;
using NZWalksAPI.Repositories;
using Microsoft.OpenApi.Models;
using System.Collections.Generic;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Extensions;
using Microsoft.Extensions.FileProviders;
using Serilog;
using NZWalksAPI.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add logging
var logger = new LoggerConfiguration()
    .WriteTo.Console() // where to log
    .WriteTo.File("Logs/NZWalks_Logs.txt", rollingInterval: RollingInterval.Minute) // log to file
    .MinimumLevel.Information()
    .CreateLogger();

builder.Logging.ClearProviders(); // clear default logging providers
builder.Logging.AddSerilog(logger); // add Serilog logging

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "NZWalks API", Version = "v1" });
    options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
    {
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = JwtBearerDefaults.AuthenticationScheme
    }); ;
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = JwtBearerDefaults.AuthenticationScheme
                },
                Scheme = "Oauth2",
                Name = JwtBearerDefaults.AuthenticationScheme,
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
});


builder.Services.AddDbContext<NZWalksDBContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("NZWalksConnectionString"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("NZWalksConnectionString"))
    ));

builder.Services.AddDbContext<NZWalksAuthDBContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("NZWalksAuthConnectionString"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("NZWalksAuthConnectionString"))
    ));

// add region repository
builder.Services.AddScoped<IRegionRepository, SQLRegionRepository>();

// Add walk repository
builder.Services.AddScoped<IWalkRepository, SQLWalkRepository>();

// Add Token Repository
builder.Services.AddScoped<ITokenRepository, TokenRepository>();

// Add Image Repository
builder.Services.AddScoped<IImageRepository, LocalImageRepository>();

// Add AutoMapper
builder.Services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);

// Add Identity services to manage users and roles
builder.Services.AddIdentityCore<IdentityUser>()
.AddRoles<IdentityRole>()
.AddTokenProvider<DataProtectorTokenProvider<IdentityUser>>("NZWalks")
.AddEntityFrameworkStores<NZWalksAuthDBContext>()
.AddDefaultTokenProviders();

// Configure the Identity options
builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;
});


// Add Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters
{
    ValidateIssuer = true,
    ValidateAudience = true,
    ValidateLifetime = true,
    ValidateIssuerSigningKey = true,
    ValidIssuer = builder.Configuration["Jwt:Issuer"],
    ValidAudience = builder.Configuration["Jwt:Audience"],
    IssuerSigningKey = new SymmetricSecurityKey(
        Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
// Add Exception handler middleware
app.UseMiddleware<ExceptionhandlerMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

// Serve static files from the Images folder
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Images")),
    RequestPath = "/Images",
});

app.MapControllers();
app.MapGet("/", () => "Hello World!");

app.Run();
