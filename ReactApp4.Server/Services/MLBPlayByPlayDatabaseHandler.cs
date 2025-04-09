using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Npgsql;
using NpgsqlTypes;
using ReactApp4.Server.Data;
using Microsoft.AspNetCore.Http;
using System;
using static System.Net.WebRequestMethods;
using System.Runtime.Intrinsics.X86;
using System.Diagnostics;
using System.Collections.Generic;
using System.Numerics;
using Newtonsoft.Json.Linq;

namespace ReactApp4.Server.Services
{
    public class MLBPlayByPlayDatabaseHandler : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public MLBPlayByPlayDatabaseHandler(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        //public async Task<ActionResult<IEnumerable<MLBPlayerGameBatting>>> GetMLBPlayerGamesBattingBySeason(string season)
        //{
        //    var tableName = $"player_game_stats_batting_{season}";
//
        //    var query = $"SELECT * FROM {tableName} LIMIT 1";
//
        //    var mlbPlayerGamesBattingBySeason = await _context.MLBPlayerGamesBatting.FromSqlRaw(query).ToListAsync();
//
        //    return mlbPlayerGamesBattingBySeason;
        //}
    }
}