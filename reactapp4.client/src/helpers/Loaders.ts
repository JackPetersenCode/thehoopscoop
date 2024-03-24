import { getJsonResponseStartup } from './GetJsonResponse';
import { postLeagueGamesBySeason, postPlayersNBA, postBoxScoresTraditionalBySeason, postLeagueDashLineups, postBoxScoresAdvancedBySeason, postBoxScoresFourFactorsBySeason, postBoxScoresMiscBySeason, postBoxScoresScoringBySeason } from './PostFunctions';

const loadLeagueGamesBySeason = async () => {
    const years = ['2015_2016', '2016_2017', '2017_2018', '2018_2019', '2019_2020', '2020_2021', '2021_2022', '2022_2023', '2023_2024'];
    //let years = ['2016-2017'];
    //let tableLength = await getJsonResponseStartup(`/api/tablelength/leagueGames${years[0]}`);
    //tableLength = (tableLength[0].count)
    //console.log(tableLength)
    for (let i = 0; i < years.length; i++) {
        const tableLength = await getJsonResponseStartup(`api/tablelength/league_games_${years[i]}`);

        const gamesArray = await getJsonResponseStartup(`api/leagueGames/read/${years[i]}`);
        //console.log(gamesArray.resultSets.length)
        for (let j = 0; j < gamesArray.resultSets.length; j++) {
            for (let m = tableLength.count; m < gamesArray.resultSets[j].rowSet.length; m++) {
                console.log(m)
                //ACTIVATE CODE IF YOU NEED TO LOAD SHOTS INTO YOUR DATABASE

                const results = await postLeagueGamesBySeason(gamesArray.resultSets[j].rowSet[m], years[i]);
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
        let results = await postPlayersNBA(player);
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
    const season = "2022_2023";
    const tablelength = await getJsonResponseStartup(`/api/tablelength/box_score_traditional_${season}`)
    //tablelength = tablelength[0].count
    const data = await getJsonResponseStartup(`/api/BoxScoreTraditional/read/${season}`);
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
    const season = "2023_24";
    const tablelength = await getJsonResponseStartup(`/api/tablelength/box_score_advanced_${season}`)
    //tablelength = tablelength[0].count
    const data = await getJsonResponseStartup(`/api/BoxScoreAdvanced/read/${season}`);
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
    const season = "2016_17";
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
    const season = "2015_16";
    const tablelength = await getJsonResponseStartup(`/api/tablelength/box_score_misc_${season}`)
    //tablelength = tablelength[0].count
    const data = await getJsonResponseStartup(`/api/BoxScoreMisc/read/${season}`);
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
    const season = "2015_16";
    const tablelength = await getJsonResponseStartup(`/api/tablelength/box_score_scoring_${season}`)
    //tablelength = tablelength[0].count
    const data = await getJsonResponseStartup(`/api/BoxScoreScoring/read/${season}`);
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

        await postBoxScoresScoringBySeason(boxScore, season);
    }
}

const loadLeagueDashLineupsFunction = async (season: string, boxType: string, numPlayers: string) => {
    const results = await getJsonResponseStartup(`/api/LeagueDashLineups/read/${season}/${boxType}/${numPlayers}`);
    console.log(results.resultSets[0].rowSet.length)
    console.log(results.resultSets[0].rowSet)

    for (let i = 0; i < results.resultSets[0].rowSet.length; i++) {

        await postLeagueDashLineups(results.resultSets[0].rowSet[i], season, boxType, numPlayers);
    }
}


export { loadLeagueGamesBySeason, loadPlayers, loadBoxScoresTraditional, loadLeagueDashLineupsFunction, loadBoxScoresAdvanced, loadBoxScoresFourFactors, loadBoxScoresMisc, loadBoxScoresScoring }