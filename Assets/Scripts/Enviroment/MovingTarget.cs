using System;
using UnityEngine;


[RequireComponent(typeof(Collider2D))]
public class MovingTarget : MonoBehaviour
{
    [SerializeField] private float speed;
    
    private Rigidbody2D _rigidbody2D;
    private float[] xRange = new float[2] { -8f,8f };

    public void SetPosition(Vector3 position)
    {
        transform.position = position;
    }

    private void OnEnable()
    {
        Restart();    
        GetDependencies();
        Move();
    }

    private void Restart()
    {
        transform.rotation = Quaternion.identity;
    }

    private void GetDependencies()
    {
        if (_rigidbody2D == null)
            _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Move()
    {
        _rigidbody2D.AddForce(Vector2.down * speed);
    }

    private void FixedUpdate()
    {
        if (transform.position.x < xRange[0])
            transform.position = new Vector3(xRange[1], transform.position.y);
        else if (transform.position.x > xRange[1])
            transform.position = new Vector3(xRange[0], transform.position.y);
    }

}
