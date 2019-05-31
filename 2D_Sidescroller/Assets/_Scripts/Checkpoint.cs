using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public Transform checkpointSpawn;
    private Vector3 checkpointSpawnLocation;
    private void Awake()
    {
        checkpointSpawnLocation = checkpointSpawn.transform.position;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>().playerSpawn = checkpointSpawnLocation;  
    }
}
