using System;

namespace ReactApp4.Server.Data
{
    public class SportRadarMLBLeagueSchedule
    {
        public string? LeagueAlias { get; set; }
        public string? LeagueName { get; set; }
        public Guid? LeagueId { get; set; }
        public Guid? SeasonId { get; set; }
        public int? SeasonYear { get; set; }
        public string? SeasonType { get; set; }
        public Guid? GameId { get; set; }
        public string? GameStatus { get; set; }
        public string? GameCoverage { get; set; }
        public int? GameGameNumber { get; set; }
        public string? GameDayNight { get; set; }
        public DateTimeOffset? GameScheduled { get; set; }
        public Guid? GameHomeTeam { get; set; }
        public Guid? GameAwayTeam { get; set; }
        public double? GameAttendance { get; set; }
        public TimeSpan? GameDuration { get; set; }
        public bool? GameDoubleHeader { get; set; }
        public string? GameEntryMode { get; set; }
        public string? GameReference { get; set; }

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
        public double? VenueLocationLat { get; set; }
        public double? VenueLocationLng { get; set; }

        public string? HomeName { get; set; }
        public string? HomeMarket { get; set; }
        public string? HomeAbbr { get; set; }
        public Guid? HomeId { get; set; }
        public int? HomeWin { get; set; }
        public int? HomeLoss { get; set; }

        public string? AwayName { get; set; }
        public string? AwayMarket { get; set; }
        public string? AwayAbbr { get; set; }
        public Guid? AwayId { get; set; }
        public int? AwayWin { get; set; }
        public int? AwayLoss { get; set; }

        public string? Broadcast1Network { get; set; }
        public string? Broadcast1Type { get; set; }
        public string? Broadcast1Locale { get; set; }
        public string? Broadcast1Channel { get; set; }
        public string? Broadcast2Network { get; set; }
        public string? Broadcast2Type { get; set; }
        public string? Broadcast2Locale { get; set; }
        public string? Broadcast2Channel { get; set; }
        public string? Broadcast3Network { get; set; }
        public string? Broadcast3Type { get; set; }
        public string? Broadcast3Locale { get; set; }
        public string? Broadcast3Channel { get; set; }
        public string? GameRescheduled { get; set; }
        public Guid? GameParentId { get; set; }
    }
}
