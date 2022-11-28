using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : Singleton<DataManager>
{   
    [SerializeField] private GlobalMovement globalMovement; 
    [SerializeField] private PoolingMaster poolingMaster;
    [SerializeField] private UiController ui;
    [SerializeField] private EventsController events;

    public static GlobalMovement GlobalMovement => Instance.globalMovement;
    public static PoolingMaster PoolingMaster => Instance.poolingMaster;
    public static UiController Ui => Instance.ui;
    public static EventsController Events => Instance.events;

    public static bool isGameplay => UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex > 2;
    
    public const string playerTag = "Player";
    public const string target = "Target";
}
