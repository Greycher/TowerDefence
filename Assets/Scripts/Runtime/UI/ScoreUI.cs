using System;
using TMPro;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scoreAmountLabel;
    [SerializeField] private string _scoreAmountFormat = "Score: {0}";

    public void UpdateScoreAmount(int amount)
    {
        _scoreAmountLabel.text = String.Format(_scoreAmountFormat, amount);
    }
}