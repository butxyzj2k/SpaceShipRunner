using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyManager : Singleton<DifficultyManager>
{
    private float currentTime;
    private bool isCounting;

    private void Awake()
    {
        gameSpeedRateCalculator = new GameSpeedRateCalculator(gameSpeedRateConfig);
        ResetRateGameSpeed();
    }

    private void Start() {
        StartUpdateGameDifficulty();
    }
    
    /// <summary>
    /// Bắt đầu cập nhật độ khó của trò chơi
    /// </summary>
    /// <param name="resumePreviousState">Tiếp tục tính toán độ khó từ trạng thái hiện tại</param>
    public void StartUpdateGameDifficulty(bool resumePreviousState = true)
    {
        isCounting = true;

        if (resumePreviousState) {
            StartCoroutine(UpdateGameSpeedRate());
            StartCoroutine(UpdateObstacleTileSpawner());
        }

        else StartCoroutine(ResetAndStartRateGameSpeed());
    }

    /// <summary>
    /// Tạm dừng cập nhật độ khó của trò chơi
    /// </summary>
    public void PauseUpdateGameDifficulty()
    {
        isCounting = false;
    }

    /// <summary>
    /// Hủy và reset độ khó của trò chơi
    /// </summary>
    public void StopUpdateGameDifficulty()
    {
        ResetRateGameSpeed();
    }

    #region GameSpeedRate

    [Header("GameSpeedRate")]
    [SerializeField] private GameSpeedRateConfig gameSpeedRateConfig;
    private GameSpeedRateCalculator gameSpeedRateCalculator;
    private float gameSpeedRate;

    /// <summary>
    /// Tốc độ tỉ lệ hiện tại của trò chơi
    /// </summary>
    public float GameSpeedRate => gameSpeedRate;

    // Đặt lại trạng thái về ban đầu
    private void ResetRateGameSpeed()
    {
        isCounting = false;
        currentTime = 0f;
        gameSpeedRate = 0f;
    }

    // Cập nhật tốc độ trò chơi dựa trên thời gian
    private IEnumerator UpdateGameSpeedRate()
    {
        while (isCounting && currentTime <= gameSpeedRateCalculator.GetMaxTimeToRate())
        {
            currentTime += Time.deltaTime;
            gameSpeedRate = gameSpeedRateCalculator.CalculateGameSpeedRate(currentTime);
            yield return null;
        }
    }

    // Đặt lại thời gian rồi bắt đầu lại
    private IEnumerator ResetAndStartRateGameSpeed()
    {
        while (isCounting && currentTime > 0)
        {
            currentTime = Mathf.Clamp(currentTime - 10 * Time.deltaTime, 0, currentTime); // Giảm thời gian nhanh hơn để reset
            gameSpeedRate = gameSpeedRateCalculator.CalculateGameSpeedRate(currentTime);
            yield return null;
        }

        StartCoroutine(UpdateGameSpeedRate());
    }
    
    #endregion

    #region UpdateObstacleTileSpawner

    [SerializeField] private ObstacleTileSpawnerConfig obstacleTileSpawnerConfig;

    // Kiểm tra thông báo update obstacle tới ObstacleTileSpawner
    private IEnumerator UpdateObstacleTileSpawner(){
        while(isCounting && currentTime <= gameSpeedRateCalculator.GetMaxTimeToRate()){

            if(currentTime % obstacleTileSpawnerConfig.TimeInterval > 0.02f){
                yield return null;
                continue;
            }

            switch((int)(currentTime / obstacleTileSpawnerConfig.TimeInterval)){
                case 0:
                    Observer.PostEvent(EventID.AddMoreObstacle, new KeyValuePair<EventParameterType, object>(EventParameterType.AddMoreObstacle_ListObstaclePrefab, obstacleTileSpawnerConfig.ObstacleTilePrefabs_1));
                    break;
                case 1: 
                    Observer.PostEvent(EventID.AddMoreObstacle, new KeyValuePair<EventParameterType, object>(EventParameterType.AddMoreObstacle_ListObstaclePrefab, obstacleTileSpawnerConfig.ObstacleTilePrefabs_2));
                    break;
                case 2: 
                    Observer.PostEvent(EventID.AddMoreObstacle, new KeyValuePair<EventParameterType, object>(EventParameterType.AddMoreObstacle_ListObstaclePrefab, obstacleTileSpawnerConfig.ObstacleTilePrefabs_3));
                    break;
            }

            yield return null;
        }
        
    }

    #endregion
}

internal class GameSpeedRateCalculator
{
    private readonly GameSpeedRateConfig config;

    internal GameSpeedRateCalculator(GameSpeedRateConfig config)
    {
        this.config = config;
    }

    /// <summary>
    /// Tính tốc độ tỷ lệ của trò chơi dựa trên thời gian đã qua.
    /// </summary>
    /// <param name="_time">Thời gian đã qua (theo giây).</param>
    /// <returns>Giá trị tỷ lệ tốc độ trò chơi.</returns>
    internal float CalculateGameSpeedRate(float elapsedTime)
    {
        //Tính thời gian hiện tại để người chơi có thể phản ứng với chướng ngại vật
        //Cứ 15s lại tăng cho người chơi 0.5s để giảm độ khó của game và tạo bất ngờ
        //_time càng lơn thì thời gian để người chơi có thể phản ứng với chướng ngại vật càng giảm đi -> tăng độ khó cho game
        float currentReactionTime = Math.Max(
            config.MinObstacleTimeToReact,
            config.MaxObstacleTimeToReact 
            - config.ObstacleTimeToReactReducePerSecond * elapsedTime 
            + config.SurpriseTimeBonus * (int)(elapsedTime / config.SurpriseTimeInterval)
        );
        //Vận tốc của Obstacle hiện tại
        float currentObstacleSpeed = 1 / currentReactionTime;
        //Độ chênh lệch vận tốc của Obstacle hiện tại
        float defaultObstacleSpeed = 1 / config.MaxObstacleTimeToReact;
        //Vận tốc Obstacle ban đầu
        float initialObstacleSpeed = 1 / config.MaxObstacleTimeToReact;

        // Cho ra tốc độ tỉ lệ của trò chơi
        return (currentObstacleSpeed - defaultObstacleSpeed) / initialObstacleSpeed;
    }

    /// <summary>
    /// Lấy quãng thời gian lớn nhất để có thể rate tỷ lệ tốc độ trò chơi, có thể chênh so với kết quả <= 10s
    /// </summary>
    internal int GetMaxTimeToRate()
    {
        //Tính dựa trên phương trình
        // 15 * MinObstacleTimeToRect = 15 * MaxObstacleTimeToRect - 15 * ObstacleTimeToRectReducePerSecond * x + 0.5 * (int)x;
        float x1 = config.SurpriseTimeInterval * (config.MaxObstacleTimeToReact - config.MinObstacleTimeToReact);
        float x2 = config.SurpriseTimeInterval * config.ObstacleTimeToReactReducePerSecond - config.SurpriseTimeBonus;
        return (int)(x1 / x2);
    }
}