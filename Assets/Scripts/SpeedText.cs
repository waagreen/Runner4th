using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpeedText : MonoBehaviour
{
    [SerializeField] private TMP_Text speedText;
    [SerializeField] private TMP_Text distanceText;
    [SerializeField] private TMP_Text bestDistance;

    private float velocity => DataManager.GlobalMovement.ActualSpeed;
    private float distance => DataManager.GlobalMovement.distance;
    private PlayerGameData gameData => DataManager.GlobalMovement.GameData;
    private float displaySpeed;
    private void Awake() 
    {
        bestDistance.SetText($"BEST DISTANCE: {gameData.currentBestDistance}");    
    }
    private void FixedUpdate()
    {

        //Sets the speed and distance text
        speedText.SetText($"{Mathf.Floor(velocity * 3.6f)}km/h");
        distanceText.SetText($"{Mathf.Floor(distance)} m");
    }
}
