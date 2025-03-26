using CsvHelper.Configuration;
using CsvHelper;
using ReactApp4.Server.Data;

public sealed class MLBPlayerGameFieldingMap : ClassMap<MLBPlayerGameFielding>
{
    public MLBPlayerGameFieldingMap()
    {
        Map(m => m.GamePk).Name("gamePk");
        Map(m => m.TeamSide).Name("teamSide");
        Map(m => m.TeamName).Name("teamName");
        Map(m => m.PlayerId).Name("playerId");
        Map(m => m.PersonId).Name("personId");

        Map(m => m.CaughtStealing).Convert(args => ParseNullableDouble(args.Row, "caughtStealing"));
        Map(m => m.StolenBases).Convert(args => ParseNullableDouble(args.Row, "stolenBases"));
        Map(m => m.StolenBasePercentage).Convert(args => ParseNullableDouble(args.Row, "stolenBasePercentage"));
        Map(m => m.Assists).Convert(args => ParseNullableDouble(args.Row, "assists"));
        Map(m => m.PutOuts).Convert(args => ParseNullableDouble(args.Row, "putOuts"));
        Map(m => m.Errors).Convert(args => ParseNullableDouble(args.Row, "errors"));
        Map(m => m.Chances).Convert(args => ParseNullableDouble(args.Row, "chances"));
        Map(m => m.Fielding).Convert(args => ParseNullableDouble(args.Row, "fielding"));
        Map(m => m.PassedBall).Convert(args => ParseNullableDouble(args.Row, "passedBall"));
        Map(m => m.Pickoffs).Convert(args => ParseNullableDouble(args.Row, "pickoffs"));
        Map(m => m.GamesStarted).Convert(args => ParseNullableDouble(args.Row, "gamesStarted"));
    }

    private double? ParseNullableDouble(IReaderRow row, string columnName)
    {
        var value = row.GetField(columnName);
        return double.TryParse(value, out var result) ? result : (double?)null;
    }
}
