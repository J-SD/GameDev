using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Angel : Enemy
{
    public float rotateSpeed = 5f;
    public float fireRate = 1.5f;
    public GameObject shot;
    public Transform shotLocationR;
    public Transform shotLocationL;
    public float shotSpeed;
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        //transform.forward = new Vector3(1f,0f,0f);
    }

    private float lastZRot = 0f;
    private float lastShotTime = 0f;
    public override void Update()
    {
        base.Update();

        Collider2D playerCol = PlayerColInRange();
        if (playerCol) {
            
            Vector3 vectorToTarget = playerCol.transform.position - transform.position;
            float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
            ////Vector3 dir;
            ////if (facingRight) dir = Vector3.forward;
            //else dir = -Vector3.forward + Vector3.forward * 45f;
            Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * rotateSpeed);

            //if ((Mathf.Abs(transform.rotation.z) > .7f && !(Mathf.Abs(lastZRot) > .7f)) || (Mathf.Abs(transform.rotation.z) < .7f && !(Mathf.Abs(lastZRot) < .7f))) spriteRenderer.flipX = true;
            if (facingRight && playerCol.transform.position.x < transform.position.x) { Flip(); spriteRenderer.flipY = true; spriteRenderer.flipX = true; }
            else if (!facingRight && playerCol.transform.position.x > transform.position.x) { Flip(); spriteRenderer.flipY = false; spriteRenderer.flipX = false; }
            lastZRot = transform.rotation.z;
            float currentDeviationFromPlayer =Mathf.Abs(1 - 180 / Vector3.Angle(transform.right, transform.position - playerCol.transform.position));
            if (Time.time > lastShotTime + fireRate && currentDeviationFromPlayer<.2f)
            {
                Shoot();
                lastShotTime = Time.time;
            }

        }
    }

    private void Shoot()
    {
        anim.SetTrigger("Shoot");
        //go.GetComponent<Rigidbody2D>().velocity = new Vector2(-5f,-5f);

    }
    public void SpawnShot() {
        GameObject go;

        if (facingRight) go = GameObject.Instantiate(shot, shotLocationR);
        else
        {
            go = GameObject.Instantiate(shot, shotLocationL);
            go.GetComponent<SpriteRenderer>().flipX = true;
        }
        go.GetComponent<AngelShot>().Create(shotSpeed, playerMask);

    }

    public override void Die() {
        base.Die();
        // maybe particles?
        Destroy(gameObject);
    }

}
