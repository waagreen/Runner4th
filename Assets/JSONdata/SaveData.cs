using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    [System.Serializable]
    public struct PlayerWallet
    {
        public int myCoins;
        public float bestDistance;
    }

    public string ToJson() => JsonUtility.ToJson(this);

    public void LoadFromJson(string a_Json) => JsonUtility.FromJsonOverwrite(a_Json, this);
}

public interface ISaveble
{
    void PopulateSaveData(SaveData a_SaveData);
    void LoadFromSaveData(SaveData a_SaveData);
}