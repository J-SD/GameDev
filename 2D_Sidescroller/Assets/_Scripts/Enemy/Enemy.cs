using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float health;

    public float awareRadius = 7f;

    public LayerMask playerMask;
    public float speed;
    public GameObject deathEffect;
    public AudioSource audioSource;


    protected bool facingRight = true;

    protected SpriteRenderer spriteRenderer;
    protected Animator anim;

    public bool defeatable;

    public virtual void Awake() {
        audioSource = GetComponent<AudioSource>();
    }


    // Start is called before the first frame update
    public virtual void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    public virtual void Update()
    {
        
    }

    public virtual void Defeat()
    {
        //todo spawn particles!!
        Instantiate(deathEffect, transform.position, Quaternion.identity);
        AudioSource.PlayClipAtPoint(audioSource.clip, transform.position);
        Destroy(gameObject);
    }


    public virtual void Flip()
    {
        // Switch the way the player is labelled as facing
        facingRight = !facingRight;

        //Multiply the player's x local cale by -1
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
        foreach (SpriteRenderer sr in GetComponentsInChildren<SpriteRenderer>()) {
            sr.flipX = !sr.flipX;
            sr.flipY = !sr.flipY;

        }

    }

    public virtual void Die() {

    }

    public virtual Collider2D PlayerColInRange() {
        return Physics2D.OverlapCircle(transform.position, awareRadius, playerMask);
    }


}
