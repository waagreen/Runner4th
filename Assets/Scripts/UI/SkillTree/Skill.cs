using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;

[CreateAssetMenu (menuName = "Skills/Create new Skill")]
public class Skill : ScriptableObject
{
    [Header("DOESN'T CHANGE")]
    public int id;
    public Sprite icon;
    public string title = "generic skill";
    public string description = "this is a generic skill";

    [Header("Base values")]
    [SerializeField] private int baseCost;
    [SerializeField] private int baseLevel;
    [SerializeField] private int maxLevel;
    public int MaxLevel => maxLevel;
    public int BaseCost => baseCost;

    [Header("Current Values")]
    public int currentCost;
    public int currentLevel;
    public float increaseAmount;
    public int TotalAmountSpent;

    [ButtonMethod]
    public void ResetValues()
    {
        currentCost = baseCost;
        currentLevel = baseLevel;
        TotalAmountSpent = 0;
    }
}
