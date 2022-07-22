using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjetsCatcher : MonoBehaviour
{
    [SerializeField] private GameDefinitions.Tags[] toCheckTags;
    private void OnTriggerEnter2D(Collider2D other)
    {
        foreach(var toCheckTag in toCheckTags)
            if(other.CompareTag(toCheckTag.ToString()))
                other.gameObject.SetActive(false);
    }
}
