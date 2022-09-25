using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;
using RandomSys = System.Random;

public delegate void SpawnEnemy(GameObject gameObject);

public delegate void EnemyDisabled(TargetType targetType);

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
    public event EnemyDisabled OnEnemyDisabled;

    private void Start()
    {
        InitEnemies();
        _maxRespawnTime = 0;
        _spawnCoroutine = null;
        
        if(spawnRangeX.Length != 2)
            Debug.LogError("Spawn range was wrong set");

        _gameManager = FindObjectOfType<GameManager>();
        if (_gameManager != null)
        {
            _gameManager.OnDifficultyIncreased += GameManager_OnDifficultyIncreased;
            _spawnTimeFactor = _gameManager.TimerThreshold;
        }
        else
            Debug.LogError("Game manager was not found");
    }

    private void InitEnemies()
    {
        var rnd = new RandomSys();
        _targets = new List<GameObject>();
        for (int i = 0; i < spawnableObjectsContainer.transform.childCount; i++)
            _targets.Add(spawnableObjectsContainer.transform.GetChild(i).gameObject);

        _targets = _targets.OrderBy(x => rnd.Next()).ToList();
    }

    private void GameManager_OnDifficultyIncreased(float difficulty)
    {
        difficulty = difficulty == 0 ? 1 : difficulty;
        _maxRespawnTime = _spawnTimeFactor / difficulty;

        if (_spawnCoroutine == null)
        {
            _spawnCoroutine = StartCoroutine(SpawnEnemyCoroutine());
            Debug.Log("Spawn coroutine has started");        
        }
        else
            Debug.Log("Spawn coroutine is not null it couldn't start");        
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
        var enemyHealth = enemyGo.GetComponent<ManageEnemyHealth>();

        if (baseEnemy != null)
        {
            baseEnemy.SetPosition(GetRandomPosition());
            enemyGo.gameObject.SetActive(true);
            OnSpawnEnemy?.Invoke(enemyGo);
        }

        if (enemyHealth != null)
            enemyHealth.TargetDisabled += EnemyHealthOnTargetDisabled;
    }

    private void EnemyHealthOnTargetDisabled(GameObject enemyGo)
    {
        var enemyHealth = enemyGo.GetComponent<ManageEnemyHealth>();
        if (enemyHealth != null)
        {
            OnEnemyDisabled?.Invoke(enemyHealth.TargetType); 
            enemyHealth.TargetDisabled -= EnemyHealthOnTargetDisabled;
        }
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
