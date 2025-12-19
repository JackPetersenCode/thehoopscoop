using CsvHelper.Configuration;
using ReactApp4.Server.Data;

public class OddsApiPlayerPropMap : ClassMap<OddsApiPlayerProp>
{
    public OddsApiPlayerPropMap()
    {
        Map(m => m.GameId).Name("game_id");
        Map(m => m.CommenceTime).Name("commence_time");
        Map(m => m.HomeTeam).Name("home_team");
        Map(m => m.AwayTeam).Name("away_team");
        Map(m => m.Bookmaker).Name("bookmaker");
        Map(m => m.Market).Name("market");
        Map(m => m.Player).Name("player");
        Map(m => m.Line).Name("line");
        Map(m => m.BetType).Name("bet_type");
        Map(m => m.DecimalOdds).Name("decimal_odds");
    }
}
