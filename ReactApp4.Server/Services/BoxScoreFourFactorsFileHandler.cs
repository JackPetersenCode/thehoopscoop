using CsvHelper.Configuration;
using CsvHelper;
using Microsoft.AspNetCore.Mvc;

namespace ReactApp4.Server.Services
{
    public class BoxScoreFourFactorsFileHandler
    {
        public async Task<IActionResult> GetBoxScoreFourFactorsFromFile(string season)
        {
            Console.WriteLine(season);
            List<object> data = new List<object>();

            try
            {
                using (var reader = new StreamReader($"../juicystats/boxscorefourfactors{season}.csv"))
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
