using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Threading.RateLimiting;
using WebApi.Data;
using WebApi.Models;
using WebApi.Repositories;
using WebApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = 404;
    //options.AddTokenBucketLimiter("Token", options =>
    //{
    //    options.TokenLimit = 50;
    //    options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
    //    options.AutoReplenishment = true;
    //    options.ReplenishmentPeriod = TimeSpan.FromSeconds(10);
    //    options.TokensPerPeriod = 10;
    //    options.QueueLimit = 10;
    //});
    options.AddFixedWindowLimiter("fixed", options =>
    {
        options.Window = TimeSpan.FromMinutes(1);
        options.PermitLimit = 60;
        options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        options.QueueLimit = 5;
    });
});
builder.Services.AddDbContext<ProContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));
builder.Services.AddHttpClient();
builder.Services.AddScoped<IStocksService, StocksService>();
builder.Services.AddScoped<IUsersRepository, UsersRepository>();
builder.Services.AddSingleton<PasswordHasher<User>>();
builder.Services.AddScoped<IStocksRepository, StocksRepository>();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    var key = builder.Configuration["JWT:SecretKey"] ?? throw new NullReferenceException();
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        ValidAudience = builder.Configuration["JWT:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(s: key)),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true
    };
});
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

//Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.UseRateLimiter();
app.MapControllers();

app.Run();
