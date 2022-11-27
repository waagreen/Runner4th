using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFlying : MonoBehaviour
{
    [Header("Speed and Bounds")]
    [SerializeField] private float speed;
    [SerializeField] private float lineOfSite;
    private Transform player, target;
    
    [Space]
    [Header("Bullet Property")]
    [SerializeField] private float shootingRange;
    [SerializeField] private float fireRate = 1f;
    [SerializeField] private GameObject bullet, bulletParent;
    private float fireTime;

    [Space]
    [Header("Time for Destroy then object")]
    [SerializeField] private float timeToDie = 10f;
    private float timeToDestroy;

    //find the player
    void Awake(){
        player = GameObject.FindGameObjectWithTag(DataManager.playerTag).transform;
        target = GameObject.FindGameObjectWithTag(DataManager.target).transform;
    } 
    

    void Update(){
        //calculate the distance between player and enemie
        float distanceFromPlayer = Vector3.Distance(player.position, transform.position);

        //set a limit to the enemie view and shooting range
        if(distanceFromPlayer < lineOfSite && distanceFromPlayer > shootingRange){
            transform.position = Vector3.MoveTowards(this.transform.position, target.position, speed * Time.deltaTime);
        }
        //preparing to shoot and cooldown for it
        else if(distanceFromPlayer <= shootingRange && fireTime < Time.time){
            Instantiate(bullet, bulletParent.transform.position, Quaternion.identity);
            fireTime = Time.time + fireRate;
        }
        else if(timeToDestroy < Time.time){
            timeToDestroy = timeToDie + Time.time;
        }

    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.green;
        //set the trigger sphere
        Gizmos.DrawWireSphere(transform.position, lineOfSite);
        Gizmos.DrawWireSphere(transform.position, shootingRange);
    }
}
