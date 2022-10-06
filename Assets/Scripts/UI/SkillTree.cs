using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Skill Tree", menuName = "Skill Data")]
public class SkillTree : ScriptableObject
{
    public IntValue skillPoints;
    
    public int[] skillCaps;
    public int[] skillLevels;
    public string[] skillNames;
    public string[] skillDescriptions;

    public int[] connectedSkills;
}
