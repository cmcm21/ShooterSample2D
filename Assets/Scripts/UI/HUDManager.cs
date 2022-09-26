using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HUDManager : MonoBehaviour
{
    [SerializeField] private ScoreManager score;
    [SerializeField] private GameObject screenPanel;
    [SerializeField] private TextMeshProUGUI screenMessage;
    private void Start()
    {
        var spawnManager = FindObjectOfType<SpawnManager>();
        var gameManager = FindObjectOfType<GameManager>();
        if (spawnManager != null)
            spawnManager.OnEnemyDisabled += SpawnManagerOnOnEnemyDisabled;
        if (gameManager != null)
        {
            gameManager.OnGameOver += GameManager_OnGameOver;
            gameManager.OnGameplayLoaded += GameManager_OnGameplayLoaded; 
        }
    }

    private void GameManager_OnGameplayLoaded()
    {
        screenPanel.SetActive(false);
    }

    private void GameManager_OnGameOver()
    {
        screenPanel.SetActive(true);
        screenMessage.text = "Game over \n Press \' Enter \' to restart";
    }

    private void SpawnManagerOnOnEnemyDisabled(TargetType targetType)
    {
        if(GameDefinitions.PointsPerTarget.TryGetValue(targetType,out var points))
            score.UpdateScore(points);
    }
}
