using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;
using System.Linq;

[CreateAssetMenu(fileName = "PlayerGameData", menuName = "Data/ New gameplay data", order = 0)]
public class PlayerGameplayData : ScriptableObject
{
    public int TotalCoins => totalCoins;
    public int currentReservedCoins;
    public int BestDistance;
    
    [Space(10f)]
    [Header("Debug options")]
    public int coinsToAdd;
    
    private int totalCoins;
    private Dictionary<int, float> passiveSkills = new Dictionary<int, float>( capacity: 6 );
    
    public bool PlayerIsImpostor => passiveSkills.Keys.ToList().TrueForAll(k => k > 2);
    public bool PlayerHasNoSkills => passiveSkills.IsNullOrEmpty();

    public struct CharacterSheet
    {
        public float maxSpeed;
        public float maxAcceleration;   
        public float redCoinChance;
        public float magForce;
        public float shieldCharges;
        public float reviveCharges;
    }

    public void UpdateSkillDictionary(PassiveSkill skill) 
    {
        if (passiveSkills.ContainsKey(skill.id)) passiveSkills[skill.id] = skill.increaseAmount;
        else passiveSkills.Add(skill.id, skill.increaseAmount);
    }
    public void ClearSkills() => passiveSkills.Clear();
    public void DebugDictionary()
    {
        foreach (var item in passiveSkills)
        {   
            Debug.Log(item.Key + " " + item.Value);
        }
    }

    public void SetupPassiveSkills()
    {
        CharacterSheet sheet = new CharacterSheet();

        sheet.maxSpeed = passiveSkills[0];
        sheet.maxAcceleration = passiveSkills[1];
        sheet.redCoinChance = passiveSkills[2];
        sheet.magForce = passiveSkills[3];
        sheet.shieldCharges = passiveSkills[4];
        sheet.reviveCharges = passiveSkills[5];
    }

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

    public void SpendCoins(int desiredValue)
    {
        if(desiredValue > totalCoins) 
        {
            Debug.LogError("INSSUFICIENT FUNDS");
            return;
        }

        totalCoins -= desiredValue;
        DataManager.Events.OnCoinsSpend.Invoke();
    }    
    
    [ButtonMethod]
    protected void AddCoins()
    {
        totalCoins += coinsToAdd;
        coinsToAdd = 0;
    }
    [ButtonMethod]
    protected void ResetSkillTree()
    {
        ClearSkills();
    }
}
