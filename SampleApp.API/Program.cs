using Microsoft.EntityFrameworkCore;
using SampleApp.API.Data;
using SampleApp.API.Entities;
using SampleApp.API.Extensions;
using SampleApp.API.Interfaces;
using SampleApp.API.Repositories;
using SampleApp.API.Services;
using SampleApp.Repositories;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddOpenApi();

// builder.Services.AddSingleton<IUserRepository, UsersMemoryRepository>();
builder.Services.AddScoped<IUserRepository, UsersLocalRepository>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<BaseRepository<Micropost>>();
builder.Services.AddScoped<BaseRepository<Role>>();


builder.Services.AddCors();
builder.Services.AddDbContext<SampleAppContext>(o => o.UseSqlite(builder.Configuration.GetConnectionString("SQLite")));
//builder.Services.AddDbContext<SampleAppContext>(o => o.UseNpgsql(builder.Configuration["ConnectionStrings:PostgreSQL"]));

builder.Services.AddJwtServices(builder.Configuration);

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();

app.MapOpenApi();
// app.UseHttpsRedirection();
app.UseCors(option => option.AllowAnyOrigin().AllowAnyHeader());
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
