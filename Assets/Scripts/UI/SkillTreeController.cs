using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using MyBox;
using System.Linq;

public class SkillTreeController : MonoBehaviour
{
    public List<SkillNode> goodSkills = new List<SkillNode>();
    public List<SkillNode> badSkills = new List<SkillNode>();
    private EventsController events;
    private bool playerIsImpostor => events.GameplayData.PlayerIsImpostor;
    private bool hasNoSkills => events.GameplayData.PlayerHasNoSkills;

    [SerializeField] private RectTransform gHolder;
    [SerializeField] private RectTransform bHolder;
    [SerializeField] private Button resetButton;

    void Start()
    {
        events = DataManager.Events;
        
        events.OnCoinsSpend.AddListener(UpdateTree);
        resetButton.onClick.AddListener(ResetAllSkillValues);

        UpdateTree();
    }

    public void UpdateTree()
    {
        resetButton.gameObject.SetActive(!hasNoSkills);
        
        if (hasNoSkills) LockOppositeTree();
        else DisableOppositeSide();

        if(hasNoSkills && DataManager.Events.GameplayData.FirstSkill)
        {
            Debug.Log("GOT ON MULT");
            foreach (SkillNode skill in goodSkills)
            {
                SkillNode previousSkill = null;
                int currentIndex = goodSkills.IndexOf(skill);
                if(currentIndex > 0) previousSkill = goodSkills[currentIndex-1];

                if(previousSkill != null && previousSkill.CurrentLevel <= 2)
                {  
                    skill.DisableNode();
                }
            }

            foreach (SkillNode skill in badSkills)
            {
                SkillNode previousSkill = null;
                int currentIndex = badSkills.IndexOf(skill);
                if(currentIndex > 0) previousSkill = badSkills[currentIndex-1];

                if(previousSkill != null && previousSkill.CurrentLevel <= 2)
                {
                    skill.DisableNode();
                }
            }
        }
    }

    [ButtonMethod]
    private void ResetAllSkillValues()
    {
        int coinsToReturn = 0;
        events.GameplayData.ResetSkillTree();
        
        gHolder.gameObject.SetActive(true);
        bHolder.gameObject.SetActive(true);

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

    private void LockOppositeTree()
    {
        if(playerIsImpostor) foreach (SkillNode skill in goodSkills)
        {
            int currentIndex = goodSkills.IndexOf(skill);
            if (currentIndex == 0) continue;
            skill.DisableNode();
        }
        else foreach (SkillNode skill in badSkills)
        {
            int currentIndex = badSkills.IndexOf(skill);
            if (currentIndex == 0) continue;
            skill.DisableNode();
        }
    }

    private void DisableOppositeSide()
    {
        if(playerIsImpostor)
        {
            gHolder.gameObject.SetActive(false);
            bHolder.gameObject.SetActive(true);
        }
        else
        {
            bHolder.gameObject.SetActive(false);
            gHolder.gameObject.SetActive(true);
        }
    }

    private void OnDestroy()
    {
        events.OnCoinsSpend.RemoveListener(UpdateTree);
        resetButton.onClick.RemoveListener(ResetAllSkillValues);
    }
}
