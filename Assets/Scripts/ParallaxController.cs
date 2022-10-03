using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxController : MonoBehaviour
{
    private Transform[] layers;
    private float[] layersBounds;

    private float startX;
    private float increment;
   
    private float speed => DataManager.GlobalMovement.ActualSpeed / 5f;
    private float distance => DataManager.GlobalMovement.distance * -1f;

    void Start()
    {
        layers = new Transform[transform.childCount];
        layersBounds = new float[layers.Length];

        int childCount = 0;
        foreach (Transform child in transform)
        {
            layers[childCount] = child;
            float currentLayerWidth = 0f;

            foreach (Transform childOfChild in child)
            {
                SpriteRenderer sR = childOfChild.GetComponent<SpriteRenderer>();
                if(sR != null)
                {
                    sR.sortingOrder = childCount;
                    currentLayerWidth += sR.bounds.size.x; 
                }
            }
            
            layersBounds[childCount] = currentLayerWidth;
            childCount++;
        }

        foreach (var item in layersBounds)
        {
            Debug.Log(item);
        }
    }

    void Update()
    {
        int count = 0;
        foreach (Transform layer in layers)
        {
            increment = speed / layers.Length;
            
            float multiplier = increment * count;
            float xOffSet = distance * multiplier;


            layer.position = new Vector3(distance + xOffSet, layer.position.y, layer.position.z);
            if(Mathf.Abs(layer.position.x) < layersBounds[count])
            {
                layer.position = new Vector3(distance, layer.position.y, layer.position.z);
                Debug.Log("GOT HERE");
            }

            count++;
        }
    }
}
