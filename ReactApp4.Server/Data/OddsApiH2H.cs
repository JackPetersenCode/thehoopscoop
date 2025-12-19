using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReactApp4.Server.Data
{
    public class OddsApiH2H
    {
        public string GameId { get; set; } = string.Empty;
        public string SportKey { get; set; } = string.Empty;
        public string SportTitle { get; set; } = string.Empty;
        public DateTime CommenceTime { get; set; }
        public string HomeTeam { get; set; } = string.Empty;
        public string AwayTeam { get; set; } = string.Empty;
        public string BookmakerKey { get; set; } = string.Empty;
        public string BookmakerTitle { get; set; } = string.Empty;
        public DateTime BookmakerLastUpdate { get; set; }
        public string MarketKey { get; set; } = string.Empty;
        public DateTime MarketLastUpdate { get; set; }
        public string OutcomeName { get; set; } = string.Empty;
        public decimal OutcomePrice { get; set; }
    }
}