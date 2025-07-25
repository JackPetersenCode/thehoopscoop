﻿using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CsvHelper;
using System.Formats.Asn1;
using CsvHelper.Configuration;




namespace ReactApp4.Server.Services
{
    public class BoxScoresFileHandler
    {
        public async Task<IActionResult> GetBoxScoresFromFile(string season, string boxType, string numPlayers)
        {
            List<object> data = new List<object>();

            try
            {
                using (var reader = new StreamReader($"../juicystats/box_score_summary_{season}.csv"))
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
                        if (record.GAME_ID != "GAME_ID")
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
