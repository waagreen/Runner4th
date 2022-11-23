using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class Coins : MonoBehaviour
{
	public Renderer sphere;
    public AudioSource audioScr;
    public int coinValue;

	private Gradient[] grads => DataManager.GlobalMovement.velocityGradients;
    private float redCoinChance;

    private void Start() {
        redCoinChance = DataManager.Events.passiveSkills.redCoinChance;
        Setup();
    }

    private void Setup()
    {
        float diceThrow = Random.Range(0f, 100f);
        if (redCoinChance >= diceThrow) SetRedCoin();
        else SetGreenCoin();
    }

    private async void OnTriggerEnter(Collider other) 
    {
        audioScr.Play();
        DataManager.Events.OnCollectCoin.Invoke(coinValue);
        await Task.Delay(115);
        gameObject.SetActive(false);
    }

    private void SetRedCoin()
    {
        coinValue = 5;
        sphere.material.color = grads[2].colorKeys[1].color;
        transform.localScale = Vector3.one * 1.2f;
    }

    private void SetGreenCoin()
    {
        coinValue = 1;
        sphere.material.color = grads[0].colorKeys[1].color;
        transform.localScale = Vector3.one;
    }
}
