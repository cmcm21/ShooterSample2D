using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HUDManager : MonoBehaviour
{
    [SerializeField] private ScoreManager score;
    private void Start()
    {
        var spawnManager = FindObjectOfType<SpawnManager>();
        if (spawnManager != null)
        {
            spawnManager.OnEnemyDisabled += SpawnManagerOnOnEnemyDisabled;
        }
    }

    private void SpawnManagerOnOnEnemyDisabled(TargetType targetType)
    {
        if(GameDefinitions.PointsPerTarget.TryGetValue(targetType,out var points))
            score.UpdateScore(points);
    }
}
