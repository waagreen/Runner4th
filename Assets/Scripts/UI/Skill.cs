using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Skill : MonoBehaviour
{
    [SerializeField] private SkillTree skillTree;
    [SerializeField] private SkillHolder SkillHolder;

    public int id;

    public TextMeshProUGUI TitleText;
    public TextMeshProUGUI DescriptionText;
    public Image image;

    public int[] ConnectedSkills;

    public void UpdateUI()
    {
        TitleText.text = $"{skillTree.skillLevels[id]}/{skillTree.skillCaps[id]}\n{skillTree.skillNames[id]}";
        DescriptionText.text = $"{skillTree.skillDescriptions[id]}\nCost: {skillTree.skillPoints.value}/1 SP";

        image.color = skillTree.skillLevels[id] >= skillTree.skillCaps[id] ? Color.yellow
            : skillTree.skillPoints.value > 1 ? Color.green : Color.white;

        foreach (var connectedSkill in ConnectedSkills)
        {
            SkillHolder.SkillList[connectedSkill].gameObject.SetActive(skillTree.skillLevels[id] < 0);
            SkillHolder.connectorList[connectedSkill].SetActive(skillTree.skillLevels[id] < 0);
        }
    }

    public void Buy()
    {
        if (skillTree.skillPoints.value < 1 || skillTree.skillLevels[id] >= skillTree.skillCaps[id]) return;
        skillTree.skillPoints.value -= 1;
        skillTree.skillLevels[id]++;
        SkillHolder.UpdateAllSkillUI();
    }
}
