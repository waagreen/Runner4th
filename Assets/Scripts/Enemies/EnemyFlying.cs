using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFlying : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float lineOfSite;
    [SerializeField] private float shootingRange;
    [SerializeField] private float fireRate = 1f;
    [SerializeField] private GameObject bullet, bulletParent;
    private float fireTime;
    private Transform player;

    //find the player
    void Awake() => player = GameObject.FindGameObjectWithTag(DataManager.playerTag).transform;
    

    void Update(){
        //calculate the distance between player and enemie
        float distanceFromPlayer = Vector3.Distance(player.position, transform.position);

        //set a limit to the enemie view and shooting range
        if(distanceFromPlayer < lineOfSite && distanceFromPlayer > shootingRange){
            transform.position = Vector3.MoveTowards(this.transform.position, player.position, speed * Time.deltaTime);
        }
        //preparing to shoot and cooldown for it
        else if(distanceFromPlayer <= shootingRange && fireTime < Time.time){
            Instantiate(bullet, bulletParent.transform.position, Quaternion.identity);
            fireTime = Time.time + fireRate;
        }

    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.green;
        //set the trigger sphere
        Gizmos.DrawWireSphere(transform.position, lineOfSite);
        Gizmos.DrawWireSphere(transform.position, shootingRange);
    }
}
