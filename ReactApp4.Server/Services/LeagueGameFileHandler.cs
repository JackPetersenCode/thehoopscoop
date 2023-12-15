using Microsoft.AspNetCore.Mvc;

namespace ReactApp4.Server.Services
{
    public class LeagueGameFileHandler
    {
        public async Task<IActionResult> GetGamesFromFile(string season)
        {
            try
            {
                string filePath = $"../juicystats/leaguegames{season}.json"; // Adjust the path as needed
                if (!System.IO.File.Exists(filePath))
                {
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
