using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReactApp4.Server.Data
{
    public class Player
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("full_name")]
        public string? Full_name { get; set; }

        [Column("first_name")]
        public string? First_name { get; set; }

        [Column("last_name")]
        public string? Last_name { get; set; }

        [Column("is_active")]
        public bool? Is_active { get; set; }

        [Column("player_id")]
        public string? Player_id { get; set; }
    }
}
