using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpeedText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI speedText;
    [SerializeField] private TextMeshProUGUI distanceText;

    private float velocity => DataManager.GlobalMovement.ActualSpeed;
    private float distance => DataManager.GlobalMovement.distance;
    private float displaySpeed;
    
    private void FixedUpdate()
    {
        //Sets the speed and distance text
        speedText.text = $"{Mathf.Floor(velocity * 3.6f)}km/h";
        distanceText.text = $"{Mathf.Floor(distance)} m";
    }
}
