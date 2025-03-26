using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CsvHelper;
using System.Formats.Asn1;
using CsvHelper.Configuration;
using System.Globalization;
using ReactApp4.Server.Data;




namespace ReactApp4.Server.Services
{
    public class MLBActivePlayerFileHandler
    {
        public async Task<IActionResult> GetMLBActivePlayerFromFile(string season)
        {
            List<object> data = new List<object>();

            try
            {
                using (var reader = new StreamReader(
                    $"../mlb_stats/active_mlb_players_{season.Replace(" ", "").Trim()}.csv"))
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
                        if (record.id != "id")
                        {
                            data.Add(record);
                        }
                    }
                }

                // Log the result array
                System.Console.WriteLine("Parsed CSV data:");
                return new OkObjectResult(data); // Return HTTP 200 OK with data
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError); // Return HTTP 500 Internal Server Error
            }
        }
    }
}
