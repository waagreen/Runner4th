using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ResetPoints : MonoBehaviour
{
    [SerializeField] private SkillTree skillTree;
    [SerializeField] private SkillHolder skillHolder;
    public void ResetSkillPoints()
    {
        for (int i = 0; i < skillTree.skillLevels.Length; i++)
        {
            int points = skillTree.skillLevels[i];
            skillTree.skillLevels[i] = 0;
            skillTree.skillPoints += points;
        }

        skillHolder.UpdateAllSkillUI();
    }
}
