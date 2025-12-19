using CsvHelper.Configuration;
using ReactApp4.Server.Data;

public class SportRadarMLBLeagueScheduleMap : ClassMap<SportRadarMLBLeagueSchedule>
{
    public SportRadarMLBLeagueScheduleMap()
    {
        Map(m => m.LeagueAlias).Name("league_alias");
        Map(m => m.LeagueName).Name("league_name");
        Map(m => m.LeagueId).Name("league_id");
        Map(m => m.SeasonId).Name("season_id");
        Map(m => m.SeasonYear).Name("season_year");
        Map(m => m.SeasonType).Name("season_type");
        Map(m => m.GameId).Name("game_id");
        Map(m => m.GameStatus).Name("game_status");
        Map(m => m.GameCoverage).Name("game_coverage");
        Map(m => m.GameGameNumber).Name("game_game_number");
        Map(m => m.GameDayNight).Name("game_day_night");
        Map(m => m.GameScheduled).Name("game_scheduled");
        Map(m => m.GameHomeTeam).Name("game_home_team");
        Map(m => m.GameAwayTeam).Name("game_away_team");
        Map(m => m.GameAttendance).Name("game_attendance");
        Map(m => m.GameDuration).Name("game_duration");           // "2:10" -> TimeSpan
        Map(m => m.GameDoubleHeader).Name("game_double_header");
        Map(m => m.GameEntryMode).Name("game_entry_mode");
        Map(m => m.GameReference).Name("game_reference");

        Map(m => m.VenueName).Name("venue_name");
        Map(m => m.VenueMarket).Name("venue_market");
        Map(m => m.VenueCapacity).Name("venue_capacity");
        Map(m => m.VenueSurface).Name("venue_surface");
        Map(m => m.VenueAddress).Name("venue_address");
        Map(m => m.VenueCity).Name("venue_city");
        Map(m => m.VenueState).Name("venue_state");
        Map(m => m.VenueZip).Name("venue_zip");
        Map(m => m.VenueCountry).Name("venue_country");
        Map(m => m.VenueId).Name("venue_id");
        Map(m => m.VenueFieldOrientation).Name("venue_field_orientation");
        Map(m => m.VenueStadiumType).Name("venue_stadium_type");
        Map(m => m.VenueTimeZone).Name("venue_time_zone");
        Map(m => m.VenueLocationLat).Name("venue_location_lat");
        Map(m => m.VenueLocationLng).Name("venue_location_lng");

        Map(m => m.HomeName).Name("home_name");
        Map(m => m.HomeMarket).Name("home_market");
        Map(m => m.HomeAbbr).Name("home_abbr");
        Map(m => m.HomeId).Name("home_id");
        Map(m => m.HomeWin).Name("home_win");
        Map(m => m.HomeLoss).Name("home_loss");

        Map(m => m.AwayName).Name("away_name");
        Map(m => m.AwayMarket).Name("away_market");
        Map(m => m.AwayAbbr).Name("away_abbr");
        Map(m => m.AwayId).Name("away_id");
        Map(m => m.AwayWin).Name("away_win");
        Map(m => m.AwayLoss).Name("away_loss");

        Map(m => m.Broadcast1Network).Name("broadcast_1_network");
        Map(m => m.Broadcast1Type).Name("broadcast_1_type");
        Map(m => m.Broadcast1Locale).Name("broadcast_1_locale");
        Map(m => m.Broadcast1Channel).Name("broadcast_1_channel");
        Map(m => m.Broadcast2Network).Name("broadcast_2_network");
        Map(m => m.Broadcast2Type).Name("broadcast_2_type");
        Map(m => m.Broadcast2Locale).Name("broadcast_2_locale");
        Map(m => m.Broadcast2Channel).Name("broadcast_2_channel");
        Map(m => m.Broadcast3Network).Name("broadcast_3_network");
        Map(m => m.Broadcast3Type).Name("broadcast_3_type");
        Map(m => m.Broadcast3Locale).Name("broadcast_3_locale");
        Map(m => m.Broadcast3Channel).Name("broadcast_3_channel");

        Map(m => m.GameRescheduled).Name("game_rescheduled");
        Map(m => m.GameParentId).Name("game_parent_id");
    }
}
