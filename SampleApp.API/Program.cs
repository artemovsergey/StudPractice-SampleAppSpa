using SampleApp.API.Interfaces;
using SampleApp.API.Repositories;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddSingleton<IUserRepository, UsersMemoryRepository>();

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();

app.MapOpenApi();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
