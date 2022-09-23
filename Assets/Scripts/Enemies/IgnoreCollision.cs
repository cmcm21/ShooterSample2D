using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class IgnoreCollision : MonoBehaviour
{
    private int _meteorLayer;
    private int _npcLayer;
    private bool _collisionIgnoredSetted = false;
    private void Start()
    {
        _meteorLayer = gameObject.layer;
        FindObjectOfType<SpawnManager>().OnSpawnEnemy += OnOnSpawnEnemy;
    }

    private void OnOnSpawnEnemy(GameObject enemyGo)
    {
        if (_collisionIgnoredSetted) return;
        if (enemyGo.TryGetComponent<NPC_Controller>(out var npc))
        {
            _npcLayer = enemyGo.layer;
            Physics2D.IgnoreLayerCollision(_meteorLayer,_npcLayer,true);
            Physics2D.IgnoreLayerCollision(_npcLayer,_npcLayer,true);
        }
    }
}
