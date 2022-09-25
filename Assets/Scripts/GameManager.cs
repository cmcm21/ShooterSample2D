using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

public delegate void DifficultyIncreased(float difficulty);

public class GameManager : MonoBehaviour
{
    enum GameManagerStatus {STARTED,LOADING}
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private float timerThreshold = 5;
    public float TimerThreshold => timerThreshold;

    private float _timer;
    private float _difficulty;
    private GameManagerStatus _state;
   
    private ManagePlayerHealth _playerHealth;
    private GameplayDataManager _gameplayDataManager;
    public event DifficultyIncreased OnDifficultyIncreased;

    private void Awake()
    {
        _state = GameManagerStatus.LOADING;
    }

    private void Start()
    {
        _timer = 0;
        _difficulty = 1;
        
        _playerHealth = FindObjectOfType<ManagePlayerHealth>();
        if(_playerHealth != null)
            _playerHealth.OnPlayerDie += PlayerHealthOnOnPlayerDie;

        _gameplayDataManager = FindObjectOfType<GameplayDataManager>();
        if(_gameplayDataManager != null)
            _gameplayDataManager.OnClipsDataLoaded += GameplayDataManager_OnClipsDataLoaded;
        else
        {
            Debug.LogError("Gameplay data manager was not founded");
        }
    }

    private void GameplayDataManager_OnClipsDataLoaded(AudioClipsData audioClipsData)
    {
        loadingScreen.SetActive(false);
        AudioManager.Initialize(audioClipsData);
        OnDifficultyIncreased?.Invoke(_difficulty);
        _state = GameManagerStatus.STARTED;
    }

    private void Update()
    {
        if (_state == GameManagerStatus.LOADING) return;
        
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
