using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour
{
    public LayerMask playerMask;
    public bool randomizeFlip;

    Animator anim;

    public float offset;

    // Start is called before the first frame update
    void Awake()
    {
        anim = GetComponent<Animator>();
        if (randomizeFlip) {
            if (Random.Range(0f, 1f) < .5f) {
                Flip();
            }
        }
        
    }
    private void Start()
    {
        offset = Random.Range(0f, 1f);
        anim.SetFloat("Offset", offset);
        transform.rotation = Quaternion.identity;
        transform.localRotation = Quaternion.identity;
        float scaleFactor = Random.Range(.7f, 1f);
        transform.localScale = new Vector3(scaleFactor, scaleFactor);
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y-(1/scaleFactor)/6);
    }



    private void OnTriggerEnter2D(Collider2D c)
    {
    
        if ((playerMask.value & 1 << c.gameObject.layer) == 1 << c.gameObject.layer) {
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Move"))
            {
                anim.SetTrigger("Move");
            }
        }


    }

    void Flip()
    {
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

}
