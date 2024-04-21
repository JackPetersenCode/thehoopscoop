using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Python.Runtime;
using ReactApp4.Server.Data;
using ReactApp4.Server.Services;
using System;

namespace ReactApp4.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            //Thread pythonThread = new Thread(ExecutePythonCode);
            //pythonThread.Start();
            var builder = WebApplication.CreateBuilder(args);

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

            builder.Services.AddScoped<BoxScoreSummaryDataHandler>();
            builder.Services.AddScoped<BoxScoreSummaryDatabaseHandler>();
            builder.Services.AddScoped<BoxScoreSummaryFileHandler>();

            builder.Services.AddScoped<LeagueDashLineupsDataHandler>();
            builder.Services.AddScoped<LeagueDashLineupsDatabaseHandler>();
            builder.Services.AddScoped<LeagueDashLineupsFileHandler>();

            builder.Services.AddScoped<ShotDataHandler>();
            builder.Services.AddScoped<ShotDatabaseHandler>();
            builder.Services.AddScoped<ShotFileHandler>();

            builder.Services.AddScoped<GamblingDataHandler>();
            builder.Services.AddScoped<GamblingDatabaseHandler>();
            builder.Services.AddScoped<GamblingFileHandler>();

            builder.Services.AddScoped<PropBetResultsDatabaseHandler>();
            builder.Services.AddScoped<PlayerResultsDatabaseHandler>();

            builder.Services.AddScoped<PythonCaller>(); // Register PythonCaller service



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
        }
        //static void ExecutePythonCode()
        //{
        //    Runtime.PythonDLL = @"C:\Python312\python312.dll";
        //    // Initialize Python engine
        //    PythonEngine.Initialize();
        //
        //    // Add directory containing testScript.py to the Python path
        //    string scriptDirectory = @"C:\Users\jackp\Desktop\ReactApp4\ReactApp4.Server";
        //    string pythonCode = $"import sys; sys.path.append(r'{scriptDirectory}')";
        //
        //    // Execute the Python code
        //    PythonEngine.Exec(pythonCode);
        //
        //    // Import the Python script
        //    dynamic exampleScript = Py.Import("testScript");
        //
        //    // Call the sayHello function from the Python script periodically
        //
        //    var result = exampleScript.sayHello();
        //    Console.WriteLine($"Result of calling Python function: {result}");
        //
        //    // Sleep for some time before calling again
        //}
    }
}