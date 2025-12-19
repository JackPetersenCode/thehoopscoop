using System;
using System.Globalization;
using CsvHelper.Configuration;
using ReactApp4.Server.Data;

public class SportRadarMLBPBPAtBatMap : ClassMap<SportRadarMLBPBPAtBat>
{
    public SportRadarMLBPBPAtBatMap()
    {
        Map(m => m.GameId).Name("game_id");
        Map(m => m.Inning).Name("inning");
        Map(m => m.Half).Name("half");
        Map(m => m.HitterId).Name("hitter_id");
        Map(m => m.AtBatId).Name("at_bat_id");
        Map(m => m.HitterHand).Name("hitter_hand");
        Map(m => m.PitcherId).Name("pitcher_id");
        Map(m => m.PitcherHand).Name("pitcher_hand");
        Map(m => m.SequenceNumber).Name("sequence_number");
        Map(m => m.Description).Name("description");

        Map(m => m.HitterPreferredName).Name("hitter_preferred_name");
        Map(m => m.HitterFirstName).Name("hitter_first_name");
        Map(m => m.HitterLastName).Name("hitter_last_name");
        Map(m => m.HitterJerseyNumber).Name("hitter_jersey_number");
        Map(m => m.HitterFullName).Name("hitter_full_name");

        Map(m => m.PitcherPreferredName).Name("pitcher_preferred_name");
        Map(m => m.PitcherFirstName).Name("pitcher_first_name");
        Map(m => m.PitcherLastName).Name("pitcher_last_name");
        Map(m => m.PitcherJerseyNumber).Name("pitcher_jersey_number");
        Map(m => m.PitcherFullName).Name("pitcher_full_name");

        Map(m => m.HomeTeamRuns).Name("home_team_runs");
        Map(m => m.AwayTeamRuns).Name("away_team_runs");
    }
}
