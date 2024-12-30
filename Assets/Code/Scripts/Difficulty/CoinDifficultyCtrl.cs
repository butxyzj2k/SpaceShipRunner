using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinDifficultyCtrl : BaseDifficulty
{
    [Header("CoinDifficultyCtrl")]
    [SerializeField] CoinSpawnerConfig coinSpawnerConfig;
    private float coinSpawnRate;
    private float numCoinSpawnedRate;

    protected override void LoadValue()
    {
        base.LoadValue();

        coinSpawnRate = 0;
        numCoinSpawnedRate = 0;
    }

    public Tuple<float, float> GetCoinSpawnData(){
        return Tuple.Create(coinSpawnRate, numCoinSpawnedRate);
    }

    protected override IEnumerator C_CalculateGameDifficulty()
    {
        while(CheckCanUpdateDifficulty()){

            currentTime += Time.deltaTime;
            
            if(currentTime % coinSpawnerConfig.TimeInterval > 0.02f){
                yield return new WaitForSeconds(Time.deltaTime); 
                continue;
            }

            int currentDifficultyLevel = (int)(currentTime / coinSpawnerConfig.TimeInterval);

            if(currentDifficultyLevel < coinSpawnerConfig.CoinSpawnRates.Count) 
                coinSpawnRate = coinSpawnerConfig.CoinSpawnRates[currentDifficultyLevel];
                
            if(currentDifficultyLevel < coinSpawnerConfig.NumCoinSpawnedRates.Count) 
                numCoinSpawnedRate = coinSpawnerConfig.NumCoinSpawnedRates[currentDifficultyLevel];

            yield return new WaitForSeconds(Time.deltaTime); 
        }

        yield break;
    }

    protected override IEnumerator C_ResetUpdateGameDifficulty()
    {
        yield break;
    }
}