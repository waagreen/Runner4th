using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpeedText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI speedText;
    [SerializeField] private TextMeshProUGUI distanceText;

    [SerializeField] private GlobalDataSO globalData;

    private void FixedUpdate()
    {
        //Sets the speed and distance text
        speedText.text = $"{Mathf.Floor(globalData.ActualSpeed * 3.6f)}km/h";
        distanceText.text = $"{Mathf.Floor(globalData.Distance)} m";
    }
}
