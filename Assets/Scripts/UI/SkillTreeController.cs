using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using MyBox;
using System.Linq;
using System.Threading.Tasks;

public class SkillTreeController : MonoBehaviour
{
    public List<SkillNode> goodSkills = new List<SkillNode>();
    public List<SkillNode> badSkills = new List<SkillNode>();
    private List<SkillNode> GetActiveList() => playerIsImpostor ? badSkills : goodSkills;
    private EventsController events;
    private bool playerIsImpostor => events.GameplayData.PlayerIsImpostor;
    private bool hasNoSkills => events.GameplayData.PlayerHasNoSkills;
    private static bool firstBuy = true;

    [SerializeField] private RectTransform gHolder;
    [SerializeField] private RectTransform bHolder;
    [SerializeField] private Button resetButton;
    [SerializeField] private Material tilingMat;
    
    private Color kRed = new Color(0.89f, 0.25f, 0.27f);
    private Color kBlue = new Color(0.26f, 0.81f, 0.89f);
    private Color kNeutral = new Color(0.45f, 0.45f, 0.45f);

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
        resetButton.gameObject.SetActive(!hasNoSkills);
        
        if (hasNoSkills)
        {
            tilingMat.SetColor("_BaseColor", kNeutral);
            SetupForFirstPurchase();
        }
        else
        {   
            Color bgColor = playerIsImpostor ? kRed : kBlue;
            tilingMat.SetColor("_BaseColor", bgColor);
            DisableOppositeSide();

            List<SkillNode> activeList = GetActiveList();
            
            foreach (SkillNode skill in activeList)
            {
                SkillNode previousSkill = null;
                int currentIndex = activeList.IndexOf(skill);
                previousSkill = currentIndex == 0 ? activeList[currentIndex] : activeList[currentIndex-1];
                skill.SetupNode(previousSkill.CurrentLevel);
            }
            
            events.OnSkillTreeLock.Invoke();
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

    private void SetupForFirstPurchase()
    {
        foreach (SkillNode skill in goodSkills)
        {
            int currentIndex = goodSkills.IndexOf(skill);
            
            skill.SetupNode(0);
            
            if (currentIndex > 0)
            {
                skill.DisableNode();
            }
            else skill.EnableNode();
        }
        
        foreach (SkillNode skill in badSkills)
        {
            int currentIndex = badSkills.IndexOf(skill);
            
            skill.SetupNode(0);
            
            if (currentIndex > 0)
            {
                skill.DisableNode();
            }
            else skill.EnableNode();
        }
    }

    [ButtonMethod]
    private void ResetAllSkillValues()
    {
        firstBuy = true;
        gHolder.gameObject.SetActive(true);
        bHolder.gameObject.SetActive(true);

        int coinsToReturn = 0;
        events.GameplayData.ResetSkillTree();

        foreach (SkillNode node in goodSkills)
        {
            if (node.CurrentLevel > 0)
            {
                coinsToReturn += node.TotalAmountSpent;
                node.ResetSkillValues();
                node.UpdateNode();
            }
        }
        foreach (SkillNode node in badSkills)
        {
            if (node.CurrentLevel > 0)
            {
                coinsToReturn += node.TotalAmountSpent;
                node.ResetSkillValues();
                node.UpdateNode();
            }
        }
        
        events.GameplayData.AddCoinsToTotal(coinsToReturn);
        events.OnCoinsSpend.Invoke();
        UpdateTree();
    }

    private void OnDestroy()
    {
        events.OnCoinsSpend.RemoveListener(UpdateTree);
        resetButton.onClick.RemoveListener(ResetAllSkillValues);
    }
}
