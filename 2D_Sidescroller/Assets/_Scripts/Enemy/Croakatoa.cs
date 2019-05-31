using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Croakatoa : Enemy
{
    public GameObject fire;
    public float shotSpeed = 2f;
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
        if (Random.Range(1,150) == 137f) {
            anim.SetTrigger("Fire");

        }

    }

    public void Fire() {

        Collider2D playerCol = PlayerColInRange();

        if (playerCol)
        {
            GameObject go = Instantiate(fire, transform.position, Quaternion.identity);
            go.GetComponent<FireProjectile>().Create(shotSpeed, playerMask);
            audioSource.Play();
                       
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, awareRadius);
    }
}
