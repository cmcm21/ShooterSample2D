using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private float[] spawnTimeRange;
    [SerializeField] private float[] spawnRangeX;
    [SerializeField] private float spawnYPosition;
    [SerializeField] private GameObject[] targets;
    
    private int currentTarget;

    private void Start()
    {
        if(spawnRangeX.Length != 2)
            Debug.LogError("Spawn range was wrong set");
        if(spawnTimeRange.Length !=  2)
            Debug.LogError("Spawn time rate was wrong set");
        
        foreach (var target in targets)
        {
            var baseEnemy = target.GetComponent<BaseEnemy>();
            baseEnemy.enemyDisable += BaseEnemyOnEnemyDisable;
        }

        InvokeRepeating(nameof(SpawnTarget), GetRandomTime(),GetRandomTime()); 
    }

    private void SpawnTarget()
    {
        if (!targets[currentTarget].activeSelf)
            SpawnEnemy(targets[currentTarget]);
        
        currentTarget++;
        currentTarget %= targets.Length;
    }

    private void BaseEnemyOnEnemyDisable(GameObject enemyGo)
    {
        if(gameObject)
            StartCoroutine(SpawnEnemyCoroutine(enemyGo));
    }

    private IEnumerator SpawnEnemyCoroutine(GameObject enemyGo)
    {
        var waitTime = GetRandomTime();
        yield return new WaitForSeconds(waitTime);
        
        SpawnEnemy(enemyGo);
    }

    private void SpawnEnemy(GameObject enemyGo)
    {
        var baseEnemy = enemyGo.GetComponent<BaseEnemy>();
        
        baseEnemy.SetPosition(GetRandomPosition());
        baseEnemy.SetType(EnemyType.TARGET_BOULDER);
        enemyGo.gameObject.SetActive(true);
    }

    private Vector3 GetRandomPosition()
    {
        var spawnXPosition = Random.Range(spawnRangeX[0], spawnRangeX[1]);
        return new Vector3(spawnXPosition, spawnYPosition);
    }

    private float GetRandomTime()
    {
        return Random.Range(spawnTimeRange[0], spawnTimeRange[1]);
    }

    private void OnDestroy()
    {
        foreach (var target in targets)
        {
            if (!target) continue;
            var baseEnemy = target.GetComponent<BaseEnemy>();
            baseEnemy.enemyDisable -= BaseEnemyOnEnemyDisable;
        }
    }
}
