using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawnPoint : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject cameraPrefab;
    // Start is called before the first frame update
    void Start()
    {


        if (!GameManager.Instance) // no gamemanager in this scene, make a new player and camera (this is for test levels, all real scenes will have GM)
        {
            Instantiate(playerPrefab, transform.position, Quaternion.identity);
            Instantiate(cameraPrefab, transform.position + Vector3.back * 5f, Quaternion.identity);

        }
        else if (Player.Instance)
        {
            Player.Instance.transform.position = transform.position;
            Player.Instance.lastSpawnPoint = transform.position;
            Player.Instance.Reset();
        }
        else {
            Instantiate(playerPrefab, transform.position, Quaternion.identity);
            Player.Instance.transform.position = transform.position;
            Player.Instance.lastSpawnPoint = transform.position;
            Player.Instance.Reset();
        }
    }
}
