using System.ComponentModel.DataAnnotations.Schema;

namespace ReactApp4.Server.Data
{
    [Table("ballers")]

    public class Baller
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("name")]
        public string? Name { get; set; }

        [Column("score")]
        public int? Score { get; set; }

        [Column("salary")]
        public int? Salary { get; set; }
    }
}