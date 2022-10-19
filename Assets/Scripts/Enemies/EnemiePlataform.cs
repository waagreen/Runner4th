using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiePlataform : MonoBehaviour
{

    [SerializeField] private float amplification;
    [SerializeField] private float frequency;
    private Vector3 currentPosition;

    void Awake() => currentPosition = transform.position;

    void Update(){
        transform.position = new Vector3(currentPosition.x, Mathf.Sin(Time.time * frequency) * amplification + currentPosition.y, currentPosition.z);
    }
    
}
