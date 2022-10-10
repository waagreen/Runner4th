using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class WalletDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text coins;
    [SerializeField] private TMP_Text bestDistance;

    void Start()
    {
        int coinValue = DataManager.Events.GameplayData.TotalCoins;
        int dist = DataManager.Events.GameplayData.BestDistance;

        coins.SetText(coinValue.ToString());
        bestDistance.SetText($"{dist} m");
    }
}
