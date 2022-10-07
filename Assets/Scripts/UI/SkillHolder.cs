using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillHolder : MonoBehaviour
{
	public SkillTree skillTree;
	public GameObject connectorHolder;

	public List<Skill> SkillList;
	public List<GameObject> connectorList;

	
	public void Start()
	{
		foreach (RectTransform connector in GetComponentsInChildren<RectTransform>()) connectorList.Add(connector.gameObject);
		foreach (Skill skill in GetComponentsInChildren<Skill>())
		{ 
			SkillList.Add(skill);
		}

		SkillList[0].ConnectedSkills = new int[] { 1 };
		SkillList[1].ConnectedSkills = new int[] { 2 };
		SkillList[2].ConnectedSkills = new int[] { 3 };

		UpdateAllSkillUI();
	}

	public void UpdateAllSkillUI()
	{
		foreach (Skill skill in SkillList)
		{
			skill.UpdateUI(this, skillTree);
		}
	}
}
