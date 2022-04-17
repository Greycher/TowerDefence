using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utility;

public class GameManager : MonoBehaviour
{
    [SerializeField] private LevelManager _levelManager;
    [SerializeField] private ScoreUI _scoreUI;
    [SerializeField] private BestScoreUI _bestScoreUI;
    [SerializeField] private GoldUI _goldUI;
    [SerializeField] private int _startGoldAmount = 50;

    private int _score;
    private int _gold;
    private int _totalEnemyCount;

    private void Awake()
    {
        _gold = _startGoldAmount;
        _totalEnemyCount = _levelManager.GetTotalEnemyCount();
        UpdateScoreUISafe();
        UpdateBestScoreUISafe(PlayerPrefs.GetInt(GlobalConst.BestScoreKey));
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
    
    private void UpdateBestScoreUISafe(int bestScore)
    {
        if (_bestScoreUI)
        {
            _bestScoreUI.UpdateBestScore(bestScore);
        }
    }

    public void AddGold(int amount)
    {
        _gold += amount;
        UpdateGoldUISafe();
    }
    
    public void RemoveGold(int amount)
    {
        _gold -= amount;
        UpdateGoldUISafe();
    }

    private void UpdateGoldUISafe()
    {
        if (_goldUI)
        {
            _goldUI.UpdateGoldAmount(_gold);
        }
    }

    public bool HasSuffiecentGold(int coinCostAmount)
    {
        return coinCostAmount <= _gold;
    }

    public void FailLevel()
    {
        FinishLevel(false);
    }

    private void FinishLevel(bool success)
    {
        var savedBestScore = PlayerPrefs.GetInt(GlobalConst.BestScoreKey, 0);
        if (_score > savedBestScore)
        {
            PlayerPrefs.SetInt(GlobalConst.BestScoreKey, _score);
            UpdateBestScoreUISafe(_score);
        }
        
        if (success)
        {
            SceneManager.LoadScene(1);
        }
        else
        {
            SceneManager.LoadScene(2);
        }
    }

    public void NotifyEnemyDead()
    {
        if (--_totalEnemyCount == 0)
        {
            FinishLevel(true);
        }
    }
}