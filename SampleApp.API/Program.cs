using Microsoft.EntityFrameworkCore;
using SampleApp.API.Data;
using SampleApp.API.Interfaces;
using SampleApp.API.Repositories;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddSingleton<IUserRepository, UsersMemoryRepository>();
builder.Services.AddCors();

builder.Services.AddDbContext<SampleAppContext>(o => o.UseSqlite(builder.Configuration["SQLite"]));
//builder.Services.AddDbContext<SampleAppContext>(o => o.UseNpgsql(builder.Configuration["PostgreSQL"]));


var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();

app.MapOpenApi();
app.UseHttpsRedirection();
app.UseCors(option => option.AllowAnyOrigin());
app.UseAuthorization();
app.MapControllers();
app.Run();
