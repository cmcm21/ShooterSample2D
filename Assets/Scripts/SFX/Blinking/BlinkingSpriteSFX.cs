using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BlinkingSpriteSFX : MonoBehaviour
{
    [SerializeField] private BlinkingData blinkingData;
    [SerializeField] private SpriteRenderer sprite;

    public event Action OnAnimationFinished = delegate {}; 

    private BlinkingEffect _blinkingEffect;

    private void Awake()
    {
        _blinkingEffect = new BlinkingEffect(blinkingData,ref sprite);
        _blinkingEffect.OnAnimationFinished += RiseEvent;
    }

    private void OnDestroy()
    {
        _blinkingEffect.OnAnimationFinished -= RiseEvent;
    }

    private void RiseEvent()
    {
       Debug.Log("Rising event animation finished");
       OnAnimationFinished?.Invoke();
    }

    private void Update()
    {
        _blinkingEffect?.Update(Time.deltaTime);
    }

    public void Blink()
    {
        _blinkingEffect.Blink();
    }

    public void Stop()
    {
        _blinkingEffect.Stop();
    }
}
