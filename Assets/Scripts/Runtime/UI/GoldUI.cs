using System;
using TMPro;
using UnityEngine;

public class GoldUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _goldAmountLabel;
    [SerializeField] private string _goldAmountFormat = "Gold: {0}";

    public void UpdateGoldAmount(int amount)
    {
        _goldAmountLabel.text = String.Format(_goldAmountFormat, amount);
    }
}