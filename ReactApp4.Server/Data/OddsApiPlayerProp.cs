public class OddsApiPlayerProp
{
    public string GameId { get; set; } = string.Empty;
    public DateTime CommenceTime { get; set; }
    public string HomeTeam { get; set; } = string.Empty;
    public string AwayTeam { get; set; } = string.Empty;
    public string Bookmaker { get; set; } = string.Empty;
    public string Market { get; set; } = string.Empty;
    public string Player { get; set; } = string.Empty;
    public decimal Line { get; set; }
    public string BetType { get; set; } = string.Empty;
    public decimal DecimalOdds { get; set; }
}