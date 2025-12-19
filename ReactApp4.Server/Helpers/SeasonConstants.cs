// File: Helpers/SeasonConstants.cs
using System.Collections.Generic;

namespace ReactApp4.Server.Helpers
{
	public static class SeasonConstants
	{
		public static readonly HashSet<string> NBAAllowedSeasons = new()
		{
			"0",
			"1",
			"2015_16",
			"2016_17",
			"2017_18",
			"2018_19",
			"2019_20",
			"2020_21",
			"2021_22",
			"2022_23",
			"2023_24",
			"2024_25",
			"2025_26"
			// Add future seasons here
		};

		public static readonly HashSet<string> MLBAllowedSeasons = new()
		{
			"0",
			"1",
			"2015",
			"2016",
			"2017",
			"2018",
			"2019",
			"2020",
			"2021",
			"2022",
			"2023",
			"2024",
			"2025"
			// Add future seasons here
		};

		public static readonly HashSet<string> AllowedSports = new()
		{
			"NBA",
			"MLB"
			// Add future sports here
		};

		public static readonly HashSet<string> MLBAllowedSeasonsNoDefaults = new()
		{
			"2015",
			"2016",
			"2017",
			"2018",
			"2019",
			"2020",
			"2021",
			"2022",
			"2023",
			"2024",
			"2025"
			// Add future seasons here
		};

		// Whitelist box types
		public static readonly HashSet<string> AllowedBoxTypes = new()
		{
			"traditional",
			"advanced",
			"summary",
			"fourfactors",
			"misc",
			"scoring"
		};

		public static readonly HashSet<string> AllowedSortFields = new()
		{
			"player_id",
			"team_id",
			"full_name",
			"runs",
			"hits",
			"rbi",
			"at_bats",
			"innings_piched",
			"games_played",
			"min",
			"id",
			"pts"
		}; // Adjust as needed
		public static readonly HashSet<string> AllowedH_or_V = new()
		{
			"home",
			"visitor",
			"away"
		}; // Adjust as needed


		public static bool IsValidNBASeason(string season) =>
			NBAAllowedSeasons.Contains(season);

		public static bool IsValidMLBSeason(string season) =>
			MLBAllowedSeasons.Contains(season);
		public static bool IsValidSport(string sport) =>
			AllowedSports.Contains(sport);
		public static bool IsValidSortfield(string season) =>
			AllowedSortFields.Contains(season);

		public static bool IsValidBoxType(string season) =>
			AllowedBoxTypes.Contains(season);
		public static bool IsValidH_or_V(string season) =>
			AllowedH_or_V.Contains(season);
	}
}
