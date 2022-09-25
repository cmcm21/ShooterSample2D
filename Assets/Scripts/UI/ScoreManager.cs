using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;

[RequireComponent(typeof(TextMeshProUGUI))]
public class ScoreManager : MonoBehaviour
{
    private TextMeshProUGUI _text;
    private int _points = 0;
    private void Start()
    {
        _text = GetComponent<TextMeshProUGUI>();
        UpdateScore();
    }

    public void UpdateScore(int points = 0)
    {
        _points += points;
        _text.text = _points.ToString().PadLeft(5,'0');
    }
}
