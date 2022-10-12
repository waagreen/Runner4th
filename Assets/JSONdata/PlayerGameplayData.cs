using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "PlayerGameData", menuName = "Data/ New gameplay data", order = 0)]
public class PlayerGameplayData : ScriptableObject
{
    public int TotalCoins => totalCoins;
    public int currentReservedCoins;
    
    private int totalCoins;

    public int BestDistance;

    public Dictionary<string, int> passiveSkills = new Dictionary<string, int>();

    public struct CharacterSheet
    {
        public float maxSpeed;
        public float maxAcceleration;
        public float redCoinChance;
        public float magForce;
        public float shieldCharges;
        public float reviveCharges;
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
}
