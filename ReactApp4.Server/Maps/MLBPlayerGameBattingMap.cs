using CsvHelper.Configuration;
using CsvHelper;
using CsvHelper.Configuration.Attributes;
using ReactApp4.Server.Data;

public sealed class MLBPlayerGameBattingMap : ClassMap<MLBPlayerGameBatting>
{
    public MLBPlayerGameBattingMap()
    {
        Map(m => m.GamePk).Name("gamePk");
        Map(m => m.TeamSide).Name("teamSide");
        Map(m => m.TeamName).Name("teamName");
        Map(m => m.TeamId).Name("teamId");
        Map(m => m.PlayerId).Name("playerId");
        Map(m => m.PersonId).Name("personId");
        Map(m => m.Summary).Name("summary");
        Map(m => m.GamesPlayed).Convert(args => ParseNullableDouble(args.Row, "gamesPlayed"));
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
        Map(m => m.GroundIntoDoublePlay).Convert(args => ParseNullableDouble(args.Row, "groundIntoDoublePlay"));
        Map(m => m.GroundIntoTriplePlay).Convert(args => ParseNullableDouble(args.Row, "groundIntoTriplePlay"));
        Map(m => m.PlateAppearances).Convert(args => ParseNullableDouble(args.Row, "plateAppearances"));
        Map(m => m.TotalBases).Convert(args => ParseNullableDouble(args.Row, "totalBases"));
        Map(m => m.Rbi).Convert(args => ParseNullableDouble(args.Row, "rbi"));
        Map(m => m.LeftOnBase).Convert(args => ParseNullableDouble(args.Row, "leftOnBase"));
        Map(m => m.SacBunts).Convert(args => ParseNullableDouble(args.Row, "sacBunts"));
        Map(m => m.SacFlies).Convert(args => ParseNullableDouble(args.Row, "sacFlies"));
        Map(m => m.CatchersInterference).Convert(args => ParseNullableDouble(args.Row, "catchersInterference"));
        Map(m => m.Pickoffs).Convert(args => ParseNullableDouble(args.Row, "pickoffs"));
        Map(m => m.AtBatsPerHomeRun).Convert(args => ParseNullableDouble(args.Row, "atBatsPerHomeRun"));
        Map(m => m.PopOuts).Convert(args => ParseNullableDouble(args.Row, "popOuts"));
        Map(m => m.LineOuts).Convert(args => ParseNullableDouble(args.Row, "lineOuts"));
        Map(m => m.Note).Name("note");
    }

    private double? ParseNullableDouble(IReaderRow row, string columnName)
    {
        var value = row.GetField(columnName);
        return double.TryParse(value, out var result) ? result : (double?)null;
    }
}
