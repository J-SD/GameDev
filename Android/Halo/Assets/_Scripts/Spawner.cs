using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float spawnRadius = 8f;
    private float enemySpeed = 500f;
    public float difficultyMod = 1.1f;
    public int difficultyLevel = 1;

    private float nextSpawnTime = 0.0f;
    private float spawnPeriod = 3f;
    private int spawnCount;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        if (Time.time > nextSpawnTime)
        {
            spawnCount++;
            nextSpawnTime += spawnPeriod;
            Vector3 spawnPoint = RandomPointOnCircleEdge(spawnRadius);

            GameObject ob = Instantiate(enemyPrefab, spawnPoint, Quaternion.identity);
            ob.transform.LookAt(Vector3.zero);
            ob.transform.Rotate(Vector3.right, 180f);

            Enemy enemy = ob.GetComponent<Enemy>();

            enemy.despawnTime = 10f / difficultyMod; 


            Rigidbody rb = ob.GetComponent<Rigidbody>();

            rb.AddForce((Vector3.zero-ob.transform.position).normalized * enemySpeed * Time.smoothDeltaTime);
            //rb.AddTorque((Vector3.zero - ob.transform.position));

            if (spawnCount%8==0) {
                if (difficultyLevel <= 14)
                {
                    enemySpeed *= difficultyMod;
                    spawnPeriod /= difficultyMod;
                    difficultyLevel++;
                }
            }

        }

    }


    private Vector3 RandomPointOnCircleEdge(float radius)
    {
        Vector2 random = new Vector2();
        while (Mathf.Abs(random.y) < 4f) // +-30 degrees
        {
            random = Random.insideUnitCircle.normalized * radius;
        }
        return new Vector3(random.x, random.y, 0f);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(Vector3.zero, spawnRadius);
        Vector3 to = Quaternion.AngleAxis(30.0f, Vector3.forward) * (Vector3.right * spawnRadius);
        Gizmos.DrawLine(Vector3.zero, to);
        Vector3 to1 = Quaternion.AngleAxis(-30.0f, Vector3.forward) * (Vector3.right * spawnRadius);
        Gizmos.DrawLine(Vector3.zero, to1);
        Vector3 to2 = Quaternion.AngleAxis(150.0f, Vector3.forward) * (Vector3.right * spawnRadius);
        Gizmos.DrawLine(Vector3.zero, to2);
        Vector3 to3 = Quaternion.AngleAxis(-150.0f, Vector3.forward) * (Vector3.right * spawnRadius);
        Gizmos.DrawLine(Vector3.zero, to3);
    }

}
