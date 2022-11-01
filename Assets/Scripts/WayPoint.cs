using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPoint : MonoBehaviour
{
    public EnemyManager manager;
    public int health = 100; // for 4 times collsion

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // to hide and show waypoints
        if (Input.GetKeyDown("h")) {
            Color fadeColor = GetComponent<SpriteRenderer>().color;
            if (fadeColor.a != 0.0f) {
                fadeColor.a = 0.0f;
            }
            else {
                fadeColor.a = 1.0f;
            }
            GetComponent<SpriteRenderer>().color = fadeColor;
        }
    }

    void OnTriggerEnter2D(Collider2D collision) {

        // Comes in contact with eggs
        // Loses 25% of current energy
        // Once energy gets to 0% --> destroy the waypoint & spawn new one
        if (collision.gameObject.CompareTag("Egg")) {
            health -= 25;
            
            // Color changes after collision
            Color fadeColor = GetComponent<SpriteRenderer>().color;
            fadeColor.a = health * 0.01f;
            GetComponent<SpriteRenderer>().color = fadeColor;
            
            if (health <= 0) {
                // As soon as one waypoint destroyed, new waypoint will be created
                // randomly located at +- 15 units from initial position
                Destroy(this.gameObject);
                manager.spawnLetter(this);
            }
        }
    }
}