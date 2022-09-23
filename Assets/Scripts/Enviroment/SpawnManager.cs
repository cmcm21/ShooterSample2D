using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;
using RandomSys = System.Random;

public delegate void SpawnEnemy(GameObject gameObject); 

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private float[] spawnRangeX;
    [SerializeField] private float spawnYPosition;
    [SerializeField] private GameObject spawnableObjectsContainer;
    
    private float _spawnTimeFactor;
    private int _currentTarget;
    private float _maxRespawnTime;
    private GameManager _gameManager;
    private Coroutine _spawnCoroutine;
    private List<GameObject> _targets;
    public event SpawnEnemy OnSpawnEnemy;

    private void Start()
    {
        InitEnemies();
        _maxRespawnTime = 0; 
        
        if(spawnRangeX.Length != 2)
            Debug.LogError("Spawn range was wrong set");

        _gameManager = FindObjectOfType<GameManager>();
        if (_gameManager != null)
        {
            _gameManager.OnDifficultyIncreased += GameManager_OnDifficultyIncreased;
            _spawnTimeFactor = _gameManager.TimerThreshold;
        }
    }

    private void InitEnemies()
    {
        var rnd = new RandomSys();
        _targets = new List<GameObject>();
        for(int i = 0; i < spawnableObjectsContainer.transform.childCount; i++)
            _targets.Add(spawnableObjectsContainer.transform.GetChild(i).gameObject);

        _targets = _targets.OrderBy(x => rnd.Next()).ToList();
    }

    private void GameManager_OnDifficultyIncreased(float difficulty)
    {
        _maxRespawnTime = _spawnTimeFactor / difficulty;

        if (_spawnCoroutine == null)
            _spawnCoroutine = StartCoroutine(SpawnEnemyCoroutine());
    }

    private void GoOverEnemies()
    {
        if (!_targets[_currentTarget].activeSelf)
            SpawnEnemy(_targets[_currentTarget]);
        
        _currentTarget++;
        _currentTarget %= _targets.Count;
    }

    private IEnumerator SpawnEnemyCoroutine()
    {
        while (isActiveAndEnabled)
        {
            var waitTime = GetRandomTime();
            yield return new WaitForSeconds(waitTime);
            
            GoOverEnemies();
        }
    }

    private void SpawnEnemy(GameObject enemyGo)
    {
        var baseEnemy = enemyGo.GetComponent<MovingTarget>();
        
        baseEnemy.SetPosition(GetRandomPosition());
        enemyGo.gameObject.SetActive(true);
        OnSpawnEnemy?.Invoke(enemyGo);
    }

    private Vector3 GetRandomPosition()
    {
        var spawnXPosition = Random.Range(spawnRangeX[0], spawnRangeX[1]);
        return new Vector3(spawnXPosition, spawnYPosition);
    }

    private float GetRandomTime()
    {
        return Random.Range(0.5f, _maxRespawnTime);
    }
}
