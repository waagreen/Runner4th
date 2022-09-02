using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpeedText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI speedText;
    [SerializeField] private TextMeshProUGUI distanceText;
    [SerializeField] private Movement player;

    private float distance;

    void Update()
    {
        //Sets the speed and distance text
        distance += player.runSpeed * Time.fixedDeltaTime;

        speedText.text = $"{Mathf.Floor(player.runSpeed)} m/s";
        distanceText.text = $"{Mathf.Floor(distance)} m";
	}
}
