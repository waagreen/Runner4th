using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Skills/Create new Skill")]
public class Skill : ScriptableObject
{
    public int id = 0;
    public Sprite icon;
    public string title = "generic skill";
    public string description = "this is a generic skill";
    public int cost;
    public int currentLevel;
    public int mxLevel;
    public float increaseAmount;
}
