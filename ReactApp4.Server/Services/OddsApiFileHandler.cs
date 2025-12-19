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
    public class OddsApiFileHandler
    {
        public async Task<IActionResult> ReadCSVFile(string sport, string season, string fileName)
        {
            Console.WriteLine("shitholesss");
            List<object> data = new List<object>();

            try
            {
                string path = $"../mlb_stats/{fileName}.csv";
                if (sport == "NBA")
                {
                    path = $"../juicystats/{fileName}.csv";
                }
                Console.WriteLine(path);

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
                if (fileName.Contains("h2h"))
                {
                    Console.WriteLine("odds file");
                    csv.Context.RegisterClassMap<OddsApiH2HMap>();
                    var records = csv.GetRecords<OddsApiH2H>().ToList();
                    return new OkObjectResult(records);
                }
                else                {
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