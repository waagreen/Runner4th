using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class SkillTreeController : MonoBehaviour
{
    private List<SkillNode> goodSkills = new List<SkillNode>();
    private List<SkillNode> badSkills = new List<SkillNode>();
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
            goodSkills.Add(newSkill);
        }        
        
        foreach (Transform child in bHolder.transform)
        {
            var newSkill = child.GetComponent<SkillNode>();
            badSkills.Add(newSkill);
        }

        Debug.Log(events.GameplayData.PlayerHasNoSkills);
        events.GameplayData.DebugDictionary();
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
