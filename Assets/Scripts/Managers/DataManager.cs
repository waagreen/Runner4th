using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DataManager
{
    public static GlobalMovement globalMovement = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<GlobalMovement>();
}
