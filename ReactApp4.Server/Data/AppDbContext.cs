using CsvHelper.Configuration.Attributes;
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
            //options.UseNpgsql(Configuration.GetConnectionString("WebApiDatabase"));
        }

        public DbSet<Baller> Ballers { get; set; }
        public DbSet<LeagueGame> LeagueGames { get; set; }
        public DbSet<BoxScoreTraditional> BoxScoreTraditionals { get; set; }
        public DbSet<BoxScoreWithGameDate> BoxScoreWithGameDates { get; set; }

        public DbSet<BoxScoreAdvanced> BoxScoreAdvanceds { get; set; }
        public DbSet<BoxScoreFourFactors> BoxScoreFourFactorss { get; set; }
        public DbSet<BoxScoreMisc> BoxScoreMiscs { get; set; }
        public DbSet<BoxScoreScoring> BoxScoreScorings { get; set; }
        public DbSet<BoxScoreSummary> BoxScoreSummarys { get; set; }

        public DbSet<BoxScoreAdvancedPlayer> BoxScoreAdvancedPlayers { get; set; }
        public DbSet<BoxScoreTraditionalPlayer> BoxScoreTraditionalPlayers { get; set; }
        public DbSet<BoxScoreFourFactorsPlayer> BoxScoreFourFactorsPlayers { get; set; }
        public DbSet<BoxScoreMiscPlayer> BoxScoreMiscPlayers { get; set; }
        public DbSet<BoxScoreScoringPlayer> BoxScoreScoringPlayers { get; set; }

        public DbSet<CountOfGamesPlayed> CountOfGamesPlayeds { get; set; }

        public DbSet<SelectedPlayer> SelectedPlayers { get; set; }
        public DbSet<ShotChartsGame> ShotChartsGames { get; set; }
        public DbSet<Shot> Shots { get; set; }
        public DbSet<MLBGame> MLBGames { get; set; }
        public DbSet<MLBPlayerGameBatting> MLBPlayerGamesBatting { get; set; }
        public DbSet<MLBPlayerGamePitching> MLBPlayerGamesPitching { get; set; }
        public DbSet<MLBPlayerGameFielding> MLBPlayerGamesFielding { get; set; }
        public DbSet<MLBActivePlayer> MLBActivePlayers { get; set; }
        public DbSet<MLBStatsBatting> MLBStatsBattings { get; set; }
        public DbSet<MLBStatsBattingWithSplit> MLBStatsBattingWithSplits { get; set; }
        public DbSet<MLBStatsPitching> MLBStatsPitchings { get; set; }
        public DbSet<MLBStatsPitchingSplits> MLBStatsPitchingSplitss { get; set; }
        public DbSet<MLBTeamInfo> MLBTeamInfos { get; set; }
        public DbSet<Play> Plays { get; set; }
        public DbSet<PlayPlayEvents> PlayEvents { get; set; }
        public DbSet<PlayRunners> PlayRunners { get; set; }
        public DbSet<PlayRunnersCredits> PlayRunnersCredits { get; set; }
        public DbSet<MLBBattingBoxScoreWithGameDate> MLBBattingBoxScoreWithGameDates { get; set; }
        public DbSet<MLBPitchingBoxScoreWithGameDate> MLBPitchingBoxScoreWithGameDates { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            // Define table names based on seasons dynamically
            var seasons = new List<string>
            {
                "2015_16",
                "2016_17",
                "2017_18",
                "2018_19",
                "2019_20",
                "2020_21",
                "2021_22",
                "2022_23",
                "2023_24",
                "2024_25"
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
                modelBuilder.Entity<BoxScoreAdvanced>()
                    .ToTable($"box_score_advanced_{season}", schema: "dbo")
                    .HasKey(x => x.Id); // Define the primary key
                modelBuilder.Entity<BoxScoreFourFactors>()
                    .ToTable($"box_score_fourfactors_{season}", schema: "dbo")
                    .HasKey(x => x.Id); // Define the primary key
                modelBuilder.Entity<BoxScoreMisc>()
                    .ToTable($"box_score_misc_{season}", schema: "dbo")
                    .HasKey(x => x.Id); // Define the primary key
                modelBuilder.Entity<BoxScoreScoring>()
                    .ToTable($"box_score_scoring_{season}", schema: "dbo")
                    .HasKey(x => x.Id); // Define the primary key
                modelBuilder.Entity<BoxScoreSummary>()
                    .ToTable($"box_score_summary_{season}", schema: "dbo")
                    .HasKey(x => x.Id); // Define the primary key
                modelBuilder.Entity<Shot>()
                    .ToTable($"shots_{season}", schema: "dbo")
                    .HasKey(x => x.Id); // Define the primary key
            }
            modelBuilder.Entity<BoxScoreAdvancedPlayer>().HasNoKey();
            modelBuilder.Entity<BoxScoreTraditionalPlayer>().HasNoKey();
            modelBuilder.Entity<BoxScoreWithGameDate>().HasNoKey();
            modelBuilder.Entity<BoxScoreFourFactorsPlayer>().HasNoKey();
            modelBuilder.Entity<BoxScoreMiscPlayer>().HasNoKey();
            modelBuilder.Entity<BoxScoreScoringPlayer>().HasNoKey();
            modelBuilder.Entity<CountOfGamesPlayed>().HasNoKey();
            modelBuilder.Entity<SelectedPlayer>().HasNoKey();
            modelBuilder.Entity<ShotChartsGame>().HasNoKey();
            modelBuilder.Entity<MLBGame>().HasNoKey();
            modelBuilder.Entity<MLBPlayerGameBatting>().HasNoKey();
            modelBuilder.Entity<MLBPlayerGamePitching>().HasNoKey();
            modelBuilder.Entity<MLBPlayerGameFielding>().HasNoKey();
            modelBuilder.Entity<MLBActivePlayer>().HasNoKey();
            modelBuilder.Entity<MLBStatsBatting>().HasNoKey();
            modelBuilder.Entity<MLBStatsBattingWithSplit>().HasNoKey();
            modelBuilder.Entity<MLBStatsPitching>().HasNoKey();
            modelBuilder.Entity<MLBStatsPitchingSplits>().HasNoKey();
            modelBuilder.Entity<MLBTeamInfo>().HasNoKey();
            modelBuilder.Entity<Play>().HasNoKey();
            modelBuilder.Entity<PlayPlayEvents>().HasNoKey();
            modelBuilder.Entity<PlayRunners>().HasNoKey();
            modelBuilder.Entity<PlayRunnersCredits>().HasNoKey();
            modelBuilder.Entity<MLBBattingBoxScoreWithGameDate>().HasNoKey();
            modelBuilder.Entity<MLBPitchingBoxScoreWithGameDate>().HasNoKey();



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
