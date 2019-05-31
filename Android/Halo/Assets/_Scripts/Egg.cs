using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Egg : MonoBehaviour
{
    public int health = 100;
    public Material[] crackMats = new Material[4];
    public Fade hurtTextureFade;
    public GameManager gm;

    private bool dead = false;
    private MeshRenderer renderer;
    private MeshCollider detectionCollider;

    private void Start()
    {
        detectionCollider = GetComponentInChildren<MeshCollider>();
        renderer = GetComponent<MeshRenderer>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy" && !dead)
        {
            Destroy(other.gameObject);
            hurtTextureFade.DoFade();
            TakeDamage();
        }

    }

    public void TakeDamage() {
        health -= 10;
        int crackMatNumber = health / 25;
        if (75 <= health && health < 100) {
            renderer.material = crackMats[0];
        }
        else if (50 <= health && health < 75) {
            renderer.material = crackMats[1];
        }
        else if (25 <= health && health < 50) {
            renderer.material = crackMats[2];
        }
        else if (0 <= health && health < 25) {
            renderer.material = crackMats[3];
        }

        

        if (health<=0) {
            gm.GameOver();
            dead = true;
        }
    }

}
