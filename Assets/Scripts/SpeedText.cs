using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpeedText : MonoBehaviour
{
    [SerializeField] private TMP_Text speed;
    [SerializeField] private TMP_Text distance;
    [SerializeField] private TMP_Text bestDistance;
    [SerializeField] private TMP_Text coins;

    private float _velocity => DataManager.GlobalMovement.CurrentSpeed;
    private float _distance => DataManager.GlobalMovement.distance;
    private int _bestDist => DataManager.GlobalMovement.gameplayData.currentBestDistance;
    private int _currentCoins => DataManager.Events.GameplayData.currentReservedCoins;

    private void Awake() 
    {
        bestDistance.SetText($"Best distance: <b>{_bestDist}m</b>");    
    }
    
    private void FixedUpdate()
    {
        speed.SetText($"{Mathf.Floor(_velocity * 3.6f)}km/h");
        distance.SetText($"{Mathf.Floor(_distance)} m");
        coins.SetText($"Coins: <b>{_currentCoins}</b>");
    }
}
