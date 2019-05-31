using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public RawImage GameOverImage;
    public RawImage GameOverBackground;


    private void Start()
    {
        GameOverImage.color = new Color(GameOverImage.color.r, GameOverImage.color.g, GameOverImage.color.b, 0f);
        GameOverBackground.color = new Color(GameOverBackground.color.r, GameOverBackground.color.g, GameOverBackground.color.b, 0f);
        GameOverBackground.gameObject.GetComponent<Button>().interactable = false;

    }

    public void GameOver() {

        GameOverFade();
    }

    public void Restart() {
        print("Got click");
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    void GameOverFade()
    {
        StartCoroutine("FadeInGameOver");
    }

    IEnumerator FadeInGameOver()
    {

        for (float i = 0f; i <= 1.1f; i += .05f)
        {
            GameOverImage.color = new Color(GameOverImage.color.r, GameOverImage.color.g, GameOverImage.color.b, i);
            yield return new WaitForSeconds(.05f);

        }
        for (float i =0f; i <= 1.1f; i += .05f)
        {
            GameOverBackground.color = new Color(GameOverBackground.color.r, GameOverBackground.color.g, GameOverBackground.color.b, i);
            yield return new WaitForSeconds(.02f);
        }

        foreach (Button b in GameOverBackground.transform.parent.GetComponentsInChildren<Button>()) {
            b.gameObject.SetActive(false);
        }
        GameOverBackground.gameObject.GetComponent<Button>().interactable = true;
        GameOverBackground.gameObject.SetActive(true);
        GameOverBackground.raycastTarget = true;

    }
}
