using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolingObjectReturner : MonoBehaviour
{
    private PoolingMaster objectPool => DataManager.PoolingMaster;

    private void OnDisable() 
    {
        if (objectPool != null)
        {
            objectPool.ReturnGameObject(gameObject);
            // Debug.Log($"{gameObject.name} was disabled and stored");
        }
    }
}
