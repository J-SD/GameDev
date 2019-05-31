using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public List<GameObject> enemies;
    // Start is called before the first frame update
    void Start()
    {
        GameObject toSpawn = enemies[Random.Range(0, enemies.Count)];
        Instantiate(toSpawn, transform.position, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
