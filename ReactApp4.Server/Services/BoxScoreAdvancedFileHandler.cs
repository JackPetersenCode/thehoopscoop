using CsvHelper.Configuration;
using CsvHelper;
using Microsoft.AspNetCore.Mvc;

namespace ReactApp4.Server.Services
{
    public class BoxScoreAdvancedFileHandler
    {
        public async Task<IActionResult> GetBoxScoreAdvancedFromFile(string season)
        {
            List<object> data = new List<object>();

            try
            {
                using (var reader = new StreamReader($"../juicystats/box_score_advanced_{season}.csv"))
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
                        if (record.MIN != "MIN")
                        {
                            data.Add(record);
                        }
                    }
                }

                // Log the result array
                return new OkObjectResult(data); // Return HTTP 200 OK with data
            }
            catch (Exception ex)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError); // Return HTTP 500 Internal Server Error
            }
        }
    }
}
