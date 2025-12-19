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
    public class SportRadarMLBEGSFileHandler
    {
        public async Task<IActionResult> SportRadarMLBEGSReadCSV(string fileName)
        {
            List<object> data = new List<object>();

            try
            {
                string path = $"../mlb_stats/{fileName}.csv";

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
                if (fileName.Contains("sportradar_egs_game_info"))
                {
                    csv.Context.RegisterClassMap<SportRadarMLBEGSGameInfoMap>();
                    var records = csv.GetRecords<SportRadarMLBEGSGameInfo>().ToList();
                    return new OkObjectResult(records);
                }
                else if (fileName.Contains("sportradar_league_schedule"))
                {
                    csv.Context.RegisterClassMap<SportRadarMLBLeagueScheduleMap>();
                    var records = csv.GetRecords<SportRadarMLBLeagueSchedule>().ToList();
                    return new OkObjectResult(records);
                }
                else if (fileName.Contains("sportradar_pbp_at_bats"))
                {
                    csv.Context.RegisterClassMap<SportRadarMLBPBPAtBatMap>();
                    var records = csv.GetRecords<SportRadarMLBPBPAtBat>().ToList();
                    return new OkObjectResult(records);
                }
                else if (fileName.Contains("sportradar_pbp_pitches"))
                {
                    csv.Context.RegisterClassMap<SportRadarMLBPBPPitchEventMap>();
                    var records = csv.GetRecords<SportRadarMLBPBPPitchEvent>().ToList();
                    return new OkObjectResult(records);
                }
                else
                {
                    csv.Context.RegisterClassMap<OddsApiPlayerPropMap>();
                    var records = csv.GetRecords<OddsApiPlayerProp>().ToList();
                    return new OkObjectResult(records);
                }
                

                //await _dbHandler.InsertPlayRunnersCreditsAsync(records, season);
                //return new OkObjectResult(new { inserted = records.Count });
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError); // Return HTTP 500 Internal Server Error
            }
        }
    }
}