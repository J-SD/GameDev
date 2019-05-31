using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireProjectile : EnemyProjectile
{
    Vector3 startPlayerLocation;
    Vector3 aimDirection;
    public float shotForce;

    // Start is called before the first frame update
    public virtual void Start()
    {
        base.Start();
        destroyTime = 1.5f;
        startPlayerLocation = (Player.Instance.transform.position + Vector3.up * 2f);
         aimDirection = new Vector3(Player.Instance.transform.position.x - transform.position.x, 1f).normalized;

        rb.AddForce(aimDirection * shotForce);
    }

    // Update is called once per frame
    public virtual void Update()
    {
        base.Update();
    }

    public virtual void FixedUpdate() {
        base.FixedUpdate();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, aimDirection);
    }
}
