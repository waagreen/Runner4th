using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SceneOrder
{
    MainMenu,
    FirstLevel,
    SecondLevel,
    ThirdLevel,
}

public class DataManager : Singleton<DataManager>
{   
    [SerializeField] private GlobalMovement globalMovement; 
    [SerializeField] private PoolingMaster poolingMaster;
    [SerializeField] private TransitionController loader;

    public static GlobalMovement GlobalMovement => Instance.globalMovement;
    public static PoolingMaster PoolingMaster => Instance.poolingMaster;
    public static TransitionController Loader => Instance.loader;
}
