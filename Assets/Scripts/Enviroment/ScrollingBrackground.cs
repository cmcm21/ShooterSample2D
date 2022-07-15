using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class ScrollingBrackground : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 0.5f;
    private Renderer _renderer;
    private void Start()
    {
        _renderer = GetComponent<Renderer>();
    }

    private void LateUpdate()
    {
        if(!_renderer) return;
        _renderer.material.mainTextureOffset += new Vector2(0, moveSpeed * Time.deltaTime);
    }
}
