using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class Coins : MonoBehaviour
{
    public AudioSource audioScr;

    public int coinValue = 1;

    private async void OnTriggerEnter(Collider other) 
    {
        audioScr.Play();
        DataManager.Events.OnCollectCoin.Invoke(coinValue);
        await Task.Delay(115);
        gameObject.SetActive(false);
    }
}
