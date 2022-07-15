using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private ManagePlayerHealth _playerHealth;
    private void Start()
    {
        _playerHealth = FindObjectOfType<ManagePlayerHealth>();
        if(_playerHealth != null)
            _playerHealth.OnPlayerDie += PlayerHealthOnOnPlayerDie;
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
