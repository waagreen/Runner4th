using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PoolingMaster : MonoBehaviour
{
    //MANTER UMA ÚNICA INSTANCIA POR CENA

    private Dictionary<string, Queue<GameObject>> objectPool = new Dictionary<string, Queue<GameObject>>();

    public GameObject GetObject(GameObject gameObject)
    {
        if (objectPool.TryGetValue(gameObject.name, out Queue<GameObject> objectList))
        {
            if (objectList.Count == 0) 
                return CreateNewObject(gameObject);
            else
            {
                GameObject _object = objectList.Dequeue();
                _object.SetActive(true);
                return _object;
            }
        }
        else return CreateNewObject(gameObject);
    }

    private GameObject CreateNewObject(GameObject gameObject)
    {
        GameObject newGameObj = Instantiate(gameObject);
        newGameObj.name = gameObject.name;
        return newGameObj;
    }

    public void ReturnGameObject(GameObject gameObject)
    {
        if (objectPool.TryGetValue(gameObject.name, out Queue<GameObject> objectList))
        {
            objectList.Enqueue(gameObject);
        }
        else
        {
            Queue<GameObject> newObjectQueue = new Queue<GameObject>();
            newObjectQueue.Enqueue(gameObject);
            objectPool.Add(gameObject.name, newObjectQueue);
        }

        gameObject.SetActive(false);
    }

}
