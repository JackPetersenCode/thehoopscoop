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

            // Other service registrations might already exist...
        }
    }
}
