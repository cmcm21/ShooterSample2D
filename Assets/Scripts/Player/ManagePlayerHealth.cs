using System;
using UnityEngine;

public delegate void PlayerDieDelegate();

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(PlayerController))]
public class ManagePlayerHealth : MonoBehaviour
{
    [SerializeField] private int collisionDamage = 100;
    [SerializeField] private int playerHealth = 100;
    [SerializeField] private BlinkingSpriteSFX blinkingSpriteSfx;

    private PlayerController _playerController;
    public event PlayerDieDelegate OnPlayerDie;

    private void Awake()
    {
        blinkingSpriteSfx.OnAnimationFinished += CheckHealth;
    }

    private void Start()
    {
        _playerController = GetComponent<PlayerController>();
    }

    private void OnDestroy()
    {
        blinkingSpriteSfx.OnAnimationFinished -= CheckHealth;
    }


    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag(GameDefinitions.Tags.DamageObject.ToString()))
        {
            var enemy = other.gameObject.GetComponent<BaseEnemy>();
            enemy.GetHit(collisionDamage);
            GetHit(collisionDamage/2);
            blinkingSpriteSfx.Blink();
        }
        else if (other.gameObject.CompareTag(GameDefinitions.Tags.Bullet.ToString()))
        {
            var bullet = other.gameObject.GetComponent<Bullet>();
            other.gameObject.SetActive(false);
            GetHit(bullet.Damage);
            blinkingSpriteSfx.Blink();
        }
    }

    private void GetHit(int damage)
    {
        playerHealth -= damage;
    }

    private void CheckHealth()
    {
        if (playerHealth <= 0)
        {
            blinkingSpriteSfx.Stop(); 
            OnPlayerDie?.Invoke();
        }
    }
}
