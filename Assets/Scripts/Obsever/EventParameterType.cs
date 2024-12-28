/// <summary>
/// Defines types of parameters that can be sent within a triggered event.
/// Enum constants follow the format 'EventID_KeyName' for clarity.
/// </summary>
public enum EventParameterType{
    None = 0,
    ResetWalkableTile_WalkableTileObject,
    AddMoreObstacle_ListObstaclePrefab,
    ObstacleTileSpawned_WalkableTileObjectAndListSpawnPositions,
    InitializeUpdateGame_Null,
    InitializeResetUpdateGame_Null,
    PauseUpdateGame_Null,
    InitializeUpdatePhaseChanging_TupleColor,
    ChangePhase_Null,
    B_Cube_Collide_CubeObject,
    W_Cube_Collide_CubeObject,
    Player_Collide_PlayerObject,
    Player_TakeCoin_Null,
}