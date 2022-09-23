using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpeedText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI speedText;
    [SerializeField] private TextMeshProUGUI distanceText;

    private GlobalMovement globalMovement => DataManager.globalMovement;

    private float displaySpeed;
    private float distance;

    private void FixedUpdate()
    {
        //Sets the speed and distance text

        speedText.text = $"{Mathf.Floor(globalMovement.ActualSpeed * 3.6f)}km/h";
        distanceText.text = $"{Mathf.Floor(globalMovement.distance)} m";
    }
}
