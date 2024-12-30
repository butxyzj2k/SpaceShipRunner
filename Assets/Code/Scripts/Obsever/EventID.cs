/// <summary>
/// Enumeration of possible EventIDs, defining types of events that can be triggered in the system.
/// </summary>
public enum EventID{
    None = 0,
    ResetWalkableTile,
    AddMoreObstacle,
    ObstacleTileSpawned,
    InitializeUpdateGame,
    InitializeResetUpdateGame,
    PauseUpdateGame,
    InitializeUpdatePhaseChanging,
    ChangePhase,
    B_Cube_Collide,
    W_Cube_Collide,
    Player_Collide,
    Player_TakeCoin,
}