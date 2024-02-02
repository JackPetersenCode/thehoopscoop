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
            services.AddScoped<LeagueGameDatabaseHandler>();
            services.AddScoped<LeagueGameFileHandler>();
            services.AddScoped<LeagueGameDataHandler>();

            services.AddScoped<PlayerDatabaseHandler>();
            services.AddScoped<PlayerFileHandler>();
            services.AddScoped<PlayerDataHandler>();

            services.AddScoped<BoxScoreTraditionalDatabaseHandler>();
            services.AddScoped<BoxScoreTraditionalFileHandler>();
            services.AddScoped<BoxScoreTraditionalDataHandler>();

            services.AddScoped<BoxScoreAdvancedDatabaseHandler>();
            services.AddScoped<BoxScoreAdvancedFileHandler>();
            services.AddScoped<BoxScoreAdvancedDataHandler>();

            services.AddScoped<BoxScoreFourFactorsDatabaseHandler>();
            services.AddScoped<BoxScoreFourFactorsFileHandler>();
            services.AddScoped<BoxScoreFourFactorsDataHandler>();

            services.AddScoped<BoxScoreMiscDatabaseHandler>();
            services.AddScoped<BoxScoreMiscFileHandler>();
            services.AddScoped<BoxScoreMiscDataHandler>();

            services.AddScoped<BoxScoreScoringDatabaseHandler>();
            services.AddScoped<BoxScoreScoringFileHandler>();
            services.AddScoped<BoxScoreScoringDataHandler>();

            // Other service registrations might already exist...
            services.AddScoped<LeagueDashLineupsDataHandler>();
            services.AddScoped<LeagueDashLineupsDatabaseHandler>();
            services.AddScoped<LeagueDashLineupsFileHandler>();
        }
    }
}
