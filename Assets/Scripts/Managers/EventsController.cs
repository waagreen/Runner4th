using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.Universal;
using System.Linq;

public class EventsController : MonoBehaviour, ISaveble
{
    [SerializeField] private UniversalRendererData rendererData;
    [SerializeField] private PlayerGameplayData gameplayData;

    [HideInInspector] public UnityEvent OnPlayerDeath;
    [HideInInspector] public UnityEvent<int> OnCollectCoin;
    [HideInInspector] public UnityEvent<bool> OnPauseGame;
    [HideInInspector] public UnityEvent<PassiveSkill> OnSkillBuy;
    [HideInInspector] public UnityEvent OnUpdateSkillTree;
    [HideInInspector] public UnityEvent OnCoinsSpend;
    [HideInInspector] public UnityEvent OnShieldHit;

    public PlayerGameplayData GameplayData => gameplayData; 
    private ScriptableRendererFeature blitFeature;
    public CharacterSheet passiveSkills;
    private bool pauseWasPressed = false;

    private void Awake() 
    {
        blitFeature = rendererData.rendererFeatures[0];
        blitFeature.SetActive(DataManager.isGameplay);

        OnPlayerDeath = new UnityEvent();
        OnCollectCoin = new UnityEvent<int>();
        OnPauseGame = new UnityEvent<bool>();
        OnSkillBuy = new UnityEvent<PassiveSkill>();
        OnCoinsSpend = new UnityEvent();
        OnUpdateSkillTree = new UnityEvent();
        OnShieldHit = new UnityEvent();

        LoadJsonData(this);
    
        OnSkillBuy.AddListener(UpdateSkill);
        OnCollectCoin.AddListener(AddCoinOnData);
        OnPauseGame.AddListener(FreezeTime);

        passiveSkills = gameplayData.GetCharacterSheet();
    }

    private void UpdateSkill(PassiveSkill skill) => gameplayData.UpdateSkillList(skill);
    public void AddCoinOnData(int coinsToAdd) => gameplayData.currentReservedCoins += coinsToAdd;
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
        if (gameplayData.PassiveSkills != null && gameplayData.PassiveSkills.Count > 0)
        {
            foreach (var skill in gameplayData.PassiveSkills)
            {        
                if (a_SaveData.currentPassiveSkills.Contains(skill)) 
                {
                    var skillToModify = a_SaveData.currentPassiveSkills.Find(s => s.id == skill.id);
                    skillToModify.increaseAmount += skill.increaseAmount;
                }
                else a_SaveData.currentPassiveSkills.Add(skill);
            }
        }

        gameplayData.ResetAndSaveReservedCoins();
        a_SaveData.myCoins = gameplayData.TotalCoins;

        float distance = DataManager.GlobalMovement.distance;

        if (distance > gameplayData.BestDistance)
        { 
            gameplayData.BestDistance = Mathf.RoundToInt(distance);
            a_SaveData.bestDistance = gameplayData.BestDistance;
        }
        else a_SaveData.bestDistance = gameplayData.BestDistance;
    }
    
    public void LoadFromSaveData(SaveData a_SaveData)
    {
        gameplayData.SyncPassiveSkills(a_SaveData.currentPassiveSkills);
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
        OnUpdateSkillTree.RemoveAllListeners();
        OnShieldHit.RemoveAllListeners();
    }
}
