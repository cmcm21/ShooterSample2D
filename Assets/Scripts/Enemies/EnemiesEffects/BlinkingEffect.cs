using System;
using System.Collections.Generic;
using UnityEngine;

public class BlinkingEffect
{
    private bool isBlinking;
    private float timer;
    private float time;
    private Color blinkingColor;
    private Color previousColor;
    private SpriteRenderer spriteRenderer;

    public Action OnAnimationFinished;
     
    public BlinkingEffect(BlinkingData blinkingData)
    {
        blinkingColor = blinkingData.blinkingColor;
        time = blinkingData.time;
    }

    public void GetHit(ref SpriteRenderer spriteRenderer)
    {
        if (isBlinking) return;
        
        this.spriteRenderer = spriteRenderer;
        previousColor = spriteRenderer.color;
        spriteRenderer.color = blinkingColor;
        
        isBlinking = true;
    }

    public void Update(float deltaTime)
    {
        if (!isBlinking) return;
        timer += deltaTime;
        spriteRenderer.color = spriteRenderer.color == blinkingColor ? previousColor : blinkingColor;
        if (timer >= time)
        {
            isBlinking = false;
            timer = 0;
            spriteRenderer.color = previousColor;
            OnAnimationFinished?.Invoke();
        }
    }

    public void Stop()
    {
        isBlinking = false;
        spriteRenderer.color = previousColor;
        timer = 0;
    }
}

[System.Serializable]
public struct BlinkingData
{
    public Color blinkingColor;
    public float time;
}
