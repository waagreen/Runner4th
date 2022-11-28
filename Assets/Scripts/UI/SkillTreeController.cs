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
    [SerializeField] private Material tilingMat;
    
    private Color kRed = new Color(229, 68, 71);
    private Color kBlue = new Color(68, 207, 229);

    void Start()
    {
        events = DataManager.Events;
        
        events.OnCoinsSpend.AddListener(UpdateTree);
        resetButton.onClick.AddListener(ResetAllSkillValues);

        foreach (Transform child in gHolder.transform)
        {
            var newSkill = child.GetComponent<SkillNode>();
            if (newSkill != null) goodSkills.Add(newSkill);
        }        
        
        foreach (Transform child in bHolder.transform)
        {
            var newSkill = child.GetComponent<SkillNode>();
            if (newSkill != null) badSkills.Add(newSkill);
        }

        UpdateTree();
    }

    public void UpdateTree()
    {
        Color bgColor = playerIsImpostor ? kRed : kBlue;
        // tilingMat.SetColor("_BaseColor", bgColor);

        Debug.Log("no skills: " + hasNoSkills);
        resetButton.gameObject.SetActive(!hasNoSkills);
        
        if (hasNoSkills) LockOppositeTree();
        else DisableOppositeSide();

        foreach (SkillNode skill in goodSkills)
        {
            SkillNode previousSkill = null;
            int currentIndex = goodSkills.IndexOf(skill);
            previousSkill = currentIndex == 0 ? goodSkills[currentIndex] : goodSkills[currentIndex-1];
            
            skill.SetupNode();

            if(currentIndex == 0 || previousSkill.CurrentLevel > skill.CurrentLevel / 2)
            {
                skill.EnableNode();
                Debug.Log("Disabled: " + skill.name);
            }
            else
            {   
                skill.DisableNode();
                Debug.Log("Disabled: " + skill.name);
            }
        }
        
        foreach (SkillNode skill in badSkills)
        {
            SkillNode previousSkill = null;
            int currentIndex = badSkills.IndexOf(skill);
            previousSkill = currentIndex == 0 ? badSkills[currentIndex] : badSkills[currentIndex-1];
            
            skill.SetupNode();

            if(currentIndex == 0 || previousSkill.CurrentLevel > skill.CurrentLevel / 2)
            { 
                skill.EnableNode();
            }
            else
            {
                skill.DisableNode();
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
