using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Rigidbody2D rigidbody;


    private Vector3 targetPos;
   
    private float startTime;
    private ProjectileModifiers mods;

    public Projectile(Vector3 tarPos)
    {
        this.targetPos = tarPos;
    }

    public virtual void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        mods = Player.Instance.GetProMods();

        if (mods.tarMode == ProjectileModifiers.TarModes.mouse) {
            Vector2 cursorInWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            targetPos = cursorInWorldPos;
        }


        startTime = Time.time;
    }

    // Start is called before the first frame update
    public virtual void Start()
    {
        Vector2 direction = targetPos - transform.position;
        direction.Normalize();
        rigidbody.velocity = direction * mods.force;
    }

    // Update is called once per frame
    public virtual void Update()
    {

        if (Time.time > startTime + mods.duration) {
            Destroy(this.gameObject);
        }
        
    }
}
