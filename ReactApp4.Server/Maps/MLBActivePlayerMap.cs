using CsvHelper.Configuration;
using ReactApp4.Server.Data;
using System;

public sealed class MLBActivePlayerMap : ClassMap<MLBActivePlayer>
{
    public MLBActivePlayerMap()
    {
        Map(m => m.PlayerId).Name("playerId").Convert(row =>
        {
            var raw = row.Row.GetField("playerId");
            return int.TryParse(raw, out var result) ? result : (int?)null;
        });
        Map(m => m.FullName).Name("fullName");
        Map(m => m.FirstName).Name("firstName");
        Map(m => m.LastName).Name("lastName");
        Map(m => m.PrimaryNumber).Name("primaryNumber");

        Map(m => m.BirthDate).Name("birthDate").Convert(row =>
        {
            var raw = row.Row.GetField("birthDate");
            return DateTime.TryParse(raw, out var result) ? result : (DateTime?)null;
        });

        Map(m => m.CurrentAge).Name("currentAge").Convert(row =>
        {
            var raw = row.Row.GetField("currentAge");
            return int.TryParse(raw, out var result) ? result : (int?)null;
        });

        Map(m => m.BirthCity).Name("birthCity");
        Map(m => m.BirthStateProvince).Name("birthStateProvince");
        Map(m => m.BirthCountry).Name("birthCountry");
        Map(m => m.Height).Name("height");

        Map(m => m.Weight).Name("weight").Convert(row =>
        {
            var raw = row.Row.GetField("weight");
            return int.TryParse(raw, out var result) ? result : (int?)null;
        });

        Map(m => m.Active).Name("active").Convert(row =>
        {
            var raw = row.Row.GetField("active")?.ToLower();
            return raw == "true" || raw == "1" || raw == "t";
        });

        Map(m => m.MlbDebutDate).Name("mlbDebutDate").Convert(row =>
        {
            var raw = row.Row.GetField("mlbDebutDate");
            return DateTime.TryParse(raw, out var result) ? result : (DateTime?)null;
        });

        Map(m => m.DraftYear).Name("draftYear").Convert(row =>
        {
            var raw = row.Row.GetField("draftYear");
            return int.TryParse(raw, out var result) ? result : (int?)null;
        });

        Map(m => m.TeamId).Name("teamId").Convert(row =>
        {
            var raw = row.Row.GetField("teamId");
            return int.TryParse(raw, out var result) ? result : (int?)null;
        });

        Map(m => m.TeamName).Name("teamName");
        Map(m => m.TeamLink).Name("teamLink");
        Map(m => m.PrimaryPositionCode).Name("primaryPositionCode");
        Map(m => m.PrimaryPositionName).Name("primaryPositionName");
        Map(m => m.PositionType).Name("positionType");
        Map(m => m.BatSideCode).Name("batSideCode");
        Map(m => m.BatSideDescription).Name("batSideDescription");
        Map(m => m.PitchHandCode).Name("pitchHandCode");
        Map(m => m.PitchHandDescription).Name("pitchHandDescription");
        Map(m => m.BoxscoreName).Name("boxscoreName");
        Map(m => m.NickName).Name("nickName");

        Map(m => m.StrikeZoneTop).Name("strikeZoneTop").Convert(row =>
        {
            var raw = row.Row.GetField("strikeZoneTop");
            return double.TryParse(raw, out var result) ? result : (double?)null;
        });

        Map(m => m.StrikeZoneBottom).Name("strikeZoneBottom").Convert(row =>
        {
            var raw = row.Row.GetField("strikeZoneBottom");
            return double.TryParse(raw, out var result) ? result : (double?)null;
        });

        Map(m => m.NameSlug).Name("nameSlug");
    }
}
