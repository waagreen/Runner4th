using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coins : MonoBehaviour
{
    public int coinValue = 1;

    private const string playerTag = "Player";

    private void OnTriggerEnter(Collider other) 
    {
        if(other.tag == playerTag)
        {
            gameObject.SetActive(false);
            DataManager.Events.OnCollectCoin.Invoke(coinValue);
        }
    }
}
