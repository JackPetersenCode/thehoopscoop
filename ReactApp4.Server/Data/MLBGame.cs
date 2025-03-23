using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReactApp4.Server.Data
{
    public class MLBGame
    {
        [Column("game_pk")]
        public int GamePk { get; set; }

        [Column("game_guid")]
        public string? GameGuid { get; set; }

        [Column("link")] // ✅ Added missing field
        public string? Link { get; set; }

        [Column("game_type")]
        public string? GameType { get; set; }

        [Column("season")]
        public string? Season { get; set; }

        [Column("game_date")]
        public DateTime? GameDate { get; set; }

        [Column("official_date")]
        public DateTime? OfficialDate { get; set; }

        [Column("abstract_game_state")]
        public string? AbstractGameState { get; set; }

        [Column("coded_game_state")]
        public string? CodedGameState { get; set; }

        [Column("detailed_state")]
        public string? DetailedState { get; set; }

        [Column("status_code")]
        public string? StatusCode { get; set; }

        [Column("start_time_tbd")]
        public bool? StartTimeTbd { get; set; }

        [Column("abstract_game_code")]
        public string? AbstractGameCode { get; set; }

        [Column("away_team_id")]
        public int AwayTeamId { get; set; }

        [Column("away_team_name")]
        public string? AwayTeamName { get; set; }

        [Column("away_score")]
        public int? AwayScore { get; set; }

        [Column("away_wins")]
        public int? AwayWins { get; set; }

        [Column("away_losses")]
        public int? AwayLosses { get; set; }

        [Column("away_win_pct")]
        public decimal? AwayWinPct { get; set; }

        [Column("away_is_winner")]
        public bool? AwayIsWinner { get; set; }

        [Column("home_team_id")]
        public int HomeTeamId { get; set; }

        [Column("home_team_name")]
        public string? HomeTeamName { get; set; }

        [Column("home_score")]
        public int? HomeScore { get; set; }

        [Column("home_wins")]
        public int? HomeWins { get; set; }

        [Column("home_losses")]
        public int? HomeLosses { get; set; }

        [Column("home_win_pct")]
        public decimal? HomeWinPct { get; set; }

        [Column("home_is_winner")]
        public bool? HomeIsWinner { get; set; }

        [Column("venue_id")]
        public int VenueId { get; set; }

        [Column("venue_name")]
        public string? VenueName { get; set; }

        [Column("is_tie")]
        public bool? IsTie { get; set; }

        [Column("game_number")]
        public int? GameNumber { get; set; }

        [Column("double_header")]
        public string? DoubleHeader { get; set; }

        [Column("day_night")]
        public string? DayNight { get; set; }

        [Column("description")]
        public string? Description { get; set; }

        [Column("scheduled_innings")]
        public int? ScheduledInnings { get; set; }

        [Column("games_in_series")]
        public int? GamesInSeries { get; set; }

        [Column("series_game_number")]
        public int? SeriesGameNumber { get; set; }

        [Column("series_description")]
        public string? SeriesDescription { get; set; }

        [Column("if_necessary")]
        public string? IfNecessary { get; set; }

        [Column("if_necessary_desc")]
        public string? IfNecessaryDesc { get; set; }
    }
}
