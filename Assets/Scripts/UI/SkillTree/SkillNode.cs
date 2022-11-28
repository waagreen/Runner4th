using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEditor;

[RequireComponent(typeof(Button))]
public class SkillNode : MonoBehaviour
{
    private EventsController events;
    int currentCoins => events.GameplayData.TotalCoins;

    [SerializeField] private Skill skill;
    [SerializeField] private Button nodeBt;

    public int CurrentLevel => skill.currentLevel;
    public int BaseCost => skill.BaseCost;
    public int TotalAmountSpent => skill.TotalAmountSpent;

    [Header("Visual components")]
    public TMP_Text title;
    public TMP_Text description;
    public TMP_Text cost;
    public TMP_Text level;
    public Image bg;
    public Image icon;
    public Image frame;
    public int id => skill.id; 

    [Header("Node Colors & Sprites")]
    public Color32 impostorColor;
    public Color32 scientistColor;
    public Sprite impostorIcon;
    public Sprite scientistIcon;

    private void Start() 
    {
        events = DataManager.Events;

        SetupNode();
        nodeBt.onClick.AddListener(Buy);
        events.OnCoinsSpend.AddListener(UpdateNode);
    }

    public void SetupNode()
    {
        if(skill == null)
        {
            Debug.LogError("The node needs a skill to function");
            return;
        }

        UpdateNode();
    }

    public void UpdateNode()
    {
        bool isScientist = id < 3 ;
        frame.sprite = isScientist ? scientistIcon : impostorIcon;
        
        if (currentCoins < skill.currentCost) DisableNode();
        else if (skill.currentLevel >= skill.MaxLevel) DisableNode();
        else
        {
            nodeBt.interactable = true;
            Color32 nodeColor = isScientist ? scientistColor : impostorColor;
            bg.color = nodeColor;
            frame.color = nodeColor;
        }

        title.SetText(skill.title);
        description.SetText(skill.description);
        
        level.SetText($"Level: {skill.currentLevel} / {skill.MaxLevel}");
        cost.SetText($"Cost: {skill.currentCost}");
        
        icon.sprite = skill.icon;
    }

    private void Buy()
    {
        if(currentCoins >= skill.currentCost)
        {
            PassiveSkill passiveSkill = new PassiveSkill();
            passiveSkill.id = skill.id;
            passiveSkill.increaseAmount = skill.increaseAmount;

            events.OnSkillBuy.Invoke(passiveSkill);
            events.GameplayData.SpendCoinsFromTotal(skill.currentCost);
            
            skill.TotalAmountSpent += skill.currentLevel > 0 ? skill.currentCost : skill.BaseCost;
            skill.currentLevel++;
            skill.currentCost = BaseCost * (CurrentLevel + 1);
            EditorUtility.SetDirty(skill);
            UpdateNode();
        }
        else Debug.Log("NOT ENOUGH COINS!");
    }

    public void DisableNode()
    {
        Debug.Log(skill.name + " was disabled");
        nodeBt.interactable = false;
        bg.color = Color.grey;
        frame.color = Color.grey;
    }

    public void ResetSkillValues() => skill.ResetValues();

    private void OnDestroy() {
        nodeBt.onClick.RemoveListener(Buy);
    }
}
