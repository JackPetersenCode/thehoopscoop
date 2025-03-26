using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ReactApp4.Server.Data;
using ReactApp4.Server.Services;

namespace ReactApp4.Server
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {

            // Register other services
            services.AddScoped<LeagueGameDataHandler>();
            services.AddScoped<LeagueGameDatabaseHandler>();
            services.AddScoped<LeagueGameFileHandler>();

            services.AddScoped<PlayerDataHandler>();
            services.AddScoped<PlayerDatabaseHandler>();
            services.AddScoped<PlayerFileHandler>();

            services.AddScoped<BoxScoreTraditionalDataHandler>();
            services.AddScoped<BoxScoreTraditionalDatabaseHandler>();
            services.AddScoped<BoxScoreTraditionalFileHandler>();

            services.AddScoped<BoxScoreAdvancedDataHandler>();
            services.AddScoped<BoxScoreAdvancedDatabaseHandler>();
            services.AddScoped<BoxScoreAdvancedFileHandler>();

            services.AddScoped<BoxScoreFourFactorsDataHandler>();
            services.AddScoped<BoxScoreFourFactorsDatabaseHandler>();
            services.AddScoped<BoxScoreFourFactorsFileHandler>();

            services.AddScoped<BoxScoreMiscDataHandler>();
            services.AddScoped<BoxScoreMiscDatabaseHandler>();
            services.AddScoped<BoxScoreMiscFileHandler>();

            services.AddScoped<BoxScoreScoringDataHandler>();
            services.AddScoped<BoxScoreScoringDatabaseHandler>();
            services.AddScoped<BoxScoreScoringFileHandler>();

            services.AddScoped<BoxScoresDataHandler>();
            services.AddScoped<BoxScoresDatabaseHandler>();
            services.AddScoped<BoxScoresFileHandler>();

            services.AddScoped<BoxScoreSummaryDataHandler>();
            services.AddScoped<BoxScoreSummaryDatabaseHandler>();
            services.AddScoped<BoxScoreSummaryFileHandler>();

            // Other service registrations might already exist...
            services.AddScoped<LeagueDashLineupsDataHandler>();
            services.AddScoped<LeagueDashLineupsDatabaseHandler>();
            services.AddScoped<LeagueDashLineupsFileHandler>();

            services.AddScoped<ShotDataHandler>();
            services.AddScoped<ShotDatabaseHandler>();
            services.AddScoped<ShotFileHandler>();

            services.AddScoped<GamblingDataHandler>();
            services.AddScoped<GamblingDatabaseHandler>();
            services.AddScoped<GamblingFileHandler>();

            services.AddScoped<PropBetResultsDatabaseHandler>();
            services.AddScoped<PlayerResultsDatabaseHandler>();

            services.AddScoped<PythonCaller>(); // Register PythonCaller service

            services.AddScoped<MLBGameDataHandler>();
            services.AddScoped<MLBGameDatabaseHandler>();
            services.AddScoped<MLBGameFileHandler>();
            
            services.AddScoped<MLBPlayerGameDataHandler>();
            services.AddScoped<MLBPlayerGameDatabaseHandler>();
            services.AddScoped<MLBPlayerGameFileHandler>();

            services.AddScoped<MLBActivePlayerDataHandler>();
            services.AddScoped<MLBActivePlayerDatabaseHandler>();
            services.AddScoped<MLBActivePlayerFileHandler>();

            services.Configure<KestrelServerOptions>(options =>
            {
                options.Limits.MaxRequestBodySize = 524288000; // 500 MB
            });

            
        }
    }
}
