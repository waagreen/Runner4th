using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : Singleton<DataManager>
{   
    [SerializeField] private GlobalMovement globalMovement; 
    [SerializeField] private PoolingMaster poolingMaster;

    public static GlobalMovement GlobalMovement => Instance.globalMovement;
    public static PoolingMaster PoolingMaster => Instance.poolingMaster;
}
