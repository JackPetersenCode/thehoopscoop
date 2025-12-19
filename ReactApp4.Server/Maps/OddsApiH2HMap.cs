using CsvHelper.Configuration;
using ReactApp4.Server.Data;

public class OddsApiH2HMap : ClassMap<OddsApiH2H>
{
    public OddsApiH2HMap()
    {
        Map(m => m.SportKey).Name("sport_key");
        Map(m => m.SportTitle).Name("sport_title");
        Map(m => m.GameId).Name("game_id");
        Map(m => m.CommenceTime).Name("commence_time");
        Map(m => m.HomeTeam).Name("home_team");
        Map(m => m.AwayTeam).Name("away_team");
        Map(m => m.BookmakerKey).Name("bookmaker_key");
        Map(m => m.BookmakerTitle).Name("bookmaker_title");
        Map(m => m.BookmakerLastUpdate).Name("bookmaker_last_update");
        Map(m => m.MarketKey).Name("market_key");
        Map(m => m.MarketLastUpdate).Name("market_last_update");
        Map(m => m.OutcomeName).Name("outcome_name");
        Map(m => m.OutcomePrice).Name("outcome_price");
    }
}
