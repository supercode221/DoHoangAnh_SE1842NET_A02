using System.Text;
using FUNewsManagement_CoreAPI.BLL.Services.Implements;
using FUNewsManagement_CoreAPI.BLL.Services.Interfaces;
using FUNewsManagement_CoreAPI.DAL.Data;
using FUNewsManagement_CoreAPI.DAL.Entities;
using FUNewsManagement_CoreAPI.DAL.Repositories.Implements;
using FUNewsManagement_CoreAPI.DAL.Repositories.Interfaces;
using FUNFUNewsManagement_CoreAPIMS.DAL.Repositories.Interfaces;
using FUNMS.DAL.Repositories.Implements;
using FUNMS.DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OData.ModelBuilder;

var builder = WebApplication.CreateBuilder(args);

// Config
var configuration = builder.Configuration;

// Services.
builder.Services.AddDbContext<FUNMSDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("SqlServer")));

// Repo
builder.Services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddTransient<ISystemAccountRepository, SystemAccountRepository>();
builder.Services.AddTransient<INewsArticleRepository, NewsArticleRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ITagRepository, TagRepository>();

// Services
builder.Services.AddTransient<ISystemAccountService, SystemAccountService>();
builder.Services.AddTransient<AuthService>();
builder.Services.AddTransient<INewsService, NewsService>();
builder.Services.AddTransient<ICategoryService, CategoryService>();
builder.Services.AddTransient<ITagService, TagService>();

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

builder.Services.AddAuthorization();

// Odata
builder.Services.AddControllers().AddOData(
    options => options
        .Select()
        .Filter()
        .OrderBy()
        .Expand()
        .Count()
        .SetMaxTop(100)
        .AddRouteComponents("api", GetEdmModel())
    );

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

static Microsoft.OData.Edm.IEdmModel GetEdmModel()
{
    var builder = new ODataConventionModelBuilder();

    builder.EntitySet<SystemAccount>("account");
    builder.EntitySet<Category>("category");
    builder.EntitySet<NewsArticle>("news");
    builder.EntitySet<Tag>("tag");

    return builder.GetEdmModel();
}