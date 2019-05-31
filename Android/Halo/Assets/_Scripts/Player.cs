using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //public float defaultMoveSpeed = 600f;
    public float moveSpeed = 450f;

    private float movement = 0f;

    private MeshCollider detectionCollider;
    private MeshCollider col;


    public bool dragControls = true;

    Vector3 lastPos;
    Vector3 touchAnchor;

    private void Start()
    {
        detectionCollider = GetComponentInChildren<MeshCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        //movement = Input.GetaxisRaw("Horizontal");
        if (dragControls)
        {
            moveSpeed = 3f;
            if (Input.touchCount > 0)
            {
                Touch t = Input.GetTouch(0);
                Vector3 tp = t.position;
                if (t.phase == TouchPhase.Began)
                {
                    touchAnchor = tp;
                }
                else if (t.phase == TouchPhase.Ended)
                {
                    movement = 0f;
                }
                else
                {
                    //movement = (lastPos - tp).normalized.y/1.5f;
                    //lastPos = t.position;
                    movement = t.deltaPosition.y * 10f;

                    //if (movement >=1.5f) { movement = 1.5f; }
                    //if (movement <=-1.5f) { movement = -1.5f; }


                }
            }
        }
        else {
            moveSpeed = 600f;
            if (Input.GetAxisRaw("Horizontal")!=0f) {
                movement = Input.GetAxisRaw("Horizontal");
            }
        }

    }

    private void FixedUpdate()
    {
        
        transform.RotateAround(Vector3.zero, Vector3.forward, movement * Time.fixedDeltaTime * -moveSpeed); ;
    }

    void OnTriggerEnter(Collider otherCol)
    {
        if (otherCol.gameObject.tag == "Enemy")
        {
            float origZ = otherCol.transform.position.z;
            otherCol.GetComponent<Enemy>().DelayedDestroy();
            otherCol.gameObject.tag = "Untagged";

            //Vector3 force = (otherCol.transform.position - transform.position).normalized;
            Vector3 force = (transform.forward).normalized;



            otherCol.transform.Rotate(Vector3.right, 180f);

            //Vector3 force = (gameObject.transform.forward).normalized;

            float magnitude = 50f;
            otherCol.gameObject.GetComponent<Rigidbody>().AddForce(force * magnitude);
            otherCol.transform.position = transform.position + Vector3.forward * 2f;
            otherCol.transform.position = new Vector3(otherCol.transform.position.x, otherCol.transform.position.y, origZ);
            otherCol.gameObject.GetComponent<Rigidbody>().AddTorque(new Vector3(1f,1f,1f));



            //Vector3 opposite = -col.GetComponent<Rigidbody>().velocity;
            //Vector3 oppForce = opposite.normalized * 1000f;
            //col.GetComponent<Rigidbody>().AddForce(oppForce * Time.deltaTime);
            //col.GetComponent<Rigidbody>().AddForce(-(gameObject.transform.position + col.gameObject.transform.position).normalized * 1000f * Time.smoothDeltaTime);

        }


    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, transform.forward * 100f);
    }

    public void Right()
    {
        if (!dragControls)
        {
            movement = 1f;
        }
    }

    public void Left()
    {
        if (!dragControls)
        {
            movement = -1f;
        }
    }

    public void Up()
    {
        if (!dragControls)
        {
            movement = 0f;
        }
    }

    public void ToggleControls() {
        dragControls = !dragControls;
    }
}
