using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CsvHelper;
using System.Formats.Asn1;
using CsvHelper.Configuration;




namespace ReactApp4.Server.Services
{
    public class LeagueDashLineupsFileHandler
    {
        public async Task<IActionResult> GetLeagueDashLineupsFromFile(string season, string boxType, string numPlayers)
        {
            try
            {
                Console.WriteLine(season);
                Console.WriteLine(boxType);
                Console.WriteLine(numPlayers);
                string filePath = $"../juicystats/league_dash_lineups_{boxType}_{numPlayers}man_{season}.json"; // Adjust the path as needed
                Console.WriteLine(filePath);
                if (!System.IO.File.Exists(filePath))
                {
                    Console.WriteLine("couldnt find the file");
                    return new NotFoundResult(); // Handle case where file doesn't exist
                }

                string jsonContent = await System.IO.File.ReadAllTextAsync(filePath);
                return new OkObjectResult(jsonContent);
            }
            catch (Exception)
            {
                return new StatusCodeResult(500);
            }
        }
    }
}
