using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Skill : MonoBehaviour
{
    private SkillTree skillTree;
    private SkillHolder skillHolder;

    public int id;

    public TextMeshProUGUI TitleText;
    public TextMeshProUGUI DescriptionText;
    public Image image;

    public int[] ConnectedSkills;

    public void UpdateUI(SkillHolder skillHolder, SkillTree skillTree)
    {
        this.skillTree = skillTree;
        this.skillHolder = skillHolder;

        TitleText.text = $"{skillTree.skillLevels[id]}/{skillTree.skillCaps[id]}\n{skillTree.skillNames[id]}";
        DescriptionText.text = $"{skillTree.skillDescriptions[id]}\nCost: {skillTree.skillPoints}/1 SP";

        image.color = skillTree.skillLevels[id] >= skillTree.skillCaps[id] ? Color.yellow
            : skillTree.skillPoints > 1 ? Color.green : Color.white;

        foreach (var connectedSkill in ConnectedSkills)
        {
            skillHolder.SkillList[connectedSkill].gameObject.SetActive(skillTree.skillLevels[id] < 0);
            skillHolder.connectorList[connectedSkill].SetActive(skillTree.skillLevels[id] < 0);
        }
    }

    public void Buy()
    {
        if (skillTree.skillPoints < 1 || skillTree.skillLevels[id] >= skillTree.skillCaps[id]) return;
        skillTree.skillPoints -= 1;
        skillTree.skillLevels[id]++;
        skillHolder.UpdateAllSkillUI();
    }
}
