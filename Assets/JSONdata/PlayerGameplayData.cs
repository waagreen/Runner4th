using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "PlayerGameData", menuName = "UntitledEndlessRunner/PlayerGameData", order = 0)]
public class PlayerGameplayData : ScriptableObject
{
    public int TotalCoins => totalCoins;
    public int currentReservedCoins;
    private int totalCoins;

    public int BestDistance;

    public void ResetAndSaveReservedCoins()
    {
        totalCoins += currentReservedCoins;
        currentReservedCoins = 0;
    }

    public void SyncTotalCoins(int coinValue)
    {
        if(coinValue < totalCoins) return;
        totalCoins = coinValue;
    }
}
