using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CsvHelper;
using System.Formats.Asn1;
using CsvHelper.Configuration;




namespace ReactApp4.Server.Services
{
    public class MLBGameFileHandler
    {
        public async Task<IActionResult> GetMLBGamesFromFile(string season)
        {
            List<object> data = new List<object>();
            HashSet<string> seenGamePks = new HashSet<string>();
        
            try
            {
                using (var reader = new StreamReader($"../mlb_stats/mlb_games_{season}.csv"))
                using (var csv = new CsvReader(reader, new CsvConfiguration(System.Globalization.CultureInfo.InvariantCulture)
                {
                    Delimiter = ",",
                    HasHeaderRecord = true,
                    TrimOptions = TrimOptions.Trim
                }))
                {
                    var records = csv.GetRecords<dynamic>();
                    foreach (var record in records)
                    {
                        var dict = (IDictionary<string, object>)record;
                        string gamePk = dict["game_pk"]?.ToString() ?? "";
        
                        if (!string.IsNullOrEmpty(gamePk) && !seenGamePks.Contains(gamePk))
                        {
                            seenGamePks.Add(gamePk);
                            data.Add(record);
                        }
                    }
                }
        
                Console.WriteLine("Parsed CSV data (unique game_pks):");
                return new OkObjectResult(data);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

    }
}
