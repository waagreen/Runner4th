using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class Coins : MonoBehaviour
{
    public AudioSource audioScr;

    public int coinValue = 1;
    private const string playerTag = "Player";

    private async void OnTriggerEnter(Collider other) 
    {
        if(other.tag == playerTag)
        {
            audioScr.Play();
            DataManager.Events.OnCollectCoin.Invoke(coinValue);
            await Task.Delay(150);
            gameObject.SetActive(false);
        }
    }
}
