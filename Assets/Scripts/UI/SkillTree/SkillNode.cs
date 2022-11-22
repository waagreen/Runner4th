using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class SkillNode : MonoBehaviour
{
    private EventsController events;
    int currentCoins => events.GameplayData.TotalCoins;

    [SerializeField] private Skill skill;
    [SerializeField] private Button nodeBt;

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

    private void UpdateNode()
    {
        bool isScientist = id < 3 ;
        frame.sprite = isScientist ? scientistIcon : impostorIcon;
        
        if(currentCoins < skill.cost) DisableNode();
        else
        {
            nodeBt.interactable = true;

            bg.color = isScientist ? scientistColor : impostorColor;
            frame.color = Color.white;
        }

        title.SetText(skill.title);
        description.SetText(skill.description);
        
        level.SetText($"Level:  {skill.currentLevel} / {skill.mxLevel}");
        cost.SetText($"Cost: {skill.cost}");
        
        icon.sprite = skill.icon;
    }

    private void Buy()
    {
        if(currentCoins >= skill.cost)
        {
            PassiveSkill passiveSkill = new PassiveSkill();
            passiveSkill.id = skill.id;
            passiveSkill.increaseAmount = skill.increaseAmount;

            events.OnSkillBuy.Invoke(passiveSkill);
            events.GameplayData.SpendCoins(skill.cost);
            
            skill.currentLevel++;
            skill.cost *= 2;
            UpdateNode();
        }
        else Debug.Log("NOT ENOUGH COINS!");
    }

    public void DisableNode()
    {
        nodeBt.interactable = false;
        bg.color = Color.grey;
        frame.color = Color.grey;
    }

    private void OnDestroy() {
        nodeBt.onClick.RemoveListener(Buy);
    }
}
