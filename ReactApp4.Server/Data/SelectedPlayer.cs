using System.ComponentModel.DataAnnotations.Schema;

namespace ReactApp4.Server.Data
{
    public class SelectedPlayer
    {
        [Column("player_id")]
        public string? Player_id { get; set; }

        [Column("player_name")]
        public string? Player_name { get; set; }

        [Column("team_id")]
        public string? Team_id { get; set; }

        [Column("team_abbreviation")]
        public string? Team_abbreviation { get; set; }
    }
}
