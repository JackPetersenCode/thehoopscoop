using CsvHelper.Configuration;
using ReactApp4.Server.Data;

public sealed class MLBTeamInfoMap : ClassMap<MLBTeamInfo>
{
    public MLBTeamInfoMap()
    {
        Map(m => m.GamePk).Name("gamePk");
        Map(m => m.TeamSide).Name("teamSide");
        Map(m => m.TeamId).Name("teamId");
        Map(m => m.TeamName).Name("teamName");
        Map(m => m.Season).Name("season");
        Map(m => m.SpringLeagueId).Name("spring_league_id").Optional();
        Map(m => m.SpringLeagueName).Name("spring_league_name");
        Map(m => m.SpringLeagueAbbreviation).Name("spring_league_abbreviation");
        Map(m => m.AllStarStatus).Name("all_star_status");
        Map(m => m.VenueId).Name("venue_id").Optional();
        Map(m => m.VenueName).Name("venue_name");
        Map(m => m.TeamCode).Name("team_code");
        Map(m => m.FileCode).Name("file_code");
        Map(m => m.Abbreviation).Name("abbreviation");
        Map(m => m.LocationName).Name("location_name");
        Map(m => m.FirstYearOfPlay).Name("first_year_of_play");
        Map(m => m.LeagueId).Name("league_id").Optional();
        Map(m => m.LeagueName).Name("league_name");
        Map(m => m.SportId).Name("sport_id").Optional();
        Map(m => m.SportName).Name("sport_name");
        Map(m => m.ShortName).Name("short_name");
        Map(m => m.RecordGamesPlayed).Name("record_games_played").Optional();
        Map(m => m.RecordWildCardGamesBack).Name("record_wild_card_games_back");
        Map(m => m.RecordLeagueGamesBack).Name("record_league_games_back");
        Map(m => m.RecordSpringLeagueGamesBack).Name("record_spring_league_games_back");
        Map(m => m.RecordSportGamesBack).Name("record_sport_games_back");
        Map(m => m.RecordDivisionGamesBack).Name("record_division_games_back");
        Map(m => m.RecordConferenceGamesBack).Name("record_conference_games_back");
        Map(m => m.RecordLeagueRecordWins).Name("record_league_record_wins").Optional();
        Map(m => m.RecordLeagueRecordLosses).Name("record_league_record_losses").Optional();
        Map(m => m.RecordLeagueRecordTies).Name("record_league_record_ties").Optional();
        Map(m => m.RecordLeagueRecordPct).Name("record_league_record_pct").Optional();
        Map(m => m.RecordDivisionLeader).Name("record_division_leader").Optional();
        Map(m => m.RecordWins).Name("record_wins").Optional();
        Map(m => m.RecordLosses).Name("record_losses").Optional();
        Map(m => m.RecordWinningPercentage).Name("record_winning_percentage").Optional();
        Map(m => m.FranchiseName).Name("franchise_name");
        Map(m => m.ClubName).Name("club_name");
        Map(m => m.Active).Name("active").Convert(row =>
        {
            var value = row.Row.GetField("active")?.ToLower();
            return value == "true" || value == "1" || value == "yes" || value == "y";
        });
    }
}
