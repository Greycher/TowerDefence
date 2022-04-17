using System;
using TMPro;
using UnityEngine;

public class BestScoreUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _bestScoreLabel;
    [SerializeField] private string _bestScoreFormat = "Best Score: {0}";

    public void UpdateBestScore(int bestScore)
    {
        _bestScoreLabel.text = String.Format(_bestScoreFormat, bestScore);
    }
}