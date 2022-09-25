using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void OnTargetDisabled(GameObject enemyGO);
public enum Target_State {MOVE, IDLE, DESTROYING,DESTROYED}
public class ManageEnemyHealth : MonoBehaviour
{
    [SerializeField] private BlinkingSpriteSFX blinkingSpriteSfx;
    [SerializeField] private ExplosionAnimation explosionAnimation;
    [SerializeField] private TargetType targetType;
    public TargetType TargetType => targetType;
    [SerializeField] private GameObject sprite;
    
    
    private int health = 0;
    private Target_State State;
    public event OnTargetDisabled TargetDisabled;

    private void Start()
    {
        State = Target_State.IDLE;
        explosionAnimation.onExplosionFinished += OnExplosionAnimationFinished;
        blinkingSpriteSfx.OnAnimationFinished += CheckHealth;
        
        SetType();
    }

    private void OnEnable()
    {
        sprite.SetActive(true); 
        State = Target_State.IDLE;
    }

    private void OnExplosionAnimationFinished()
    {
        State = Target_State.DESTROYED;
        explosionAnimation.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }
 
    private void Die()
    {
        State = Target_State.DESTROYING;
        blinkingSpriteSfx.Stop();
        sprite.SetActive(false);
        explosionAnimation.gameObject.SetActive(true);
        AudioManager.PlayPositionalAudio(GameDefinitions.SFXClip.Explosion,transform.position);
    }   
    
    private void CheckHealth()
    {
        if (health <= 0)
            Die();
    }
    
    private void SetType()
    {
        health = GameDefinitions.HealthPerTargetType[targetType];
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
         if(State == Target_State.DESTROYED) return;
         if (other.collider.CompareTag("Bullet"))
         {
             other.gameObject.SetActive(false);
             if (State == Target_State.DESTROYING) return;
             var damage = other.collider.GetComponent<Bullet>().Damage;
             blinkingSpriteSfx.Blink();
             GetHit(damage);
         }
    }
    public void GetHit(int damage)
    {
        health -= damage; 
        CheckHealth();
    }   
    private void OnDestroy()
    {
        blinkingSpriteSfx.OnAnimationFinished -= CheckHealth;
        explosionAnimation.onExplosionFinished -= OnExplosionAnimationFinished;
    }   
    private void OnDisable()
    {
        TargetDisabled?.Invoke(gameObject);
    }
}
