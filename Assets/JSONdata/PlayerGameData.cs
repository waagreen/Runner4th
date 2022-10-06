using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "PlayerGameData", menuName = "UntitledEndlessRunner/PlayerGameData", order = 0)]

public class PlayerGameData : ScriptableObject, ISaveble
{
    public int currentReservedCoins;
    public int totalCoins;

    public float currentBestDistance;

    public void LoadFromSaveData(SaveData a_SaveData)
    {
        throw new System.NotImplementedException();
    }

    public void PopulateSaveData(SaveData a_SaveData)
    {
        SaveData.PlayerWallet newPlayerData = new SaveData.PlayerWallet();

        newPlayerData.myCoins = totalCoins;
        newPlayerData.bestDistance = currentBestDistance;
    }
}
