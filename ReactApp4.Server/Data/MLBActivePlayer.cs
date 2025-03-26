using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReactApp4.Server.Data
{
    public class MLBActivePlayer
    {
        [Column("player_id")]
        public int? PlayerId { get; set; }

        [Column("full_name")]
        public string FullName { get; set; } = "";

        [Column("first_name")]
        public string FirstName { get; set; } = "";

        [Column("last_name")]
        public string LastName { get; set; } = "";

        [Column("primary_number")]
        public string? PrimaryNumber { get; set; }

        [Column("birth_date")]
        public DateTime? BirthDate { get; set; }

        [Column("current_age")]
        public int? CurrentAge { get; set; }

        [Column("birth_city")]
        public string? BirthCity { get; set; }

        [Column("birth_state_province")]
        public string? BirthStateProvince { get; set; }

        [Column("birth_country")]
        public string? BirthCountry { get; set; }

        [Column("height")]
        public string? Height { get; set; }

        [Column("weight")]
        public int? Weight { get; set; }

        [Column("active")]
        public bool Active { get; set; }

        [Column("mlb_debut_date")]
        public DateTime? MlbDebutDate { get; set; }

        [Column("draft_year")]
        public int? DraftYear { get; set; }

        [Column("team_id")]
        public int? TeamId { get; set; }

        [Column("team_name")]
        public string? TeamName { get; set; }

        [Column("team_link")]
        public string? TeamLink { get; set; }

        [Column("primary_position_code")]
        public string? PrimaryPositionCode { get; set; }

        [Column("primary_position_name")]
        public string? PrimaryPositionName { get; set; }

        [Column("position_type")]
        public string? PositionType { get; set; }

        [Column("bat_side_code")]
        public string? BatSideCode { get; set; }

        [Column("bat_side_description")]
        public string? BatSideDescription { get; set; }

        [Column("pitch_hand_code")]
        public string? PitchHandCode { get; set; }

        [Column("pitch_hand_description")]
        public string? PitchHandDescription { get; set; }

        [Column("boxscore_name")]
        public string? BoxscoreName { get; set; }

        [Column("nick_name")]
        public string? NickName { get; set; }

        [Column("strike_zone_top")]
        public double? StrikeZoneTop { get; set; }

        [Column("strike_zone_bottom")]
        public double? StrikeZoneBottom { get; set; }

        [Column("name_slug")]
        public string? NameSlug { get; set; }
    }
}
