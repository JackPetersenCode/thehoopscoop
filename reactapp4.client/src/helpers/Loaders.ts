import { getJsonResponseStartup } from './GetJsonResponse';
import { postLeagueGamesBySeason, postPlayersNBA, postBoxScoresTraditionalBySeason, postLeagueDashLineups, postBoxScoresAdvancedBySeason, postBoxScoresFourFactorsBySeason, postBoxScoresMiscBySeason, postBoxScoresScoringBySeason, postShotBySeason, postNewOdds, postBoxScoreSummary, postMLBGamesBySeason } from './PostFunctions';

const loadMLBGames = async () => {
    const season = "2023"; // Adjust as needed
    //const tablelength = await getJsonResponseStartup(`/api/tablelength/mlb_games_${season}`);
    //console.log(tablelength.count);

    const data = await getJsonResponseStartup(`/api/MLBGame/read/${season}`);
    console.log(data);

    for (let i = 0; i < data.length; i++) {
        const mlbGame = {
            gamePk: data[i].game_pk, // VARCHAR(50)
            gameGuid: data[i].game_guid, // VARCHAR(50)
            link: data[i].link, // TEXT
            gameType: data[i].game_type, // CHARACTER(1)
            season: data[i].season, // VARCHAR(4)
            gameDate: data[i].game_date, // DATE (ISO String)
            officialDate: data[i].official_date, // DATE (ISO String)
            abstractGameState: data[i].abstract_game_state, // VARCHAR(20)
            codedGameState: data[i].coded_game_state, // CHARACTER(1)
            detailedState: data[i].detailed_state, // VARCHAR(50)
            statusCode: data[i].status_code, // CHARACTER(1)
            startTimeTbd: data[i].start_time_tbd === "True", // BOOLEAN
            abstractGameCode: data[i].abstract_game_code, // CHARACTER(1)
            awayTeamId: parseInt(data[i].away_team_id), // INTEGER
            awayTeamName: data[i].away_team_name, // VARCHAR(50)
            awayScore: data[i].away_score ? parseInt(data[i].away_score) : null, // INTEGER
            awayWins: parseInt(data[i].away_wins), // INTEGER
            awayLosses: parseInt(data[i].away_losses), // INTEGER
            awayWinPct: parseFloat(data[i].away_win_pct), // NUMERIC(5,3)
            awayIsWinner: data[i].away_is_winner === "True", // BOOLEAN
            homeTeamId: parseInt(data[i].home_team_id), // INTEGER
            homeTeamName: data[i].home_team_name, // VARCHAR(50)
            homeScore: data[i].home_score ? parseInt(data[i].home_score) : null, // INTEGER
            homeWins: parseInt(data[i].home_wins), // INTEGER
            homeLosses: parseInt(data[i].home_losses), // INTEGER
            homeWinPct: parseFloat(data[i].home_win_pct), // NUMERIC(5,3)
            homeIsWinner: data[i].home_is_winner === "True", // BOOLEAN
            venueId: parseInt(data[i].venue_id), // INTEGER
            venueName: data[i].venue_name, // VARCHAR(100)
            isTie: data[i].is_tie === "True", // BOOLEAN
            gameNumber: parseInt(data[i].game_number), // INTEGER
            doubleHeader: data[i].double_header, // CHARACTER(1)
            dayNight: data[i].day_night, // VARCHAR(10)
            description: data[i].description, // TEXT
            scheduledInnings: parseInt(data[i].scheduled_innings), // INTEGER
            gamesInSeries: parseInt(data[i].games_in_series), // INTEGER
            seriesGameNumber: parseInt(data[i].series_game_number), // INTEGER
            seriesDescription: data[i].series_description, // VARCHAR(50)
            ifNecessary: data[i].if_necessary, // CHARACTER(1)
            ifNecessaryDesc: data[i].if_necessary_desc // VARCHAR(50)
        };
        console.log(mlbGame)
        if (mlbGame.homeScore === null && mlbGame.awayScore === null) {
            console.log('RAINED OUT ################################################################');
            continue;
        }

        await postMLBGamesBySeason(mlbGame, season);
    }
};


const loadLeagueGamesBySeason = async () => {
    //const years = ['2015_16', '2016_17', '2017_18', '2018_19', '2019_20', '2020_21', '2021_22', '2022_23', '2023_24'];
    let years = ['2024_25'];
    //let tableLength = await getJsonResponseStartup(`/api/tablelength/leagueGames${years[0]}`);
    //tableLength = (tableLength[0].count)
    //console.log(tableLength)
    for (let i = 0; i < years.length; i++) {
        const tableLength = await getJsonResponseStartup(`api/tablelength/league_games_${years[i]}`);
        console.log(tableLength);
        const gamesArray = await getJsonResponseStartup(`api/leagueGames/read/${years[i]}`);
        //console.log(gamesArray.resultSets.length)
        console.log(gamesArray.resultSets.length);
        for (let j = 0; j < gamesArray.resultSets.length; j++) {
            for (let m = tableLength.count; m < gamesArray.resultSets[j].rowSet.length; m++) {
                console.log(m)
                //ACTIVATE CODE IF YOU NEED TO LOAD SHOTS INTO YOUR DATABASE

                await postLeagueGamesBySeason(gamesArray.resultSets[j].rowSet[m], years[i]);
                console.log(gamesArray.resultSets[j].rowSet[m]);
            }
        }
    }
    console.log('FINISHED!!!!!!!!!!!!!!!!!!!!!!1');
}


const loadPlayers = async () => {
    const players = await getJsonResponseStartup('/api/players/read');
    console.log(players);


    for (let i = 0; i < players.length; i++) {
        console.log(players[i])
        console.log(typeof players[i].is_active)
        console.log(typeof players[i].full_name)

        const player = {
            full_name: players[i].full_name,
            first_name: players[i].first_name,
            last_name: players[i].last_name,
            is_active: players[i].is_active,
            player_id: players[i].id.toString()
        }
        console.log(player);
        await postPlayersNBA(player);
    }
    console.log('FINISHED!');
}

function minutesToDecimal(minutesString: string) {
    // Split the input string into minutes and seconds
    const [minutes, seconds] = minutesString.split(':').map(Number);
    // Calculate the decimal value
    const decimalValue = minutes + seconds / 60;
    // Return the decimal value rounded to 2 decimal places
    return decimalValue.toFixed(2);
}

const loadBoxScoresTraditional = async () => {
    const season = "2024_25";
    const tablelength = await getJsonResponseStartup(`/api/tablelength/box_score_traditional_${season}`)
    console.log(tablelength.count);
    const data = await getJsonResponseStartup(`/api/BoxScoreTraditional/read/${season}`);
    console.log(data.length);
    for (let i = tablelength.count - 1; i < data.length; i++) {
        if (data[i].MIN === 'MIN') {
            continue;
        }
        const mins = minutesToDecimal(data[i].MIN)

        const boxScore = {
            game_id: data[i].GAME_ID,
            team_id: data[i].TEAM_ID,
            team_abbreviation: data[i].TEAM_ABBREVIATION,
            team_city: data[i].TEAM_CITY,
            player_id: data[i].PLAYER_ID,
            player_name: data[i].PLAYER_NAME,
            nickname: data[i].NICKNAME,
            start_position: data[i].START_POSITION,
            comment: data[i].COMMENT,
            min: parseFloat(mins),
            fgm: parseFloat(data[i].FGM),
            fga: parseFloat(data[i].FGA),
            fg_pct: parseFloat(data[i].FG_PCT),
            fg3m: parseFloat(data[i].FG3M),
            fg3a: parseFloat(data[i].FG3A),
            fg3_pct: parseFloat(data[i].FG3_PCT),
            ftm: parseFloat(data[i].FTM),
            fta: parseFloat(data[i].FTA),
            ft_pct: parseFloat(data[i].FT_PCT),
            oreb: parseFloat(data[i].OREB),
            dreb: parseFloat(data[i].DREB),
            reb: parseFloat(data[i].REB),
            ast: parseFloat(data[i].AST),
            stl: parseFloat(data[i].STL),
            blk: parseFloat(data[i].BLK),
            tov: parseFloat(data[i].TO),
            pf: parseFloat(data[i].PF),
            pts: parseFloat(data[i].PTS),
            plus_minus: parseFloat(data[i].PLUS_MINUS)
        }

        for (const key in boxScore) {
            if (Object.prototype.hasOwnProperty.call(boxScore, key)) {
                if (boxScore[key as keyof typeof boxScore] === "") {
                    boxScore[key as keyof typeof boxScore] = null;
                }
            }
        }

        await postBoxScoresTraditionalBySeason(boxScore, season);
    }
    //let data = await getJsonResponseStartup(`/boxScoresTraditional/read/${season}`);

    //for (let i = 0; i < data.length; i++) {
    //    await postBoxScoresTraditionalBySeason(data[i], season);
    //} 
}

const loadBoxScoresAdvanced = async () => {
    const season = "2024_25";
    const tablelength = await getJsonResponseStartup(`/api/tablelength/box_score_advanced_${season}`)
    //tablelength = tablelength[0].count
    console.log(tablelength);
    const data = await getJsonResponseStartup(`/api/BoxScoreAdvanced/read/${season}`);
    //REPLACE i WITH tablelength.count - 1 AFTER INITIAL LOAD
    for (let i = tablelength.count - 1; i < data.length; i++) {
        if (data[i].MIN === 'MIN') {
            continue;
        }
        const mins = minutesToDecimal(data[i].MIN)

        const boxScore = {
            game_id: data[i].GAME_ID,
            team_id: data[i].TEAM_ID,
            team_abbreviation: data[i].TEAM_ABBREVIATION,
            team_city: data[i].TEAM_CITY,
            player_id: data[i].PLAYER_ID,
            player_name: data[i].PLAYER_NAME,
            nickname: data[i].NICKNAME,
            start_position: data[i].START_POSITION,
            comment: data[i].COMMENT,
            min: parseFloat(mins),
            e_off_rating: parseFloat(data[i].E_OFF_RATING),
            off_rating: parseFloat(data[i].OFF_RATING),
            e_def_rating: parseFloat(data[i].E_DEF_RATING),
            def_rating: parseFloat(data[i].DEF_RATING),
            e_net_rating: parseFloat(data[i].E_NET_RATING),
            net_rating: parseFloat(data[i].NET_RATING),
            ast_pct: parseFloat(data[i].AST_PCT),
            ast_tov: parseFloat(data[i].AST_TOV),
            ast_ratio: parseFloat(data[i].AST_RATIO),
            oreb_pct: parseFloat(data[i].OREB_PCT),
            dreb_pct: parseFloat(data[i].DREB_PCT),
            reb_pct: parseFloat(data[i].REB_PCT),
            tm_tov_pct: parseFloat(data[i].TM_TOV_PCT),
            efg_pct: parseFloat(data[i].EFG_PCT),
            ts_pct: parseFloat(data[i].TS_PCT),
            usg_pct: parseFloat(data[i].USG_PCT),
            e_usg_pct: parseFloat(data[i].E_USG_PCT),
            e_pace: parseFloat(data[i].E_PACE),
            pace: parseFloat(data[i].PACE),
            pace_per40: parseFloat(data[i].PACE_PER40),
            poss: parseFloat(data[i].POSS),
            pie: parseFloat(data[i].PIE)
        }

        for (const key in boxScore) {
            if (Object.prototype.hasOwnProperty.call(boxScore, key)) {
                if (boxScore[key as keyof typeof boxScore] === "") {
                    boxScore[key as keyof typeof boxScore] = null;
                }
            }
        }
        console.log(boxScore)

        await postBoxScoresAdvancedBySeason(boxScore, season);
    }
}

const loadBoxScoresFourFactors = async () => {
    const season = "2024_25";
    const tablelength = await getJsonResponseStartup(`/api/tablelength/box_score_fourfactors_${season}`)
    //tablelength = tablelength[0].count
    const data = await getJsonResponseStartup(`/api/BoxScoreFourFactors/read/${season}`);
    for (let i = tablelength.count; i < data.length; i++) {
        if (data[i].MIN === 'MIN') {
            continue;
        }
        const mins = minutesToDecimal(data[i].MIN)

        const boxScore = {
            game_id: data[i].GAME_ID,
            team_id: data[i].TEAM_ID,
            team_abbreviation: data[i].TEAM_ABBREVIATION,
            team_city: data[i].TEAM_CITY,
            player_id: data[i].PLAYER_ID,
            player_name: data[i].PLAYER_NAME,
            nickname: data[i].NICKNAME,
            start_position: data[i].START_POSITION,
            comment: data[i].COMMENT,
            min: parseFloat(mins),
            efg_pct: parseFloat(data[i].EFG_PCT),
            fta_rate: parseFloat(data[i].FTA_RATE),
            tm_tov_pct: parseFloat(data[i].TM_TOV_PCT),
            oreb_pct: parseFloat(data[i].OREB_PCT),
            opp_efg_pct: parseFloat(data[i].OPP_EFG_PCT),
            opp_fta_rate: parseFloat(data[i].OPP_FTA_RATE),
            opp_tov_pct: parseFloat(data[i].OPP_TOV_PCT),
            opp_oreb_pct: parseFloat(data[i].OPP_OREB_PCT)
        }

        for (const key in boxScore) {
            if (Object.prototype.hasOwnProperty.call(boxScore, key)) {
                if (boxScore[key as keyof typeof boxScore] === "") {
                    boxScore[key as keyof typeof boxScore] = null;
                }
            }
        }
        console.log(boxScore)

        await postBoxScoresFourFactorsBySeason(boxScore, season);
    }
    //let data = await getJsonResponseStartup(`/boxScoresTraditional/read/${season}`);

    //for (let i = 0; i < data.length; i++) {
    //    await postBoxScoresTraditionalBySeason(data[i], season);
    //} 
}

const loadBoxScoresMisc = async () => {
    const season = "2024_25";
    const tablelength = await getJsonResponseStartup(`/api/tablelength/box_score_misc_${season}`)
    //tablelength = tablelength[0].count
    const data = await getJsonResponseStartup(`/api/BoxScoreMisc/read/${season}`);
    for (let i = tablelength.count - 1; i < data.length; i++) {
        if (data[i].MIN === 'MIN') {
            continue;
        }
        const mins = minutesToDecimal(data[i].MIN)

        const boxScore = {
            game_id: data[i].GAME_ID,
            team_id: data[i].TEAM_ID,
            team_abbreviation: data[i].TEAM_ABBREVIATION,
            team_city: data[i].TEAM_CITY,
            player_id: data[i].PLAYER_ID,
            player_name: data[i].PLAYER_NAME,
            nickname: data[i].NICKNAME,
            start_position: data[i].START_POSITION,
            comment: data[i].COMMENT,
            min: parseFloat(mins),
            pts_off_tov: parseFloat(data[i].PTS_OFF_TOV),
            pts_2nd_chance: parseFloat(data[i].PTS_2ND_CHANCE),
            pts_fb: parseFloat(data[i].PTS_FB),
            pts_paint: parseFloat(data[i].PTS_PAINT),
            opp_pts_off_tov: parseFloat(data[i].OPP_PTS_OFF_TOV),
            opp_pts_2nd_chance: parseFloat(data[i].OPP_PTS_2ND_CHANCE),
            opp_pts_fb: parseFloat(data[i].OPP_PTS_FB),
            opp_pts_paint: parseFloat(data[i].OPP_PTS_PAINT),
            blk: parseFloat(data[i].BLK),
            blka: parseFloat(data[i].BLKA),
            pf: parseFloat(data[i].PF),
            pfd: parseFloat(data[i].PFD)
        }

        for (const key in boxScore) {
            if (Object.prototype.hasOwnProperty.call(boxScore, key)) {
                if (boxScore[key as keyof typeof boxScore] === "") {
                    boxScore[key as keyof typeof boxScore] = null;
                }
            }
        }
        console.log(boxScore)

        await postBoxScoresMiscBySeason(boxScore, season);
    }
}

const loadBoxScoresScoring = async () => {
    //const season = ["2017_18", "2018_19", "2019_20", "2020_21", "2021_22", "2022_23", "2023_24"];
    let season = ["2024_25"];
    for (let j = 0; j < season.length; j++) {

        const tablelength = await getJsonResponseStartup(`/api/tablelength/box_score_scoring_${season[j]}`)
        //tablelength = tablelength[0].count
        const data = await getJsonResponseStartup(`/api/BoxScoreScoring/read/${season[j]}`);
        for (let i = tablelength.count - 1; i < data.length; i++) {
            if (data[i].MIN === 'MIN') {
                continue;
            }
            const mins = minutesToDecimal(data[i].MIN)

            const boxScore = {
                game_id: data[i].GAME_ID,
                team_id: data[i].TEAM_ID,
                team_abbreviation: data[i].TEAM_ABBREVIATION,
                team_city: data[i].TEAM_CITY,
                player_id: data[i].PLAYER_ID,
                player_name: data[i].PLAYER_NAME,
                nickname: data[i].NICKNAME,
                start_position: data[i].START_POSITION,
                comment: data[i].COMMENT,
                min: parseFloat(mins),
                pct_fga_2pt: parseFloat(data[i].PCT_FGA_2PT),
                pct_fga_3pt: parseFloat(data[i].PCT_FGA_3PT),
                pct_pts_2pt: parseFloat(data[i].PCT_PTS_2PT),
                pct_pts_2pt_mr: parseFloat(data[i].PCT_PTS_2PT_MR),
                pct_pts_3pt: parseFloat(data[i].PCT_PTS_3PT),
                pct_pts_fb: parseFloat(data[i].PCT_PTS_FB),
                pct_pts_ft: parseFloat(data[i].PCT_PTS_FT),
                pct_pts_off_tov: parseFloat(data[i].PCT_PTS_OFF_TOV),
                pct_pts_paint: parseFloat(data[i].PCT_PTS_PAINT),
                pct_ast_2pm: parseFloat(data[i].PCT_AST_2PM),
                pct_uast_2pm: parseFloat(data[i].PCT_UAST_2PM),
                pct_ast_3pm: parseFloat(data[i].PCT_AST_3PM),
                pct_uast_3pm: parseFloat(data[i].PCT_UAST_3PM),
                pct_ast_fgm: parseFloat(data[i].PCT_AST_FGM),
                pct_uast_fgm: parseFloat(data[i].PCT_UAST_FGM)

            }

            for (const key in boxScore) {
                if (Object.prototype.hasOwnProperty.call(boxScore, key)) {
                    if (boxScore[key as keyof typeof boxScore] === "") {
                        boxScore[key as keyof typeof boxScore] = null;
                    }
                }
            }
            console.log(boxScore)

            await postBoxScoresScoringBySeason(boxScore, season[j]);
        }
    }
}

const loadShotsBySeason = async () => {
    const years = ['2024_25'];

    //let years = ['2015_16', '2016_17', '2017_18', '2018_19', '2019_20', '2020_21', '2021_22', '2022_23', '2023_24'];
    
    for (let j = 0; j < years.length; j++) {   
        const tableLength = await getJsonResponseStartup(`/api/tablelength/shots_${years[j]}`);
        console.log(tableLength);
        const shotsArray = await getJsonResponseStartup(`/api/Shot/read/${years[j]}`);
        console.log(shotsArray.resultSets[0].rowSet.length)


        for (let m = tableLength.count - 1; m < shotsArray.resultSets[0].rowSet.length; m++) {
            console.log(m)

            const shotObject = {
                grid_type: shotsArray.resultSets[0].rowSet[m][0],
                game_id: shotsArray.resultSets[0].rowSet[m][1].toString(),
                game_event_id: shotsArray.resultSets[0].rowSet[m][2].toString(),
                player_id: shotsArray.resultSets[0].rowSet[m][3].toString(),
                player_name: shotsArray.resultSets[0].rowSet[m][4],
                team_id: shotsArray.resultSets[0].rowSet[m][5].toString(),
                team_name: shotsArray.resultSets[0].rowSet[m][6],
                period: shotsArray.resultSets[0].rowSet[m][7].toString(),
                minutes_remaining: shotsArray.resultSets[0].rowSet[m][8].toString(),
                seconds_remaining: shotsArray.resultSets[0].rowSet[m][9].toString(),
                event_type: shotsArray.resultSets[0].rowSet[m][10],
                action_type: shotsArray.resultSets[0].rowSet[m][11],
                shot_type: shotsArray.resultSets[0].rowSet[m][12],
                shot_zone_basic: shotsArray.resultSets[0].rowSet[m][13],
                shot_zone_area: shotsArray.resultSets[0].rowSet[m][14],
                shot_zone_range: shotsArray.resultSets[0].rowSet[m][15],
                shot_distance: shotsArray.resultSets[0].rowSet[m][16].toString(),
                loc_x: shotsArray.resultSets[0].rowSet[m][17].toString(),
                loc_y: shotsArray.resultSets[0].rowSet[m][18].toString(),
                shot_attempted_flag: shotsArray.resultSets[0].rowSet[m][19].toString(),
                shot_made_flag: shotsArray.resultSets[0].rowSet[m][20].toString(),
                game_date: shotsArray.resultSets[0].rowSet[m][21].toString(),
                htm: shotsArray.resultSets[0].rowSet[m][22],
                vtm: shotsArray.resultSets[0].rowSet[m][23]
            }
            //ACTIVATE CODE IF YOU NEED TO LOAD SHOTS INTO YOUR DATABASE
            await postShotBySeason(shotObject, years[j]);
        }
    }


    console.log('FINISHED!!!!!!!!!!!!!!!!!!!!!!1');
}

const loadLeagueDashLineupsFunction = async (season: string, boxType: string, numPlayers: string) => {
    const results = await getJsonResponseStartup(`/api/LeagueDashLineups/read/${season}/${boxType}/${numPlayers}`);
    console.log(results.resultSets[0].rowSet.length)
    console.log(results.resultSets[0].rowSet)

    for (let i = 0; i < results.resultSets[0].rowSet.length; i++) {

        await postLeagueDashLineups(results.resultSets[0].rowSet[i], season, boxType, numPlayers);
    }
}

const loadNewOddsFunction = async () => {
    const season = "2024_25";

    const data = await getJsonResponseStartup(`/api/Gambling/read/newOdds/${season}`);
    for (let i = 0; i < data.length; i++) {
        console.log(data[i]);
        await postNewOdds(data[i], season);
    }
}

const loadBoxScoreSummary = async () => {
    //const season = ["2019_20", "2020_21", "2021_22", "2022_23", "2023_24"];
    let season = ["2024_25"];
    for (let j = 0; j < season.length; j++) {
        const tablelength = await getJsonResponseStartup(`/api/tablelength/box_score_summary_${season[j]}`)
        //tablelength = tablelength[0].count
        const data = await getJsonResponseStartup(`/api/BoxScores/read/${season[j]}/summary/5`);
        console.log(tablelength);
        console.log(data);
        for (let i = tablelength.count - 1; i < data.length; i++) {
            if (data[i].GAME_ID === 'GAME_ID') {
                continue;
            }

            const boxScore = {
                game_date_est: data[i].GAME_DATE_EST,
                game_sequence: data[i].GAME_SEQUENCE,
                game_id: data[i].GAME_ID,
                game_status_id: data[i].GAME_STATUS_ID,
                game_status_text: data[i].GAME_STATUS_TEXT,
                gamecode: data[i].GAMECODE,
                home_team_id: data[i].HOME_TEAM_ID,
                visitor_team_id: data[i].VISITOR_TEAM_ID,
                season: data[i].SEASON,
                live_period: data[i].LIVE_PERIOD,
                live_pc_time: data[i].LIVE_PC_TIME,
                natl_tv_broadcaster_abbreviation: data[i].NATL_TV_BROADCASTER_ABBREVIATION,
                live_period_time_bcast: data[i].LIVE_PERIOD_TIME_BCAST,
                wh_status: data[i].WH_STATUS
            }

            for (const key in boxScore) {
                if (Object.prototype.hasOwnProperty.call(boxScore, key)) {
                    if (boxScore[key as keyof typeof boxScore] === "") {
                        boxScore[key as keyof typeof boxScore] = null;
                    }
                }
            }
            console.log(boxScore)

            await postBoxScoreSummary(boxScore, season[j]);
        }
    }
}


export {
    loadLeagueGamesBySeason,
    loadPlayers,
    loadBoxScoresTraditional,
    loadLeagueDashLineupsFunction,
    loadBoxScoresAdvanced,
    loadBoxScoresFourFactors,
    loadBoxScoresMisc,
    loadBoxScoresScoring,
    loadShotsBySeason,
    loadNewOddsFunction,
    loadBoxScoreSummary,
    loadMLBGames,
}