using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventsController : MonoBehaviour, ISaveble
{
    [SerializeField] private PlayerGameplayData gameplayData;

    [HideInInspector] public UnityEvent OnPlayerDeath = new UnityEvent();
    [HideInInspector] public UnityEvent<int> OnCollectCoin = new UnityEvent<int>();

    public PlayerGameplayData GameplayData => gameplayData; 


    private void Awake() 
    {
        LoadJsonData(this);
        OnCollectCoin.AddListener(AddCoinOnData);
        Debug.Log("Total Coins: " + gameplayData.TotalCoins);
    }

    private void AddCoinOnData(int coinsToAdd) => gameplayData.currentReservedCoins += coinsToAdd;

    private static void SaveJsonData(EventsController eController)
    {
        SaveData sd = new SaveData();
        eController.PopulateSaveData(sd);

        if(FileManager.WriteToFile("SaveData.dat", sd.ToJson()))
        {
            Debug.Log("Save Succesful");
        }
    }

    private static void LoadJsonData(EventsController eController)
    {
        if(FileManager.LoadFromFile("SaveData.dat", out var json))
        {
            SaveData sd = new SaveData();
            sd.LoadFromJson(json);

            eController.LoadFromSaveData(sd);
            Debug.Log("Load Succesful");
        }
    }

    public void PopulateSaveData(SaveData a_SaveData)
    {
        gameplayData.ResetAndSaveReservedCoins();
        a_SaveData.myCoins = gameplayData.TotalCoins;

        float distance = DataManager.GlobalMovement.distance;

        if(distance > gameplayData.BestDistance)
        { 
            gameplayData.BestDistance = Mathf.RoundToInt(distance);
            a_SaveData.bestDistance = gameplayData.BestDistance;
        }
    }
    
    public void LoadFromSaveData(SaveData a_SaveData)
    {
        gameplayData.SyncTotalCoins(a_SaveData.myCoins);
        gameplayData.BestDistance = a_SaveData.bestDistance;
    }

    private void OnDestroy() 
    {
        SaveJsonData(this);
        OnCollectCoin.RemoveListener(AddCoinOnData);
    }
}
