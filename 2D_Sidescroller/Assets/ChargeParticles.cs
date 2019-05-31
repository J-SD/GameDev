using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeParticles : MonoBehaviour
{
    public LayerMask enemyMask;
    // Start is called before the first frame update
    void Awake()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D c)
    {
        if ((enemyMask.value & 1 << c.gameObject.layer) == 1 << c.gameObject.layer) {
            if (c.gameObject.GetComponent<Enemy>().defeatable) {
                c.gameObject.GetComponent<Enemy>().Defeat();
            }
        }


    }
}
