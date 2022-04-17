using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private ScoreUI _scoreUI;
    [SerializeField] private GoldUI _goldUI;
    
    private int _score;
    private int _gold;

    private void Awake()
    {
        UpdateScoreUISafe();
        UpdateGoldUISafe();
    }

    public void AddScore(int amount)
    {
        _score += amount;
        UpdateScoreUISafe();
    }

    private void UpdateScoreUISafe()
    {
        if (_scoreUI)
        {
            _scoreUI.UpdateScoreAmount(_score);
        }
    }

    public void AddGold(int amount)
    {
        _gold += amount;
        UpdateGoldUISafe();
    }

    private void UpdateGoldUISafe()
    {
        if (_goldUI)
        {
            _goldUI.UpdateGoldAmount(_gold);
        }
    }
}