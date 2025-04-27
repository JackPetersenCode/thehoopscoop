using CsvHelper.Configuration;
using ReactApp4.Server.Data;

public class PlayRunnersMap : ClassMap<PlayRunners>
{
    public PlayRunnersMap()
    {
        Map(m => m.GamePk).Name("game_pk");
        Map(m => m.AtBatIndex).Name("at_bat_index");

        Map(m => m.RunnersMovementOriginBase).Name("runners_movement_origin_base");
        Map(m => m.RunnersMovementStart).Name("runners_movement_start");
        Map(m => m.RunnersMovementEnd).Name("runners_movement_end");
        Map(m => m.RunnersMovementOutBase).Name("runners_movement_out_base");
        Map(m => m.RunnersMovementIsOut).Name("runners_movement_is_out");
        Map(m => m.RunnersMovementOutNumber).Name("runners_movement_out_number");

        Map(m => m.RunnersDetailsEvent).Name("runners_details_event");
        Map(m => m.RunnersDetailsEventType).Name("runners_details_event_type");
        Map(m => m.RunnersDetailsMovementReason).Name("runners_details_movement_reason");
        Map(m => m.RunnersDetailsPlayerId).Name("runners_details_player_id");
        Map(m => m.RunnersDetailsPlayerFullName).Name("runners_details_player_full_name");
        Map(m => m.RunnersDetailsResponsiblePitcherId).Name("runners_details_responsible_pitcher_id");
        Map(m => m.RunnersDetailsIsScoringEvent).Name("runners_details_is_scoring_event");
        Map(m => m.RunnersDetailsRbi).Name("runners_details_rbi");
        Map(m => m.RunnersDetailsEarned).Name("runners_details_earned");
        Map(m => m.RunnersDetailsTeamUnearned).Name("runners_details_team_unearned");
        Map(m => m.RunnersDetailsPlayIndex).Name("runners_details_play_index");
        Map(m => m.RunnersCredits).Name("runners_credits");

    }
}
