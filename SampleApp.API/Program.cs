using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SampleApp.API.Data;
using SampleApp.API.Extensions;
using SampleApp.API.Interfaces;
using SampleApp.API.Repositories;
using SampleApp.API.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IUserRepository, UserLocalRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddDbContext<SampleAppContext>(o => o.UseNpgsql(builder.Configuration["ConnectionStrings:PostgreSQL"]));
//builder.Services.AddDbContext<SampleAppContext>(o => o.UseSqlServer(builder.Configuration["ConnectionStrings:DefaultConnection"]));

builder.Services.AddScoped<ITokenService, TokenService>();

builder.Services.AddCors();

builder.Services.AddJwtServices(builder.Configuration);

builder.Services.AddAuthorization();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();

app.UseCors(o =>o.AllowAnyOrigin());

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();
