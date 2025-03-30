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
    public class MLBPlayerGameFileHandler
    {
        public async Task<IActionResult> GetMLBPlayerGamesFromFile(string season, string category)
        {
            List<object> data = new List<object>();

            try
            {
                using (var reader = new StreamReader(
                    $"../mlb_stats/player_game_stats_{category.Replace(" ", "").Trim()}_{season.Replace(" ", "").Trim()}.csv"))
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
                        //if (record.game_pk != "game_pk")
                        //{
                        data.Add(record);
                        
                        //}
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


        public async Task<IActionResult> ReadCSVPlayerGameInfo(string season)
        {
            try
            {
                string path = $"../mlb_stats/player_stats_{season.Replace(" ", "").Trim()}.csv";

                using var reader = new StreamReader(path);
                using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    Delimiter = ",",
                    HasHeaderRecord = true,
                    TrimOptions = TrimOptions.Trim,
                    MissingFieldFound = null,         // ⬅️ Ignore missing fields
                    HeaderValidated = null            // ⬅️ Disable strict header matching
                });
                // ✅ Register the map here
                csv.Context.RegisterClassMap<MLBPlayerGameInfoMap>();
                var records = csv.GetRecords<MLBPlayerGameInfo>().ToList();
                return new OkObjectResult(records);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError); // Return HTTP 500 Internal Server Error
            }
        }

        public async Task<IActionResult> ReadCSVPlayerGameStats(string season, string category)
        {
            try
            {
                string path = $"../mlb_stats/player_game_stats_{category.Replace(" ", "").Trim()}_{season.Replace(" ", "").Trim()}.csv";

                using var reader = new StreamReader(path);
                using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    Delimiter = ",",
                    HasHeaderRecord = true,
                    TrimOptions = TrimOptions.Trim,
                    MissingFieldFound = null,         // ⬅️ Ignore missing fields
                    HeaderValidated = null            // ⬅️ Disable strict header matching
                });

                if (category.ToLower() == "batting")
                {
                    // ✅ Register the map here
                    csv.Context.RegisterClassMap<MLBPlayerGameBattingMap>();

                    var records = csv.GetRecords<MLBPlayerGameBatting>().ToList();
                    return new OkObjectResult(records);
                }
                else if (category.ToLower() == "pitching")
                {
                    csv.Context.RegisterClassMap<MLBPlayerGamePitchingMap>();
                    var records = csv.GetRecords<MLBPlayerGamePitching>().ToList();
                    return new OkObjectResult(records);
                }
                else if (category.ToLower() == "fielding")
                {
                    csv.Context.RegisterClassMap<MLBPlayerGameFieldingMap>();
                    var records = csv.GetRecords<MLBPlayerGameFielding>().ToList();
                    return new OkObjectResult(records);
                }
                else
                {
                    return new BadRequestObjectResult("Invalid category.");
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError); // Return HTTP 500 Internal Server Error
            }
        }

    }
}
