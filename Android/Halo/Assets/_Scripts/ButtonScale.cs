using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonScale : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        foreach (RectTransform rt in GetComponentsInChildren<RectTransform>()) {
            if (rt.gameObject.GetComponent<Button>()) {
                rt.sizeDelta = new Vector2(Screen.width / 2, Screen.height);
            }
        }
    }

}
