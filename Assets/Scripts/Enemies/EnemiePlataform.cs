using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiePlataform : MonoBehaviour
{
    [SerializeField] private Transform targetA, targetB; 
    [SerializeField] private float speed = 3f;
    private bool switching = false;
    void FixedUpdate()
    {
        if(targetA == null || targetB == null) return;

        if (!switching)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetA.position, speed * Time.deltaTime); 
        }
        else if (switching)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetB.position, speed * Time.deltaTime); 
        }
        if (transform.position == targetA.position)
        {
        switching = true;
        }
        else if (transform.position == targetB.position)
        {
            switching = false;
        }
    }
}
