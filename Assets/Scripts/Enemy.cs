using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Enemy : MonoBehaviour
{
    public EnemyManager manager;
    public int energy = 100; // for 4 times collsion

    
    private int index = 0;
    private Vector3 positionWaypoint; // position of the next point that enemy is moving to
    
    // for waypoint mode
    public float speed = 20f;
    public float rotateSpeed = 45f;

    // Start is called before the first frame update
    void Start()
    {
        // to get specific position of current waypoint
        positionWaypoint = manager.letters[index].gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {   
        // to move foward enemies 
        speed += Input.GetAxis("Vertical");
        transform.position += transform.up * (speed * Time.smoothDeltaTime);
        Vector3 curPoisition = transform.position;

        // to rotate to the next waypoint
        Vector3 relativePosition = positionWaypoint - curPoisition;
        Quaternion lookRotation = Quaternion.LookRotation(Vector3.forward, relativePosition);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotateSpeed * Time.deltaTime);
        
        if (relativePosition.magnitude < 1) {
            findNextWaypoint();
        }

    }

    // to move to the next waypoint
    private void findNextWaypoint() {
        // rotate randomly
        if (manager.isRandom) { 
            index = Random.Range(0, manager.letters.Count - 1);
        }
        // rotate sequentially
        else {
            if (index < manager.letters.Count - 1) {
                index++;
            }
            else {
                index = 0;
            }
        }
        
        // set to new position
        positionWaypoint = manager.letters[index].gameObject.transform.position;
    }

    public void destroyWaypoint(int indexOfLetter) {
        if (indexOfLetter == index) {
            positionWaypoint = manager.letters[index].gameObject.transform.position;
        }
    } 

    void OnTriggerEnter2D(Collider2D collision) {

        // Comes in contact with eggs
        // Loses 25% of current energy
        // Once energy gets to 0% --> destroy the enemy & spawn new one
        if (collision.gameObject.CompareTag("Egg")) {
            energy -= 25;
            
            // Color changes after collision
            Color losingColor = GetComponent<SpriteRenderer>().color;
            losingColor.a = energy * 0.008f;
            GetComponent<SpriteRenderer>().color = losingColor;
            
            if (energy <= 0) {
                // As soon as one enemy destroyed, new enemy will be created
                Destroy(this.gameObject);
                manager.spawningNewEnemies();
                manager.destroyEnemies();
            }
        }
        else if (collision.gameObject.CompareTag("Player")) {
            // As soon as one enemy destroyed, new enemy will be created
            manager.spawningNewEnemies();
            Destroy(this.gameObject);
            manager.destroyEnemies();
        }
        else if (!collision.gameObject.CompareTag("Enemy") && !collision.gameObject.CompareTag("Waypoints")) {
            // As soon as one enemy destroyed, new enemy will be created
            manager.spawningNewEnemies();
            Destroy(this.gameObject);
        }
    }
}