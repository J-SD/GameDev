using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class DeathOnTouch : MonoBehaviour
{
    Collider2D col;
    public LayerMask playerMask;
    //public bool defeatable;

    private void OnCollisionEnter2D(Collision2D c)
    {
        if ((playerMask.value & 1 << c.gameObject.layer) == 1 << c.gameObject.layer)
        {
            //if (defeatable && c.gameObject.GetComponent<Player>().dashing) {
            //    Destroy(gameObject, .1f);
            //}
            //else c.gameObject.GetComponent<Player>().Die();
            c.gameObject.GetComponent<Player>().Die();

        }

    }

    private void OnTriggerEnter2D(Collider2D c)
    {
        if ((playerMask.value & 1 << c.gameObject.layer) == 1 << c.gameObject.layer)
        {
            //if (defeatable && c.gameObject.GetComponent<Player>().dashing) {
            //    Destroy(gameObject, .1f);
            //}
            //else c.gameObject.GetComponent<Player>().Die();
            c.gameObject.GetComponent<Player>().Die();

        }

    }
}
