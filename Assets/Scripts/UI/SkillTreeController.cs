using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class SkillTreeController : MonoBehaviour
{
    private List<SkillNode> goodSkills = new List<SkillNode>();
    private List<SkillNode> badSkills = new List<SkillNode>();

    [SerializeField] private RectTransform gHolder;
    [SerializeField] private RectTransform bHolder;

    void Start()
    {
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

    }

    void Update()
    {
        
    }
}
