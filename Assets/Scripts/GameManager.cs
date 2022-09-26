using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

public delegate void DifficultyIncreased(float difficulty);
public delegate void GameplayLoaded();

public delegate void GameOver();

public class GameManager : MonoBehaviour
{
    enum GameManagerStatus {GAMEPLAY,LOADING,GAME_OVER}
    [SerializeField] private float timerThreshold = 5;
    public float TimerThreshold => timerThreshold;

    private float _timer;
    private float _difficulty;
    private GameManagerStatus _state;
   
    private ManagePlayerHealth _playerHealth;
    private GameplayDataManager _gameplayDataManager;
    public event DifficultyIncreased OnDifficultyIncreased;
    public event GameplayLoaded OnGameplayLoaded;
    public event GameOver OnGameOver;

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
        AudioManager.Initialize(audioClipsData);
        OnDifficultyIncreased?.Invoke(_difficulty);
        _state = GameManagerStatus.GAMEPLAY;
        OnGameplayLoaded?.Invoke();
    }

    private void Update()
    {
        CountTime();
        if(_state == GameManagerStatus.GAME_OVER && Input.GetKeyDown(KeyCode.Return))
            ReloadLevel();
    }

    private void CountTime()
    {
         if (_state != GameManagerStatus.GAMEPLAY) return;
         
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
        _state = GameManagerStatus.GAME_OVER;
        OnGameOver?.Invoke();
    }

    private void ReloadLevel()
    {
        AudioManager.Reset();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
