using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    public LayerMask playerMask;
    private AudioSource audioSource;
    // Start is called before the first frame update
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D c)
    {
        if ((playerMask.value & 1 << c.gameObject.layer) == 1 << c.gameObject.layer)
        {
            GameManager.Instance.AddCollectable();
            AudioSource.PlayClipAtPoint(audioSource.clip, MainCamera.Instance.transform.position);
            //effect
            Destroy(gameObject);
        }
    }

}
