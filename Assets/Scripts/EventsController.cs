using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventsController : MonoBehaviour
{
    [SerializeField] private PlayerGameplayData gameData;

    [HideInInspector] public UnityEvent OnPlayerDeath = new UnityEvent();
    [HideInInspector] public UnityEvent<int> OnCollectCoin = new UnityEvent<int>();

    public PlayerGameplayData GameplayData => gameData; 


    private void Awake() 
    {
        OnCollectCoin.AddListener(AddCoinOnData);
    }

    private void AddCoinOnData(int coinsToAdd) => gameData.currentReservedCoins += coinsToAdd;

    private void OnDestroy() 
    {
        OnCollectCoin.RemoveListener(AddCoinOnData);
    }
}
