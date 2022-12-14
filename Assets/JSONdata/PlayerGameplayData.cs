using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;
using System.Linq;

public struct CharacterSheet
{
    public float maxSpeed;
    public float maxAcceleration;   
    public float redCoinChance;
    public float magForce;
    public float shieldCharges;
    public float reviveCharges;
}

[CreateAssetMenu(fileName = "PlayerGameData", menuName = "Data/ New gameplay data", order = 0)]
public class PlayerGameplayData : ScriptableObject
{
    public int TotalCoins => totalCoins;
    public int TotalAmountSpent => totalAmountSpent;
    public int currentReservedCoins;
    public int BestDistance;
    
    
    [Space(10f)]
    [Header("Debug options")]
    public int coinsToAdd;
    
    private int totalAmountSpent;
    private int totalCoins;
    private List<PassiveSkill> passiveSkills = new List<PassiveSkill>(capacity: 6);
    public List<PassiveSkill> PassiveSkills => passiveSkills;
    
    public bool PlayerIsImpostor => passiveSkills.TrueForAll(k => k.id > 2);
    public bool PlayerHasNoSkills => passiveSkills.Count == 0;
    public bool FirstSkill => passiveSkills.Count == 1;

    public void ClearSkills() => passiveSkills.Clear();
    public void SyncPassiveSkills(List<PassiveSkill> skillsToSync)
    {
        foreach (var skill in skillsToSync)
        {
            UpdateSkillList(skill);
        }
    }
    public void UpdateSkillList(PassiveSkill skill) 
    {
        if (passiveSkills.Contains(skill)) 
        {
           var skillToModify = passiveSkills.Find(s => s.id == skill.id);
           skillToModify.increaseAmount += skill.increaseAmount;
        }
        else passiveSkills.Add(skill);
    }
    public CharacterSheet GetCharacterSheet()
    {
        // magnet - 0 | maxAccel - 1 | revive - 2 | redCoins - 3 | maxSpeed - 4 | Shield - 5

        CharacterSheet newSheet = new CharacterSheet();

        newSheet.magForce = passiveSkills.Find(p => p.id == 0).increaseAmount; 
        newSheet.maxAcceleration = passiveSkills.Find(p => p.id == 1).increaseAmount;
        newSheet.reviveCharges = passiveSkills.Find(p => p.id == 2).increaseAmount;
        newSheet.redCoinChance = passiveSkills.Find(p => p.id == 3).increaseAmount;
        newSheet.maxSpeed = passiveSkills.Find(p => p.id == 4).increaseAmount;
        newSheet.shieldCharges = passiveSkills.Find(p => p.id == 5).increaseAmount;

        return newSheet;
    }

    public void ResetAndSaveReservedCoins()
    {
        totalCoins += currentReservedCoins;
        currentReservedCoins = 0;
    }

    public void SyncTotalCoins(int coinValue, bool canSubtract = false)
    {
        if(coinValue < totalCoins && !canSubtract) return;
        totalCoins = coinValue;
    }
    public void AddCoinsToTotal(int desiredValue)
    {
        totalCoins += desiredValue;
    }
    public void SpendCoinsFromTotal(int desiredValue)
    {
        if(desiredValue > totalCoins) 
        {
            Debug.LogError("INSSUFICIENT FUNDS");
            return;
        }

        totalCoins -= desiredValue;
        DataManager.Events.OnCoinsSpend.Invoke();
    }    
    public void RegisterAmountSpent(int amount) => totalAmountSpent += amount;
    public void SyncTotalSpent(int amount) => totalAmountSpent = amount;

    [ButtonMethod]
    protected void AddCoins()
    {
        totalCoins += coinsToAdd;
        coinsToAdd = 0;
    }
    [ButtonMethod]
    private void ResetTotalCoins()
    {
        totalCoins = 0;
    }
    [ButtonMethod]
    public void ResetSkillTree()
    {
        ClearSkills(); // clear the local skills list

        // gets the json, converts back to save data, clear the skills list and save back as json 
        if(FileManager.LoadFromFile("SaveData.dat", out var json))
        {
            SaveData _tempLoadListData = JsonUtility.FromJson<SaveData>(json);
            _tempLoadListData.currentPassiveSkills = new List<PassiveSkill>(capacity: 6);
            string jsonToSave = JsonUtility.ToJson(_tempLoadListData);
            FileManager.WriteToFile("SaveData.dat", jsonToSave);
        };
    }
}
