using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextTrigger : MonoBehaviour
{
    private RevealText textReveal;
    public LayerMask mask;

    private void Start()
    {
        textReveal = GetComponentInChildren<RevealText>();
        textReveal.gameObject.SetActive(false);

    }

    private void OnTriggerEnter2D(Collider2D c)
    {
        if ((mask.value & 1 << c.gameObject.layer) == 1 << c.gameObject.layer) { 
            textReveal.gameObject.SetActive(true);
            textReveal.StartReveal(0f, 0.05f);

        } 
    }

}
