using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseScreen : MonoBehaviour
{
    [SerializeField] private Button resumeButton;


    void Start()
    {
        resumeButton.onClick.AddListener(DataManager.Events.HandlePause);
    }

    void OnDestroy() 
    {
        resumeButton.onClick.RemoveListener(DataManager.Events.HandlePause);
    }
}
