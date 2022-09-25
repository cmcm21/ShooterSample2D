using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 5.5f;
    [SerializeField] private GameObject[] bullets;
    [SerializeField] private Transform cannonPosition;
    
    private int currentBullet = 0;
    private Vector2 input;
    
    private void Update()
    {
        KeepInPOV();
        GetInput();
    }

    private void KeepInPOV()
    {
        Vector3 viewPortPosition = Camera.main.WorldToViewportPoint(transform.position);
        Vector3 viewPortXDelta = Camera.main.WorldToViewportPoint(transform.position + Vector3.left/2);
        Vector3 viewPortYDelta = Camera.main.WorldToViewportPoint(transform.position + Vector3.up);

        float deltaX = viewPortPosition.x - viewPortXDelta.x;
        float deltaY = viewPortYDelta.y - viewPortPosition.y;

        viewPortPosition.x = Mathf.Clamp(viewPortPosition.x, 0 + deltaX, 1 - deltaX);
        viewPortPosition.y = Mathf.Clamp(viewPortPosition.y, 0 + deltaY, 1 - deltaY);

        transform.position = Camera.main.ViewportToWorldPoint(viewPortPosition);
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void GetInput()
    {
       input = Vector2.zero;
       if(Input.GetKey(KeyCode.A)) 
           input += Vector2.left;
       if(Input.GetKey(KeyCode.S)) 
           input += Vector2.down;
       if(Input.GetKey(KeyCode.D))
           input += Vector2.right;
       if(Input.GetKey(KeyCode.W))
           input += Vector2.up;

       input = input.normalized;
       if (Input.GetKeyDown(KeyCode.Space))
           Fire();
    }

    private void Move()
    {
       gameObject.transform.Translate(input * Time.deltaTime * speed);
    }

    private void Fire()
    {
       bullets[currentBullet].SetActive(true);
       bullets[currentBullet].transform.position = new Vector2(cannonPosition.position.x, cannonPosition.position.y);
       currentBullet++;
       currentBullet %= bullets.Length;
       AudioManager.PlayPositionalAudio(GameDefinitions.SFXClip.Fire,transform.position);
    }
}
