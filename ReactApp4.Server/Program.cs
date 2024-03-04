using Microsoft.EntityFrameworkCore;
using ReactApp4.Server.Data;
using ReactApp4.Server.Services;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("WebApiDatabase")));

builder.Services.AddScoped<LeagueGameDataHandler>();
builder.Services.AddScoped<LeagueGameDatabaseHandler>();
builder.Services.AddScoped<LeagueGameFileHandler>();

builder.Services.AddScoped<PlayerDataHandler>();
builder.Services.AddScoped<PlayerDatabaseHandler>();
builder.Services.AddScoped<PlayerFileHandler>();

builder.Services.AddScoped<BoxScoreTraditionalDataHandler>();
builder.Services.AddScoped<BoxScoreTraditionalDatabaseHandler>();
builder.Services.AddScoped<BoxScoreTraditionalFileHandler>();

builder.Services.AddScoped<BoxScoreAdvancedDataHandler>();
builder.Services.AddScoped<BoxScoreAdvancedDatabaseHandler>();
builder.Services.AddScoped<BoxScoreAdvancedFileHandler>();

builder.Services.AddScoped<BoxScoreFourFactorsDataHandler>();
builder.Services.AddScoped<BoxScoreFourFactorsDatabaseHandler>();
builder.Services.AddScoped<BoxScoreFourFactorsFileHandler>();

builder.Services.AddScoped<BoxScoreMiscDataHandler>();
builder.Services.AddScoped<BoxScoreMiscDatabaseHandler>();
builder.Services.AddScoped<BoxScoreMiscFileHandler>();

builder.Services.AddScoped<BoxScoreScoringDataHandler>();
builder.Services.AddScoped<BoxScoreScoringDatabaseHandler>();
builder.Services.AddScoped<BoxScoreScoringFileHandler>();

builder.Services.AddScoped<BoxScoresDataHandler>();
builder.Services.AddScoped<BoxScoresDatabaseHandler>();
builder.Services.AddScoped<BoxScoresFileHandler>();

builder.Services.AddScoped<LeagueDashLineupsDataHandler>();
builder.Services.AddScoped<LeagueDashLineupsDatabaseHandler>();
builder.Services.AddScoped<LeagueDashLineupsFileHandler>();

builder.Services.AddScoped<PropBetResultsDatabaseHandler>();

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
