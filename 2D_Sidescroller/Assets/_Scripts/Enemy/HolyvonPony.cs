using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class HolyvonPony : Enemy
{
    private Transform target;
    private Rigidbody2D rb;
    private CinemachineVirtualCamera vcam;
    public Transform restartLocation;
    public Transform endLocation;


    Vector2 moveToPosition;
    Vector3 lastPosition;


    float currentSpeed;

    private void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
    }
    // Update is called once per frame
    void Update()
    {
        base.Update();
        if (!target) target = Player.Instance.transform;
        if (transform.position.x > endLocation.position.x) { anim.SetFloat("Speed", 0f); return; }

        if (target.position.x > transform.position.x ) moveToPosition = new Vector2(target.position.x, transform.position.y);
        else moveToPosition = new Vector2(transform.position.x, transform.position.y);

        transform.position = Vector2.MoveTowards(transform.position, moveToPosition, speed*Time.deltaTime);


        currentSpeed = (transform.position - lastPosition).magnitude / Time.deltaTime;
        anim.SetFloat("Speed", currentSpeed);

        lastPosition = transform.position;

    }

    // we also need to check if the player fell to death to restart hvp!
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.Equals(Player.Instance.gameObject)) {
            StartCoroutine(HitPlayer());
        }
    }

    IEnumerator HitPlayer() {
        Player.Instance.Die();
        yield return new WaitForSeconds(3f);
        Reset();
    }

    public void Reset()
    {
        transform.position = restartLocation.position;
        transform.rotation = Quaternion.identity;
    }
}
