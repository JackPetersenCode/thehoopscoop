using CsvHelper.Configuration;
using ReactApp4.Server.Data;

public class PlayRunnersCreditsMap : ClassMap<PlayRunnersCredits>
{
    public PlayRunnersCreditsMap()
    {
        Map(m => m.GamePk).Name("game_pk");
        Map(m => m.AtBatIndex).Name("at_bat_index");
        Map(m => m.RunnersDetailsPlayIndex).Name("runners_details_play_index");
        Map(m => m.RunnersDetailsPlayerId).Name("runners_details_player_id");
        Map(m => m.RunnersCreditsPlayerId).Name("runners_credits_player_id");
        Map(m => m.RunnersCreditsPositionCode).Name("runners_credits_position_code");
        Map(m => m.RunnersCreditsPositionName).Name("runners_credits_position_name");
        Map(m => m.RunnersCreditsPositionType).Name("runners_credits_position_type");
        Map(m => m.RunnersCreditsPositionAbbreviation).Name("runners_credits_position_abbreviation");
        Map(m => m.RunnersCreditsCredit).Name("runners_credits_credit");    
    }
}
