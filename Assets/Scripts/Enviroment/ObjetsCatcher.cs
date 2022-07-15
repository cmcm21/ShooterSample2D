using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjetsCatcher : MonoBehaviour
{
    [SerializeField] private bool CheckEnemies;
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"other: {other.name}");
        if (other.CompareTag("Bullet"))
            other.gameObject.SetActive(false);
        
        if(other.CompareTag("Enemy") && CheckEnemies)
            other.gameObject.SetActive(false);
    }
}
