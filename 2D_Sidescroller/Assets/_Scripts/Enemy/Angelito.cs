using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Angelito : Enemy
{
    bool stopped = false;
    public override void Update()
    {
        base.Update();

        Collider2D playerCol = PlayerColInRange();

        if (playerCol && !stopped) {
            if (!Physics.Linecast(transform.position, playerCol.transform.position))
            {
                transform.position = Vector2.MoveTowards(transform.position, playerCol.transform.position, speed * Time.deltaTime);

            }

           
        }

    }

    private void OnCollisionStay2D (Collision2D c)
    {
        if ((playerMask.value & 1 << c.gameObject.layer) == 1 << c.gameObject.layer)
            stopped = true;

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        if (PlayerColInRange())
        {

            //Gizmos.DrawRay(transform.position, (PlayerColInRange().transform.position - transform.position));
        }

    }
}
