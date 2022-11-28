using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using MyBox;

public class SkillTreeController : MonoBehaviour
{
    public List<SkillNode> goodSkills = new List<SkillNode>();
    public List<SkillNode> badSkills = new List<SkillNode>();
    private EventsController events;

    [SerializeField] private RectTransform gHolder;
    [SerializeField] private RectTransform bHolder;
    [SerializeField] private Button resetButton;

    void Start()
    {
        events = DataManager.Events;
        events.OnCoinsSpend.AddListener(UpdateTree);

        resetButton.onClick.AddListener(ResetAllSkillValues);

        foreach (Transform child in gHolder.transform)
        {
            var newSkill = child.GetComponent<SkillNode>();
            child.name = newSkill.name;
            goodSkills.Add(newSkill);
        }        
        
        foreach (Transform child in bHolder.transform)
        {
            var newSkill = child.GetComponent<SkillNode>();
            child.name = newSkill.name;
            badSkills.Add(newSkill);
        }

        UpdateTree();
    }

    public void UpdateTree()
    {
        bool hasSkills = !events.GameplayData.PlayerHasNoSkills;
        
        resetButton.gameObject.SetActive(hasSkills);

        if(hasSkills)
        {
            if(events.GameplayData.PlayerIsImpostor) foreach (SkillNode skill in goodSkills) skill.DisableNode();
            else foreach (SkillNode skill in badSkills) skill.DisableNode();
        }
    }

    [ButtonMethod]
    private void ResetAllSkillValues()
    {
        int coinsToReturn = 0;
        events.GameplayData.ResetSkillTree();

        foreach (SkillNode node in goodSkills)
        {
            if (node.CurrentLevel > 0)
            {
                coinsToReturn += node.TotalAmountSpent;
                node.ResetSkillValues();
                node.UpdateNode();
            }
            else continue;
        }
        foreach (SkillNode node in badSkills)
        {
            if (node.CurrentLevel > 0)
            {
                coinsToReturn += node.TotalAmountSpent;
                node.ResetSkillValues();
                node.UpdateNode();
            }
            else continue;
        }
        
        events.GameplayData.AddCoinsToTotal(coinsToReturn);
        events.OnCoinsSpend.Invoke();
        UpdateTree();
    }

    private void OnDestroy()
    {
        events.OnCoinsSpend.RemoveListener(UpdateTree);
        resetButton.onClick.RemoveListener(ResetAllSkillValues);
    }
}
