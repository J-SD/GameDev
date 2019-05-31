using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyProjectile : MonoBehaviour
{
    public LayerMask playerMask;
    public LayerMask destroyMask;
    public float speed;
    public float damage;
    public float createTime;
    public float destroyTime = 3f;
    public Rigidbody2D rb;



    virtual public void Awake() { }

    // Start is called before the first frame update
    virtual public void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    virtual public void Update()
    {

    }
    virtual public void Create(float s, LayerMask mask)
    {
        speed = s;
        transform.parent = null;
        playerMask = mask;
        createTime = Time.time;
    }

    virtual public void OnTriggerEnter2D(Collider2D c)
    {
        if ((playerMask.value & 1 << c.gameObject.layer) == 1 << c.gameObject.layer)
        {
            GameManager.Instance.PlayerDamage(damage);
            Destroy(gameObject);
        }
        else if ((destroyMask.value & 1 << c.gameObject.layer) == 1 << c.gameObject.layer) {
            Destroy(gameObject);
        }
    }

    

    // Update is called once per frame
    virtual public void FixedUpdate()
    {
        rb.AddForce(transform.right * speed * Time.fixedDeltaTime);

        if (Time.time > (createTime + destroyTime)) Destroy(gameObject);

    }

}
