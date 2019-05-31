using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    public GameObject projectile;


    private float rateBase = 2f;

    private float lastShotTime = 0f;
    private ProjectileModifiers mods;

    // Start is called before the first frame update
    void Start()
    {
        mods = Player.Instance.GetProMods();
    }

    // Update is called once per frame
    void Update()
    {
        //projectile
        if (Input.GetButton("Fire"))
        {
            if (Time.time > lastShotTime + (rateBase/mods.GetModValue("rate")))
            {
                GameObject newProjectile= GameObject.Instantiate(projectile, transform.position, Quaternion.identity);
                lastShotTime = Time.time;
            }

        }
    }
}
