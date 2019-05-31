using UnityEngine;
using System.Collections;

public class LoadOnClick : MonoBehaviour
{

    public GameObject loadingImage;

    public void LoadScene(int level)
    {
        StartCoroutine(FadeToScene(level));
       
    }

    IEnumerator FadeToScene(int level) {
        float fadeTime = GameObject.Find("Game_Master").GetComponent<Fading>().BeginFade(1);
        yield return new WaitForSeconds(fadeTime);
        Application.LoadLevel(level);

    }
}