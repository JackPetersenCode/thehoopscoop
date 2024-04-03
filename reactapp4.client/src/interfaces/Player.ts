interface Player {
    full_name: string;
    first_name: string;
    last_name: string;
    is_active: string;
    player_id: string;
}

interface SelectedPlayer {
    player_name: string;
    player_id: string;
    team_id: string;
    team_abbreviation: string;
}

export type { Player, SelectedPlayer };