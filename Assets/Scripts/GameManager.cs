using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public delegate void DifficultyIncreased(float difficulty);

public class GameManager : MonoBehaviour
{
    [SerializeField] private float timerThreshold = 5;
    public float TimerThreshold => timerThreshold;
    
    private float _timer;
    private float _difficulty;
   
    private ManagePlayerHealth _playerHealth;
    public event DifficultyIncreased OnDifficultyIncreased;
    private void Start()
    {
        _timer = 0;
        _difficulty = 0;
        
        _playerHealth = FindObjectOfType<ManagePlayerHealth>();
        if(_playerHealth != null)
            _playerHealth.OnPlayerDie += PlayerHealthOnOnPlayerDie;
        
        OnDifficultyIncreased?.Invoke(_difficulty);
    }

    private void Update()
    {
        _timer += Time.deltaTime;

        if (_timer >= timerThreshold)
        {
            _timer = 0;
            IncreaseDifficulty();
        }
    }

    private void IncreaseDifficulty()
    {
        _difficulty++;
        OnDifficultyIncreased?.Invoke(_difficulty);
        Debug.Log($"Difficulty increased to level {_difficulty}");
    }

    private void OnDestroy()
    {
        _playerHealth.OnPlayerDie -= PlayerHealthOnOnPlayerDie;
    }

    private void PlayerHealthOnOnPlayerDie()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
