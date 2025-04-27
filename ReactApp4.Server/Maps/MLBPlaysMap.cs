using CsvHelper.Configuration;
using ReactApp4.Server.Data;
using CsvHelper;
public sealed class PlayMap : ClassMap<Play>
{
    public PlayMap()
    {
        Map(m => m.GamePk).Name("game_pk");
        Map(m => m.ResultType).Name("result_type");
        Map(m => m.ResultEvent).Name("result_event");
        Map(m => m.ResultEventType).Name("result_event_type");
        Map(m => m.ResultDescription).Name("result_description");
        Map(m => m.ResultRbi).Name("result_rbi");
        Map(m => m.ResultAwayScore).Name("result_away_score");
        Map(m => m.ResultHomeScore).Name("result_home_score");
        Map(m => m.ResultIsOut).Name("result_is_out");

        Map(m => m.AboutAtBatIndex).Name("about_at_bat_index");
        Map(m => m.AboutHalfInning).Name("about_half_inning");
        Map(m => m.AboutIsTopInning).Name("about_is_top_inning");
        Map(m => m.AboutInning).Name("about_inning");
        Map(m => m.AboutStartTime).Name("about_start_time");
        Map(m => m.AboutEndTime).Name("about_end_time");
        Map(m => m.AboutIsComplete).Name("about_is_complete");
        Map(m => m.AboutIsScoringPlay).Name("about_is_scoring_play");
        Map(m => m.AboutHasReview).Name("about_has_review");
        Map(m => m.AboutHasOut).Name("about_has_out");
        Map(m => m.AboutCaptivatingIndex).Name("about_captivating_index");

        Map(m => m.CountBalls).Name("count_balls");
        Map(m => m.CountStrikes).Name("count_strikes");
        Map(m => m.CountOuts).Name("count_outs");

        Map(m => m.MatchupBatterId).Name("matchup_batter_id");
        Map(m => m.MatchupBatterFullName).Name("matchup_batter_full_name");
        Map(m => m.MatchupBatSideCode).Name("matchup_bat_side_code");
        Map(m => m.MatchupBatSideDescription).Name("matchup_bat_side_description");
        Map(m => m.MatchupPitcherId).Name("matchup_pitcher_id");
        Map(m => m.MatchupPitcherFullName).Name("matchup_pitcher_full_name");
        Map(m => m.MatchupPitchHandCode).Name("matchup_pitch_hand_code");
        Map(m => m.MatchupPitchHandDescription).Name("matchup_pitch_hand_description");
        Map(m => m.MatchupSplitsBatter).Name("matchup_splits_batter");
        Map(m => m.MatchupSplitsPitcher).Name("matchup_splits_pitcher");
        Map(m => m.MatchupSplitsMenOnBase).Name("matchup_splits_men_on_base");

        Map(m => m.MatchupPostOnFirstId).Name("matchup_post_on_first_id");
        Map(m => m.MatchupPostOnFirstFullName).Name("matchup_post_on_first_full_name");

        Map(m => m.MatchupBatterHotColdZones).Name("matchup_batter_hot_cold_zones");
        Map(m => m.MatchupPitcherHotColdZones).Name("matchup_pitcher_hot_cold_zones");

        Map(m => m.PitchIndex).Name("pitch_index");
        Map(m => m.ActionIndex).Name("action_index");
        Map(m => m.RunnerIndex).Name("runner_index");

        Map(m => m.PlayEndTime).Name("play_end_time");
        Map(m => m.AtBatIndex).Name("play_at_bat_index");
    }
}
