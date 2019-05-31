using UnityEngine;
using System.Collections;

public class FadeOut : MonoBehaviour {
    public Sprite testLoad;
	// Use this for initialization
	void Awake () {
        StartCoroutine(fade());
	}

    IEnumerator fade() {
        yield return new WaitForSeconds(2);
        SpriteRenderer sRenderer = GetComponent<SpriteRenderer>();
        if (sRenderer.sprite == testLoad) {
            yield return new WaitForSeconds(1);
        }
        for (float f = 1f; f >= 0; f -= 0.01f)
        {
            
            Color c = sRenderer.material.color;
            c.a = f;
            GetComponent<Renderer>().material.color = c;
            yield return null;
        }
        Destroy(gameObject);

    }

}
