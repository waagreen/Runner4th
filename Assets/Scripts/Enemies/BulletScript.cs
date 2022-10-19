using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    [SerializeField] private float speed;
    private GameObject target;
    private Rigidbody bulletRB;

    void Awake(){
        bulletRB = GetComponent<Rigidbody>();
        target = GameObject.FindGameObjectWithTag(DataManager.playerTag);

        Vector3 move = (target.transform.position - transform.position).normalized * speed;
        bulletRB.velocity = new Vector3(move.x, move.y, move.z);
        Destroy(this.gameObject, 2);
    }
}
