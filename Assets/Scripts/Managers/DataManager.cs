using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DataManager
{   
    //acessa o gameObject que segura os scripts VITAIS para a fase funcionar.
    private static GameObject levelManager = GameObject.FindGameObjectWithTag("LevelManager");

    //managers
    public static GlobalMovement globalMovement = levelManager.GetComponent<GlobalMovement>();
    public static PoolingMaster masterPool = levelManager.GetComponent<PoolingMaster>();
}
