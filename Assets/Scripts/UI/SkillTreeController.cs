using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class SkillTreeController : MonoBehaviour
{
    public List<SkillNode> goodSkills = new List<SkillNode>();
    public List<SkillNode> badSkills = new List<SkillNode>();
    private EventsController events;

    [SerializeField] private RectTransform gHolder;
    [SerializeField] private RectTransform bHolder;

    void Start()
    {
        events = DataManager.Events;
        events.OnCoinsSpend.AddListener(UpdateTree);

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
        if(!events.GameplayData.PlayerHasNoSkills)
        {
            if(events.GameplayData.PlayerIsImpostor) foreach (SkillNode skill in goodSkills) skill.DisableNode();
            else foreach (SkillNode skill in badSkills) skill.DisableNode();
        }
    }

    private void OnDestroy()
    {
        events.OnCoinsSpend.RemoveListener(UpdateTree);
    }
}
