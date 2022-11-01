using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class WalletDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text coins;

    private void Start() {
        UpdateValues();
        DataManager.Events.OnCoinsSpend.AddListener(UpdateValues);
    }

    void UpdateValues()
    {
        int coinValue = DataManager.Events.GameplayData.TotalCoins;

        coins.SetText(coinValue.ToString());
    }
}
