using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

public delegate void PlayerDieDelegate();

public enum State {COMMON, INVINCIBLE}

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(PlayerController))]
public class ManagePlayerHealth : MonoBehaviour
{
    [SerializeField] private int collisionDamage = 100;
    [SerializeField] private int playerHealth = 100;
    [SerializeField] private GameObject shield;
    [SerializeField] private BlinkingSpriteSFX blinkingSpriteSfx;

    private PlayerController _playerController;
    private State _state;
    public event PlayerDieDelegate OnPlayerDie;

    private void Awake()
    {
        blinkingSpriteSfx.OnAnimationFinished += CheckHealth;
        _state = State.COMMON;
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
            var enemy = other.gameObject.GetComponent<ManageEnemyHealth>();
            enemy.GetHit(collisionDamage);
            if(_state == State.INVINCIBLE) return;
            GetHit(collisionDamage/2);
            blinkingSpriteSfx.Blink();
        }
        else if (other.gameObject.CompareTag(GameDefinitions.Tags.Bullet.ToString()))
        {
            other.gameObject.SetActive(false);
            if(_state == State.INVINCIBLE) return;
            var bullet = other.gameObject.GetComponent<Bullet>();
            GetHit(bullet.Damage);
            blinkingSpriteSfx.Blink();
        }
        else if (other.gameObject.CompareTag(GameDefinitions.Tags.Bonus.ToString()))
        {
            var powerUp = other.gameObject.GetComponent<PowerUp>();
            ApplyPowerUp(powerUp);     
        }
    }

    private void ApplyPowerUp(PowerUp powerUp)
    {
        switch (powerUp.PowerUpType)
        {
           case PowerUpType.SHIELD:
               ActivateShield(powerUp.TimeEffect);
               break;
        } 
    }

    private async void ActivateShield(float powerUpTime)
    {
        shield.gameObject.SetActive(true);
        _state = State.INVINCIBLE;
        await Task.Delay((int)(powerUpTime * 1000));
        _state = State.COMMON;
        shield.gameObject.SetActive(false);
    }
    

    private void GetHit(int damage)
    {
        playerHealth -= damage;
    }

    private void CheckHealth()
    {
        Debug.Log("Checking health");
        if (playerHealth <= 0)
        {
            blinkingSpriteSfx.Stop(); 
            OnPlayerDie?.Invoke();
        }
    }
}
