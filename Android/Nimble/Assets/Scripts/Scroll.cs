using UnityEngine;
using System.Collections;

public class Scroll : MonoBehaviour {

    public void Open() {
        this.gameObject.SetActive(true);
    }

    public void Close()
    {
        this.gameObject.SetActive(false);
    }
}
