using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class Interactable : MonoBehaviour
{
    public string textToShow;
    public LayerMask mask;
    public UnityEvent eventToCall;
    public TextMeshPro textField;
    public float interactRadius;
    // Start is called before the first frame update
    void Start()
    {
        textField = GetComponentInChildren<TextMeshPro>();
        textField.enabled = false;
        textField.text = textToShow;

    }

    // Update is called once per frame
    void Update()
    {
        if (Physics2D.OverlapCircle(transform.position, interactRadius, mask))
        {
            textField.enabled = true;
            if (Input.GetButton("Interact"))
            {
                eventToCall.Invoke();
                Destroy(this.gameObject);
            }
        }
        else {
            textField.enabled = false;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactRadius);

    }

}

