

using System.Text;
using FUNewsManagement_AnalyticsAPI.BLL.Implements;
using FUNewsManagement_AnalyticsAPI.BLL.Interfaces;
using FUNewsManagement_AnalyticsAPI.DAL.Repositories.Implements;
using FUNewsManagement_AnalyticsAPI.DAL.Repositories.Interfaces;
using FUNewsManagement_CoreAPI.DAL.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Config
var configuration = builder.Configuration;

// Services.
builder.Services.AddDbContext<FUNMSDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("SqlServer")));

// Repo
builder.Services.AddTransient<INewsArticleRepository, NewsArticleRepository>();

// Services
builder.Services.AddTransient<INewsArticleService, NewsArticleService>();

var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

builder.Services.AddHttpContextAccessor();

builder.Services.AddAuthorization();

// Odata
builder.Services.AddControllers();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        b => b.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader());
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();