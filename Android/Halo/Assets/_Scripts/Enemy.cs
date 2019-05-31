using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    private float destroyTime;
    public float timeToDestroy = 2f;
    public float despawnTime = 5f;

    private void Start()
    {
        destroyTime = Time.time + despawnTime;
    }


    public void DelayedDestroy() {
        destroyTime = Time.time + timeToDestroy;
    }

    private void Update()
    {
        if (Time.time > destroyTime) {
            Destroy(gameObject);
        }
    }
}
