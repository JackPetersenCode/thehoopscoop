using System.ComponentModel.DataAnnotations.Schema;
namespace ReactApp4.Server.Data
{
    public class SportRadarMLBEGSGameInfo
    {
        public Guid GameId { get; set; }
        public string? Status { get; set; }
        public string? Coverage { get; set; }
        public int? GameNumber { get; set; }
        public string? DayNight { get; set; }
        public DateTimeOffset? Scheduled { get; set; }
        public Guid? HomeTeamId { get; set; }
        public Guid? AwayTeamId { get; set; }
        public int? Attendance { get; set; }
        public string? Duration { get; set; }
        public Guid? SeasonId { get; set; }
        public string? SeasonType { get; set; }
        public int? SeasonYear { get; set; }
        public bool? DoubleHeader { get; set; }
        public string? EntryMode { get; set; }
        public string? Reference { get; set; }
        public string? TimeZonesVenue { get; set; }
        public string? TimeZonesHome { get; set; }
        public string? TimeZonesAway { get; set; }
        public string? VenueName { get; set; }
        public string? VenueMarket { get; set; }
        public int? VenueCapacity { get; set; }
        public string? VenueSurface { get; set; }
        public string? VenueAddress { get; set; }
        public string? VenueCity { get; set; }
        public string? VenueState { get; set; }
        public string? VenueZip { get; set; }
        public string? VenueCountry { get; set; }
        public Guid? VenueId { get; set; }
        public string? VenueFieldOrientation { get; set; }
        public string? VenueStadiumType { get; set; }
        public string? VenueTimeZone { get; set; }
        public decimal? VenueLocationLat { get; set; }
        public decimal? VenueLocationLng { get; set; }
    }
}
