using System;
using TMPro;
using UnityEngine;

public class HealthUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _healthLabel;
    [SerializeField] private string _healthFormat = "Health: {0}";

    public void UpdateHealth(int health)
    {
        _healthLabel.text = String.Format(_healthFormat, health);
    }
}