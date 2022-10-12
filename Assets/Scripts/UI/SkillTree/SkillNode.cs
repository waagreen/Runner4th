using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class SkillNode : MonoBehaviour
{
    private EventsController events => DataManager.Events;
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
    public int id => skill.id; 

    [Header("Node Colors")]
    public Color32 impostorColor;
    public Color32 scientistColor;

    private void Start() {
        SetupNode();
        nodeBt.onClick.AddListener(Buy);
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
        if(currentCoins < skill.cost)
        {
            nodeBt.interactable = false;
            bg.color = Color.grey;
        }
        else
        {
            nodeBt.interactable = true;
            bg.color = id < 3 ? scientistColor : impostorColor;
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
            events.OnSkillBuy.Invoke(skill.increaseAmount);
            events.GameplayData.SpendCoins(skill.cost);
            
            skill.currentLevel++;
            skill.cost *= 2;
            UpdateNode();
        }
        else Debug.Log("NOT ENOUGH COINS!");
    }

    private void OnDestroy() {
        nodeBt.onClick.RemoveListener(Buy);
    }
}
