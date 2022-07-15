using System;
using UnityEngine;

public delegate void OnEnemyDisable(GameObject enemyGO);

public enum NPC_State {MOVE, IDLE, DYING,DIE}
[RequireComponent(typeof(BoxCollider2D))]
public class BaseEnemy : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private BlinkingData blinkingData;
    [SerializeField] private GameObject sprite;
    [SerializeField] private ExplosionAnimation explosionAnimation;
    
    private Rigidbody2D _rigidbody2D;
    private BlinkingEffect _blinkingEffect;
    private SpriteRenderer _spriteRenderer;
    private NPC_State State;

    private float[] xRange = new float[2] { -8f,8f };
    private int health = 0;

    public event OnEnemyDisable enemyDisable;

    public void Start()
    {
        explosionAnimation.onExplosionFinished += OnExplosionAnimationFinished;
        State = NPC_State.IDLE;
    }

    private void OnExplosionAnimationFinished()
    {
        State = NPC_State.DIE;
        explosionAnimation.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }

    public void SetPosition(Vector3 position)
    {
        transform.position = position;
    }

    public void SetType(EnemyType type)
    {
        health = GameDefinitions.HealthPerEnemyType[type];
    }
    
    private void OnEnable()
    {
        sprite.SetActive(true); 
        Reset();    
        GetDependencies();
        Move();
    }

    private void Reset()
    {
        State = NPC_State.IDLE;
        transform.rotation = Quaternion.identity;
        ResetBlinkingEffect();
    }

    private void CheckHealth()
    {
        if (health <= 0)
            Dead();
    }

    private void Dead()
    {
        State = NPC_State.DYING;
        _blinkingEffect.Stop();
        sprite.SetActive(false);
        explosionAnimation.gameObject.SetActive(true);
    }

    private void GetDependencies()
    {
        if (_rigidbody2D == null)
            _rigidbody2D = GetComponent<Rigidbody2D>();
        if (_spriteRenderer == null)
            _spriteRenderer = sprite.GetComponent<SpriteRenderer>();
    }

    private void ResetBlinkingEffect()
    {
        _blinkingEffect ??= new BlinkingEffect(blinkingData);
        _blinkingEffect.OnAnimationFinished += CheckHealth;
    }

    private void Move()
    {
        State = NPC_State.MOVE;
        _rigidbody2D.AddForce(Vector2.down * speed);
    }

    private void Update()
    {
        _blinkingEffect?.Update(Time.deltaTime);
    }

    private void FixedUpdate()
    {
        if (transform.position.x < xRange[0])
            transform.position = new Vector3(xRange[1], transform.position.y);
        else if (transform.position.x > xRange[1])
            transform.position = new Vector3(xRange[0], transform.position.y);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(State == NPC_State.DIE) return;
        if (other.collider.CompareTag("Bullet"))
        {
            other.gameObject.SetActive(false);
            if (State == NPC_State.DYING) return;
            var damage = other.collider.GetComponent<Bullet>().Damage;
            GetHit(damage);
        }
    }

    public void GetHit(int damage)
    {
        health -= damage; 
        _blinkingEffect.GetHit(ref _spriteRenderer);
    }

    private void OnDisable()
    {
        enemyDisable?.Invoke(gameObject);
        _blinkingEffect.OnAnimationFinished -= CheckHealth;
    }

    private void OnDestroy()
    {
        explosionAnimation.onExplosionFinished -= OnExplosionAnimationFinished;
    }
}
