using CsvHelper.Configuration;
using ReactApp4.Server.Data;

public class SportRadarMLBEGSGameInfoMap : ClassMap<SportRadarMLBEGSGameInfo>
{
    public SportRadarMLBEGSGameInfoMap()
    {
        Map(m => m.GameId).Name("game_id");
        Map(m => m.Status).Name("status");
        Map(m => m.Coverage).Name("coverage");
        Map(m => m.GameNumber).Name("game_number");
        Map(m => m.DayNight).Name("day_night");
        Map(m => m.Scheduled).Name("scheduled");
        Map(m => m.HomeTeamId).Name("home_team_id");
        Map(m => m.AwayTeamId).Name("away_team_id");
        Map(m => m.Attendance).Name("attendance");
        Map(m => m.Duration).Name("duration");
        Map(m => m.SeasonId).Name("season_id");
        Map(m => m.SeasonType).Name("season_type");
        Map(m => m.SeasonYear).Name("season_year");
        Map(m => m.DoubleHeader).Name("double_header");
        Map(m => m.EntryMode).Name("entry_mode");
        Map(m => m.Reference).Name("reference");
        Map(m => m.TimeZonesVenue).Name("time_zones_venue");
        Map(m => m.TimeZonesHome).Name("time_zones_home");
        Map(m => m.TimeZonesAway).Name("time_zones_away");
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
    }
}
