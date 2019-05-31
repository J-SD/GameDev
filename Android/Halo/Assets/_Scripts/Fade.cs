using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{
    private RawImage image;


    private void Start()
    {
        image = GetComponent<RawImage>();
        image.color = new Color(image.color.r, image.color.g, image.color.b, 0f);
    }
    public void DoFade() {
        StartCoroutine("HurtFade");
    }

    IEnumerator HurtFade()
    {

        for (float i = 0f; i<=.24; i+=.1f) {
            image.color = new Color(image.color.r, image.color.g, image.color.b, i);
            yield return new WaitForSeconds(.07f);

        }
        for (float i = .24f; i >= -.1f; i-=.1f)
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, i);
            yield return new WaitForSeconds(.07f);

        }


    }
}
