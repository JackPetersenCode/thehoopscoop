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
    public class MLBPlayByPlayFileHandler
    {
        private readonly MLBPlayByPlayDatabaseHandler _dbHandler;

        public MLBPlayByPlayFileHandler(MLBPlayByPlayDatabaseHandler dbHandler)
        {
            _dbHandler = dbHandler;
        }
        public async Task<IActionResult> ReadCSVPlaysBySeason(string season)
        {
            try
            {
                string path = $"../mlb_stats/plays_{season.Replace(" ", "").Trim()}.csv";

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
                csv.Context.RegisterClassMap<PlayMap>();
                var records = csv.GetRecords<Play>().Take(50).ToList();
                return new OkObjectResult(records);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError); // Return HTTP 500 Internal Server Error
            }
        }

        public async Task<IActionResult> ReadCSVPlayEventsBySeason(string season)
        {
            try
            {
                string path = $"../mlb_stats/play_events_{season.Replace(" ", "").Trim()}.csv";

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
                csv.Context.RegisterClassMap<PlayPlayEventsMap>();
                var records = csv.GetRecords<PlayPlayEvents>().ToList();
                await _dbHandler.InsertPlayEventAsync(records, season);
                return new OkObjectResult(new { inserted = records.Count });
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError); // Return HTTP 500 Internal Server Error
            }
        }

        public async Task<IActionResult> ReadCSVRunnersBySeason(string season)
        {
            try
            {
                string path = $"../mlb_stats/runners_{season.Replace(" ", "").Trim()}.csv";

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
                csv.Context.RegisterClassMap<PlayRunnersMap>();
                var records = csv.GetRecords<PlayRunners>().ToList();
                //return new OkObjectResult(records);


                await _dbHandler.InsertPlayRunnersAsync(records, season);
                return new OkObjectResult(new { inserted = records.Count });
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError); // Return HTTP 500 Internal Server Error
            }
        }

        public async Task<IActionResult> ReadCSVRunnersCreditsBySeason(string season)
        {
            try
            {
                string path = $"../mlb_stats/credits_{season.Replace(" ", "").Trim()}.csv";

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
                csv.Context.RegisterClassMap<PlayRunnersCreditsMap>();
                var records = csv.GetRecords<PlayRunnersCredits>().ToList();
                //return new OkObjectResult(records);


                await _dbHandler.InsertPlayRunnersCreditsAsync(records, season);
                return new OkObjectResult(new { inserted = records.Count });
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError); // Return HTTP 500 Internal Server Error
            }
        }
    }
}