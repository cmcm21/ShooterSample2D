using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PowerUpType {SHIELD}
public class PowerUp : MonoBehaviour
{
    [SerializeField] private float timeEffect;
    public float TimeEffect => timeEffect;
    [SerializeField] private PowerUpType type;
    public PowerUpType PowerUpType => type;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag(GameDefinitions.Tags.Player.ToString()))
        {
            gameObject.SetActive(false);
        }
        else if (other.gameObject.CompareTag(GameDefinitions.Tags.Bullet.ToString()))
        {
            other.gameObject.SetActive(false);
        }
            
    }
}
