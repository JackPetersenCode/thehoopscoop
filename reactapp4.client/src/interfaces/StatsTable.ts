type Column = {
    label: string;
    accessor: string;
};

type SortingFunction = (sortField: string, sortOrder: string) => void;

type Stats = {
    [key: string]: number | string;
}

type LeagueDashLineupAdvanced = {
    id: number;
    group_set?: string;
    group_id?: string;
    group_name?: string;
    team_id?: string;
    team_abbreviation?: string;
    gp?: number;
    w?: number;
    l?: number;
    w_pct?: number;
    min?: number;
    e_off_rating?: number;
    off_rating?: number;
    e_def_rating?: number;
    def_rating?: number;
    e_net_rating?: number;
    net_rating?: number;
    ast_pct?: number;
    ast_to?: number;
    ast_ratio?: number;
    oreb_pct?: number;
    dreb_pct?: number;
    reb_pct?: number;
    tm_tov_pct?: number;
    efg_pct?: number;
    ts_pct?: number;
    e_pace?: number;
    pace?: number;
    pace_per40?: number;
    poss?: number;
    pie?: number;
    gp_rank?: number;
    w_rank?: number;
    l_rank?: number;
    w_pct_rank?: number;
    min_rank?: number;
    off_rating_rank?: number;
    def_rating_rank?: number;
    net_rating_rank?: number;
    ast_pct_rank?: number;
    ast_to_rank?: number;
    ast_ratio_rank?: number;
    oreb_pct_rank?: number;
    dreb_pct_rank?: number;
    reb_pct_rank?: number;
    tm_tov_pct_rank?: number;
    efg_pct_rank?: number;
    ts_pct_rank?: number;
    pace_rank?: number;
    pie_rank?: number;
};

export type { Column, SortingFunction, LeagueDashLineupAdvanced, Stats }