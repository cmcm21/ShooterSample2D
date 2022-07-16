using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObject : MonoBehaviour
{
    [SerializeField] private float rotationSpeed;

    private void FixedUpdate()
    {
       transform.Rotate(Vector3.forward,rotationSpeed * Time.deltaTime,Space.World); 
    }
}
