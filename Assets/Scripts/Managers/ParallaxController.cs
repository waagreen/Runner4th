using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxController : MonoBehaviour
{
    private Transform[] layers;
    private float[] layersBounds;

    private float startX;
    private float increment;
   
    private float speed => DataManager.GlobalMovement.CurrentSpeed * 5;
    private List<Vector3> startPositions;

    void Start()
    {

        startPositions = new List<Vector3>();
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
                    currentLayerWidth += sR.bounds.size.x / 2f; 
                }
            }
            
            startPositions.Add(child.transform.position);
            layersBounds[childCount] = currentLayerWidth;
            childCount++;
        }
    }

    void Update()
    {
        int count = 0;
        foreach (Transform layer in layers)
        {
            increment = speed / layers.Length;
            
            Vector3 startPos = startPositions[count];
            float layerWidth = layersBounds[count]; 

            float multiplier = increment * count;

            layer.localPosition += Vector3.left * multiplier * Time.deltaTime;

            if(layer.localPosition.x < startPos.x - layerWidth) layer.position = startPos;

            count++;
        }
    }
}
