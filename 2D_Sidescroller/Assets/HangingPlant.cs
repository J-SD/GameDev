using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HangingPlant : MonoBehaviour
{
    public LayerMask playerMask;

    Animator anim;

    public float offset;

    // Start is called before the first frame update
    void Awake()
    {
        anim = GetComponent<Animator>();
        
    }
    private void Start()
    {
        offset = Random.Range(0f, 1f);
        anim.SetFloat("Offset", offset);
    }
    private void OnTriggerEnter2D(Collider2D c)
    {
        if ((playerMask.value & 1 << c.gameObject.layer) == 1 << c.gameObject.layer) {
            anim.SetTrigger("Move");
        }


    }
}
