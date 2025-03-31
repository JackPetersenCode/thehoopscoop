using System.ComponentModel.DataAnnotations.Schema;

namespace ReactApp4.Server.Data
{
    public class MLBTeamInfo
    {
        [Column("game_pk")] public int GamePk { get; set; }
        [Column("team_side")] public string TeamSide { get; set; } = "";
        [Column("team_id")] public int TeamId { get; set; }
        [Column("team_name")] public string TeamName { get; set; } = "";
        [Column("season")] public string Season { get; set; } = "";
        [Column("spring_league_id")] public int? SpringLeagueId { get; set; }
        [Column("spring_league_name")] public string SpringLeagueName { get; set; } = "";
        [Column("spring_league_abbreviation")] public string SpringLeagueAbbreviation { get; set; } = "";
        [Column("all_star_status")] public string AllStarStatus { get; set; } = "";
        [Column("venue_id")] public int? VenueId { get; set; }
        [Column("venue_name")] public string VenueName { get; set; } = "";
        [Column("team_code")] public string TeamCode { get; set; } = "";
        [Column("file_code")] public string FileCode { get; set; } = "";
        [Column("abbreviation")] public string Abbreviation { get; set; } = "";
        [Column("location_name")] public string LocationName { get; set; } = "";
        [Column("first_year_of_play")] public string FirstYearOfPlay { get; set; } = "";
        [Column("league_id")] public int? LeagueId { get; set; }
        [Column("league_name")] public string LeagueName { get; set; } = "";
        [Column("sport_id")] public int? SportId { get; set; }
        [Column("sport_name")] public string SportName { get; set; } = "";
        [Column("short_name")] public string ShortName { get; set; } = "";
        [Column("record_games_played")] public int? RecordGamesPlayed { get; set; }
        [Column("record_wild_card_games_back")] public string RecordWildCardGamesBack { get; set; } = "";
        [Column("record_league_games_back")] public string RecordLeagueGamesBack { get; set; } = "";
        [Column("record_spring_league_games_back")] public string RecordSpringLeagueGamesBack { get; set; } = "";
        [Column("record_sport_games_back")] public string RecordSportGamesBack { get; set; } = "";
        [Column("record_division_games_back")] public string RecordDivisionGamesBack { get; set; } = "";
        [Column("record_conference_games_back")] public string RecordConferenceGamesBack { get; set; } = "";
        [Column("record_league_record_wins")] public int? RecordLeagueRecordWins { get; set; }
        [Column("record_league_record_losses")] public int? RecordLeagueRecordLosses { get; set; }
        [Column("record_league_record_ties")] public int? RecordLeagueRecordTies { get; set; }
        [Column("record_league_record_pct")] public decimal? RecordLeagueRecordPct { get; set; }
        [Column("record_division_leader")] public bool? RecordDivisionLeader { get; set; }
        [Column("record_wins")] public int? RecordWins { get; set; }
        [Column("record_losses")] public int? RecordLosses { get; set; }
        [Column("record_winning_percentage")] public decimal? RecordWinningPercentage { get; set; }
        [Column("franchise_name")] public string FranchiseName { get; set; } = "";
        [Column("club_name")] public string ClubName { get; set; } = "";
        [Column("active")] public bool Active { get; set; }
    }
}
