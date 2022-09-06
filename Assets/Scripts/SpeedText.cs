using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpeedText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI speedText;
    [SerializeField] private TextMeshProUGUI distanceText;

    private float displaySpeed => DataManager.globalMovement.ActualSpeed;
    private float distance;

    void Update()
    {
        //Sets the speed and distance text
        distance += displaySpeed * Time.deltaTime;

        speedText.text = $"{Mathf.Floor(displaySpeed)} m/s";
        distanceText.text = $"{Mathf.Floor(distance)} m";
    }
}
