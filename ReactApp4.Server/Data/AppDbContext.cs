using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ReactApp4.Server.Data
{
    public class AppDbContext : DbContext
    {
        protected readonly IConfiguration Configuration;

        public AppDbContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            // connect to postgres with connection string from app settings
            options.UseNpgsql(Configuration.GetConnectionString("WebApiDatabase")).LogTo(Console.WriteLine, LogLevel.Information);
        }

        public DbSet<Baller> Ballers { get; set; }
        public DbSet<LeagueGame> LeagueGames { get; set; }
        public DbSet<BoxScoreTraditional> BoxScoreTraditionals { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Define table names based on seasons dynamically
            var seasons = new List<string>
        {
            "2015_2016",
            "2016_2017",
            "2017_2018",
            "2018_2019",
            "2019_2020",
            "2020_2021",
            "2021_2022",
            "2022_2023",
            "2023_2024"
            // Add other season identifiers...
        };

            foreach (var season in seasons)
            {
                modelBuilder.Entity<LeagueGame>()
                    .ToTable($"league_games_{season}", schema: "dbo")
                    .HasKey(x => x.Id); // Define the primary key
                modelBuilder.Entity<BoxScoreTraditional>()
                    .ToTable($"box_score_traditional_{season}", schema: "dbo")
                    .HasKey(x => x.Id); // Define the primary key
            }
        }
        public DbSet<TableLength> TableLengths { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<LeagueDashLineupAdvanced> LeagueDashLineupAdvanceds { get; set; }
        public DbSet<LeagueDashLineupBase> LeagueDashLineupBases { get; set; }
        public DbSet<LeagueDashLineupFourFactors> LeagueDashLineupFourFactors { get; set; }
        public DbSet<LeagueDashLineupMisc> LeagueDashLineupMiscs { get; set; }
        public DbSet<LeagueDashLineupScoring> LeagueDashLineupScorings { get; set; }
        public DbSet<LeagueDashLineupOpponent> LeagueDashLineupOpponents { get; set; }

    }
}
