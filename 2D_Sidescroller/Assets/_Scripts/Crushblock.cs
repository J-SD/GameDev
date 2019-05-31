using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crushblock : MonoBehaviour
{
    [Range(0f,1f)]
    public float offset;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Animator>().SetFloat("Offset", offset);
    }

}
