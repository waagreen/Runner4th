using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class SaveData
{
    public int myCoins;
    public int bestDistance;
    public int amountSpent;
    public List<PassiveSkill> currentPassiveSkills = new List<PassiveSkill>(capacity: 6);

    public string ToJson() => JsonUtility.ToJson(this);

    public void LoadFromJson(string a_Json) => JsonUtility.FromJsonOverwrite(a_Json, this);
}
[System.Serializable]
public struct PassiveSkill
{
    public int id;
    public float increaseAmount;
}

public interface ISaveble
{
    void PopulateSaveData(SaveData a_SaveData);
    void LoadFromSaveData(SaveData a_SaveData);
}