using Microsoft.EntityFrameworkCore;
using ReactApp4.Server.Data;
using ReactApp4.Server.Services;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("WebApiDatabase")));

builder.Services.AddScoped<LeagueGameDatabaseHandler>();
builder.Services.AddScoped<LeagueGameFileHandler>();
builder.Services.AddScoped<LeagueGameDataHandler>();

builder.Services.AddScoped<PlayerDatabaseHandler>();
builder.Services.AddScoped<PlayerFileHandler>();
builder.Services.AddScoped<PlayerDataHandler>();

builder.Services.AddScoped<BoxScoreTraditionalDatabaseHandler>();
builder.Services.AddScoped<BoxScoreTraditionalFileHandler>();
builder.Services.AddScoped<BoxScoreTraditionalDataHandler>();

builder.Services.AddScoped<LeagueDashLineupsDataHandler>();
builder.Services.AddScoped<LeagueDashLineupsDatabaseHandler>();
builder.Services.AddScoped<LeagueDashLineupsFileHandler>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();
