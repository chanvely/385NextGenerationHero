using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Egg : MonoBehaviour
{
    public float speed = 40f;
    public Hero decreaseEggs;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Spawned eggs traverse towards its direction at a speed of 40 units/sec
        transform.position += transform.up * (speed * Time.smoothDeltaTime);

    }

    void OnBecameInvisible() {
        Destroy(this.gameObject);
        decreaseEggs.decreaseNumEggs();
    }

    // void OnTriggerEnter2D(Collider2D collision) {
        
    //     if (collision.gameObject.CompareTag("Enemy")) {
    //         Destroy(this.gameObject);
    //     }
    //     else if (collision.gameObject.CompareTag("Waypoints")) {
    //         Destroy(this.gameObject);
    //     }
    // }
}