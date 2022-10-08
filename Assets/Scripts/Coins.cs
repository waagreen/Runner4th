using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coins : MonoBehaviour
{
    [SerializeField] private IntValue skillPoints;

    private void OnTriggerEnter(Collider other) 
    {
        skillPoints.value++;
        gameObject.SetActive(false);
    }
}
