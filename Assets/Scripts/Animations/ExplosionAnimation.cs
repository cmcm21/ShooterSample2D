using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void OnExplosionFinished();

[RequireComponent(typeof(Animator))]
public class ExplosionAnimation : MonoBehaviour
{
    private Animator _animator;
    private static readonly int AnimationFinished = Animator.StringToHash("AnimationFinished");
    private static readonly int AnimationStart = Animator.StringToHash("AnimationStart");
    public event OnExplosionFinished onExplosionFinished;

    private void Start()
    {
        GetAnimator();
    }

    private void GetAnimator()
    {
        if(_animator == null)
            _animator = GetComponent<Animator>();
    }

    public void OnEnable()
    {
        gameObject.SetActive(true);
        GetAnimator();
        _animator.SetTrigger(AnimationStart);
    }

    public void OnExplosionFinished()
    {
        _animator.SetTrigger(AnimationFinished);
        onExplosionFinished?.Invoke();
        gameObject.SetActive(false);
    }
}
