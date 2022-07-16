using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveNPC : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private Transform cannon;
    [SerializeField] private float detectionScope;
    [SerializeField] private GameObject[] bullets;
    [SerializeField] private float coolDown = 1.5f;
    
    private enum State {CANSHOOT,COOLDOWN} 

    private float _direction = 1.0f;
    private float _timer = 0f;
    private float _fireCoolDown = 0f;
    private State state;

    private void OnEnable()
    {
        state = State.CANSHOOT;
    }

    private void Update()
    {
        if (state == State.COOLDOWN)
        {
            _fireCoolDown += Time.deltaTime;
            if (_fireCoolDown >= coolDown)
            {
                _fireCoolDown = 0;
                state = State.CANSHOOT;
            }
        }
    }
    
    private void FixedUpdate()
    {
        DetectTarget();
        Move();
    }
    
    private void Move()
    {
        _timer += Time.deltaTime;
        transform.Translate(Vector3.left * _direction * Time.deltaTime * speed);
        if (_timer >= 2) { _direction *= -1; _timer = 0; }
    }
    
    private void DetectTarget()
    {
        var hit = Physics2D.Raycast(cannon.position, -Vector2.up);
        Debug.DrawRay(transform.position,Vector3.down*detectionScope, Color.red);
        if (hit.collider != null && hit.collider.CompareTag("Player"))
        {
            float distance = Mathf.Abs(hit.point.y - transform.position.y);
            if(distance <= detectionScope && state == State.CANSHOOT)
                Shoot();
        }
    }

    private void Shoot()
    {
        for(int i = 0; i < bullets.Length; i++)
        {
            if(bullets[i].activeSelf) continue;
            bullets[i].SetActive(true);
            bullets[i].transform.position = new Vector2(cannon.position.x, cannon.position.y);
            state = State.COOLDOWN;
            break;
        }
    }
}
