using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour
{
    [SerializeField] private int damage = 5;
    [SerializeField] private float fireForce = 100;
    [SerializeField] private Vector2 shootDirection = Vector2.up;

    public int Damage => damage;
    private Rigidbody2D _rigidbody2D;

    private void OnEnable()
    {
        transform.position = new Vector3(0, 0, 0);
        transform.localPosition = new Vector3(0, 0, 0);
        transform.rotation = Quaternion.identity;
        
        if(_rigidbody2D == null)
            _rigidbody2D = GetComponent<Rigidbody2D>();
        _rigidbody2D.AddForce(shootDirection * fireForce);
    }

    private void Update()
    {
        Debug.Log($"bullet position : {transform.position}");
    }
}
