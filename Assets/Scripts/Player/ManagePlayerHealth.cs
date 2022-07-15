using System;
using UnityEngine;

public delegate void PlayerDieDelegate();

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(PlayerController))]
public class ManagePlayerHealth : MonoBehaviour
{
    [SerializeField] private int collisionDamage = 100;
    [SerializeField] private int playerHealth = 100;

    private PlayerController _playerController;
    public event PlayerDieDelegate OnPlayerDie;

    private void Start()
    {
        _playerController = GetComponent<PlayerController>();
    }
    
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            var enemy = other.gameObject.GetComponent<BaseEnemy>();
            enemy.GetHit(collisionDamage);
            GetHit(collisionDamage/2);
        }
    }

    private void GetHit(int damage)
    {
        playerHealth -= damage;
        if (playerHealth <= 0)
            OnPlayerDie?.Invoke();
    }
}
