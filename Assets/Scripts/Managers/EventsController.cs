using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventsController : MonoBehaviour, ISaveble
{
    [SerializeField] private PlayerGameplayData gameplayData;

    [HideInInspector] public UnityEvent OnPlayerDeath;
    [HideInInspector] public UnityEvent<int> OnCollectCoin;
    [HideInInspector] public UnityEvent<bool> OnPauseGame;
    [HideInInspector] public UnityEvent<float> OnSkillBuy;
    [HideInInspector] public UnityEvent OnCoinsSpend;

    public PlayerGameplayData GameplayData => gameplayData; 
    private bool pauseWasPressed = false;

    private void Awake() 
    {
        OnPlayerDeath = new UnityEvent();
        OnCollectCoin = new UnityEvent<int>();
        OnPauseGame = new UnityEvent<bool>();
        OnSkillBuy = new UnityEvent<float>();
        OnCoinsSpend = new UnityEvent();

        LoadJsonData(this);
    
        OnCollectCoin.AddListener(AddCoinOnData);
        OnPauseGame.AddListener(FreezeTime);
    }

    private void AddCoinOnData(int coinsToAdd) => gameplayData.currentReservedCoins += coinsToAdd;

    private void FreezeTime(bool shouldFreeze) => Time.timeScale = shouldFreeze ? 0f : 1f;

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
        else a_SaveData.bestDistance = gameplayData.BestDistance;
    }
    
    public void LoadFromSaveData(SaveData a_SaveData)
    {
        gameplayData.SyncTotalCoins(a_SaveData.myCoins);
        gameplayData.BestDistance = a_SaveData.bestDistance;
    }

    public void HandlePause()
    {
        pauseWasPressed = pauseWasPressed ? false : true ;
        DataManager.Events.OnPauseGame.Invoke(pauseWasPressed);
    }
    
    private void OnDestroy() 
    {
        SaveJsonData(this);

        OnCollectCoin.RemoveAllListeners();
        OnPauseGame.RemoveAllListeners();
        OnPlayerDeath.RemoveAllListeners();
        OnSkillBuy.RemoveAllListeners();
        OnCoinsSpend.RemoveAllListeners();
    }
}
