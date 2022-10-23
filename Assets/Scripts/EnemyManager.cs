using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyManager : MonoBehaviour
{
    public GameObject enemy; // prefab
    public GameObject letter; // prefab

    // For application status of ENEMY
    public TextMeshProUGUI textEnemy;
    public int totalDestroyed = 0;

    // For different waypoints
    public List<WayPoint> letters;
    public List<Enemy> enemies;

    public bool isRandom = false; 

    // For application status of Enemy to Waypoints mode
    public TextMeshProUGUI textWaypoint;
    public string rotateMode;

    // Start is called before the first frame update
    void Start()
    {
        // Always 10 enemies in the game screen
        for (int i = 0; i < 10; i++) {
            spawningNewEnemies();
        }
    }

    // Update is called once per frame
    void Update()
    {
        // waypoint mode with 'j' key sequential and random movement
        // changes the mode(status)
        if (Input.GetKeyDown("j")) {
            
            // false -> true or true -> false
            isRandom = !isRandom;
        }

        if (isRandom) {
            // application status
            rotateMode = "Random";
        }
        else {
            // application status
            rotateMode = "Squential";
        }
        textWaypoint.text = "WAYPOINTS: (" + rotateMode + ")";
    }

    // New enemy will be created at the same time one destroyed
    public void spawningNewEnemies() {
        GameObject newEnemy = Instantiate(enemy);
        newEnemy.GetComponent<Enemy>().manager = this;

        // adding enemies in the list
        enemies.Add(newEnemy.GetComponent<Enemy>());

        // Enemies are within 90% of the boundaries
        float maxY = (Camera.main.orthographicSize) * 0.9f;
        float maxX = (Camera.main.orthographicSize * Camera.main.aspect) * 0.9f;
        float enemyX = Random.Range(-maxX, maxX);
        float enemyY = Random.Range(-maxY, maxY);
        newEnemy.transform.position = new Vector3(enemyX, enemyY, 0f);
    }

    // to display total number of destoryed enemies
    public void destroyEnemies() {
        totalDestroyed++;
        textEnemy.text = "ENEMY: Count(10) Destroyed(" + totalDestroyed + ") ";
    }

    // New waypoint will be created at the same time one destroyed
    public void spawnLetter(WayPoint oldletter) {
        // to save index of the waypoints
        int index = letters.IndexOf(oldletter);
        // to remove old waypoint from the list
        letters.Remove(oldletter);

        // to instantiate with original position of the waypoint
        Vector3 oldPosition = oldletter.gameObject.transform.position;
        GameObject spawnedLetter = Instantiate(letter);
        spawnedLetter.GetComponent<WayPoint>().manager = this;

        // to insert new created waypoint to the list
        letters.Insert(index, spawnedLetter.GetComponent<WayPoint>());

        // to set new sprite to old sprite
        Sprite oldSprite = oldletter.gameObject.GetComponent<SpriteRenderer>().sprite;
        spawnedLetter.GetComponent<SpriteRenderer>().sprite = oldSprite;

        // radom reposition of waypoints at + or - 15 units
        // in both X and Y from initial position
        float letterX = Random.Range(oldPosition.x - 15, oldPosition.x + 15);
        float letterY = Random.Range(oldPosition.y - 15, oldPosition.y + 15);
        spawnedLetter.transform.position = new Vector3(letterX, letterY, 0f);

        // to tell all enemies new position of waypoint that they're going to (current index of waypoint)
        for (int i = 0; i < enemies.Count; i++) {
            enemies[i].destroyWaypoint(index);
        }
    }
}