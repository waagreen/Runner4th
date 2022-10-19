using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFlying : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float lineOfSite;
    [SerializeField] private GameObject bullet, bulletParent;
    private Transform player;

    //find the player
    void Awake() => player = GameObject.FindGameObjectWithTag(DataManager.playerTag).transform;
    

    void Update(){
        //calculate the distance between player and enemie
        float distanceFromPlayer = Vector3.Distance(player.position, transform.position);

        //set a limit to the enemie view
        if(distanceFromPlayer < lineOfSite){
            transform.position = Vector3.MoveTowards(this.transform.position, player.position, speed * Time.deltaTime);
        }

    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.green;
        //set the trigger sphere
        Gizmos.DrawWireSphere(transform.position, lineOfSite);
    }
}
