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

    private GlobalMovement globalMove;
    private EventsController events;
    
    private void Start() 
    {
        globalMove = DataManager.GlobalMovement;
        events = DataManager.Events;

        float _bestDist = events.GameplayData.BestDistance;
        bestDistance.SetText($"Best distance: <b>{_bestDist}m</b>");   
    }
    
    private void FixedUpdate()
    {
        float _velocity = globalMove.CurrentSpeed;
        float _distance = globalMove.distance;
        int _coins = events.GameplayData.currentReservedCoins;

        speed.SetText($"{Mathf.Floor(_velocity * 3.6f)}km/h");
        distance.SetText($"{Mathf.Floor(_distance)} m");
        coins.SetText($"Coins: <b>{_coins}</b>");
    }
}
