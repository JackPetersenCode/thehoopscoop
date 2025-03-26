using CsvHelper.Configuration;
using CsvHelper;
using ReactApp4.Server.Data;

public sealed class MLBPlayerGamePitchingMap : ClassMap<MLBPlayerGamePitching>
{
    public MLBPlayerGamePitchingMap()
    {
        Map(m => m.GamePk).Name("gamePk");
        Map(m => m.TeamSide).Name("teamSide");
        Map(m => m.TeamName).Name("teamName");
        Map(m => m.PlayerId).Name("playerId");
        Map(m => m.PersonId).Name("personId");

        Map(m => m.Note).Name("note");
        Map(m => m.Summary).Name("summary");

        Map(m => m.GamesPlayed).Convert(args => ParseNullableDouble(args.Row, "gamesPlayed"));
        Map(m => m.GamesStarted).Convert(args => ParseNullableDouble(args.Row, "gamesStarted"));
        Map(m => m.FlyOuts).Convert(args => ParseNullableDouble(args.Row, "flyOuts"));
        Map(m => m.GroundOuts).Convert(args => ParseNullableDouble(args.Row, "groundOuts"));
        Map(m => m.AirOuts).Convert(args => ParseNullableDouble(args.Row, "airOuts"));
        Map(m => m.Runs).Convert(args => ParseNullableDouble(args.Row, "runs"));
        Map(m => m.Doubles).Convert(args => ParseNullableDouble(args.Row, "doubles"));
        Map(m => m.Triples).Convert(args => ParseNullableDouble(args.Row, "triples"));
        Map(m => m.HomeRuns).Convert(args => ParseNullableDouble(args.Row, "homeRuns"));
        Map(m => m.StrikeOuts).Convert(args => ParseNullableDouble(args.Row, "strikeOuts"));
        Map(m => m.BaseOnBalls).Convert(args => ParseNullableDouble(args.Row, "baseOnBalls"));
        Map(m => m.IntentionalWalks).Convert(args => ParseNullableDouble(args.Row, "intentionalWalks"));
        Map(m => m.Hits).Convert(args => ParseNullableDouble(args.Row, "hits"));
        Map(m => m.HitByPitch).Convert(args => ParseNullableDouble(args.Row, "hitByPitch"));
        Map(m => m.AtBats).Convert(args => ParseNullableDouble(args.Row, "atBats"));
        Map(m => m.CaughtStealing).Convert(args => ParseNullableDouble(args.Row, "caughtStealing"));
        Map(m => m.StolenBases).Convert(args => ParseNullableDouble(args.Row, "stolenBases"));
        Map(m => m.StolenBasePercentage).Convert(args => ParseNullableDouble(args.Row, "stolenBasePercentage"));
        Map(m => m.NumberOfPitches).Convert(args => ParseNullableDouble(args.Row, "numberOfPitches"));

        Map(m => m.InningsPitched).Name("inningsPitched");

        Map(m => m.Wins).Convert(args => ParseNullableDouble(args.Row, "wins"));
        Map(m => m.Losses).Convert(args => ParseNullableDouble(args.Row, "losses"));
        Map(m => m.Saves).Convert(args => ParseNullableDouble(args.Row, "saves"));
        Map(m => m.SaveOpportunities).Convert(args => ParseNullableDouble(args.Row, "saveOpportunities"));
        Map(m => m.Holds).Convert(args => ParseNullableDouble(args.Row, "holds"));
        Map(m => m.BlownSaves).Convert(args => ParseNullableDouble(args.Row, "blownSaves"));
        Map(m => m.EarnedRuns).Convert(args => ParseNullableDouble(args.Row, "earnedRuns"));
        Map(m => m.BattersFaced).Convert(args => ParseNullableDouble(args.Row, "battersFaced"));
        Map(m => m.Outs).Convert(args => ParseNullableDouble(args.Row, "outs"));
        Map(m => m.GamesPitched).Convert(args => ParseNullableDouble(args.Row, "gamesPitched"));
        Map(m => m.CompleteGames).Convert(args => ParseNullableDouble(args.Row, "completeGames"));
        Map(m => m.Shutouts).Convert(args => ParseNullableDouble(args.Row, "shutouts"));
        Map(m => m.PitchesThrown).Convert(args => ParseNullableDouble(args.Row, "pitchesThrown"));
        Map(m => m.Balls).Convert(args => ParseNullableDouble(args.Row, "balls"));
        Map(m => m.Strikes).Convert(args => ParseNullableDouble(args.Row, "strikes"));
        Map(m => m.StrikePercentage).Convert(args => ParseNullableDouble(args.Row, "strikePercentage"));
        Map(m => m.HitBatsmen).Convert(args => ParseNullableDouble(args.Row, "hitBatsmen"));
        Map(m => m.Balks).Convert(args => ParseNullableDouble(args.Row, "balks"));
        Map(m => m.WildPitches).Convert(args => ParseNullableDouble(args.Row, "wildPitches"));
        Map(m => m.Pickoffs).Convert(args => ParseNullableDouble(args.Row, "pickoffs"));
        Map(m => m.Rbi).Convert(args => ParseNullableDouble(args.Row, "rbi"));
        Map(m => m.GamesFinished).Convert(args => ParseNullableDouble(args.Row, "gamesFinished"));
        Map(m => m.RunsScoredPer9).Convert(args => ParseNullableDouble(args.Row, "runsScoredPer9"));
        Map(m => m.HomeRunsPer9).Convert(args => ParseNullableDouble(args.Row, "homeRunsPer9"));
        Map(m => m.InheritedRunners).Convert(args => ParseNullableDouble(args.Row, "inheritedRunners"));
        Map(m => m.InheritedRunnersScored).Convert(args => ParseNullableDouble(args.Row, "inheritedRunnersScored"));
        Map(m => m.CatchersInterference).Convert(args => ParseNullableDouble(args.Row, "catchersInterference"));
        Map(m => m.SacBunts).Convert(args => ParseNullableDouble(args.Row, "sacBunts"));
        Map(m => m.SacFlies).Convert(args => ParseNullableDouble(args.Row, "sacFlies"));
        Map(m => m.PassedBall).Convert(args => ParseNullableDouble(args.Row, "passedBall"));
        Map(m => m.PopOuts).Convert(args => ParseNullableDouble(args.Row, "popOuts"));
        Map(m => m.LineOuts).Convert(args => ParseNullableDouble(args.Row, "lineOuts"));
    }

    private double? ParseNullableDouble(IReaderRow row, string columnName)
    {
        var value = row.GetField(columnName);
        return double.TryParse(value, out var result) ? result : (double?)null;
    }
}
