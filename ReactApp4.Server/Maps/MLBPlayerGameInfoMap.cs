using CsvHelper.Configuration;
using ReactApp4.Server.Data;

public sealed class MLBPlayerGameInfoMap : ClassMap<MLBPlayerGameInfo>
{
    public MLBPlayerGameInfoMap()
    {
        Map(m => m.GamePk).Name("gamePk");
        Map(m => m.TeamSide).Name("teamSide");
        Map(m => m.TeamName).Name("teamName");
        Map(m => m.PlayerId).Name("playerId");
        Map(m => m.PersonId).Name("personId");
        Map(m => m.FullName).Name("fullName");
        Map(m => m.BoxscoreName).Name("boxscoreName");
        Map(m => m.JerseyNumber).Name("jerseyNumber");
        Map(m => m.Position).Name("position");
        Map(m => m.PositionAbbr).Name("position_abbr");
        Map(m => m.StatusCode).Name("status_code");
        Map(m => m.StatusDescription).Name("status_description");
        Map(m => m.IsCurrentBatter).Name("isCurrentBatter");
        Map(m => m.IsCurrentPitcher).Name("isCurrentPitcher");
        Map(m => m.IsOnBench).Name("isOnBench");
        Map(m => m.IsSubstitute).Name("isSubstitute");
    }
}
