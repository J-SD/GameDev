using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTrigger : MonoBehaviour
{
    public enum levelType {
        thisLevel=-2,
        nextLevel=-1,
        other=-3
        
    }

    public levelType level;
    public LayerMask playerMask;
    public int otherLevel;
    int levelToLoad;
    int curBuildIndex;

    bool doing = false;
    private void OnTriggerEnter2D(Collider2D c)
    {
        doing = true;
        curBuildIndex = SceneManager.GetActiveScene().buildIndex;
        switch (level)
        {
            case levelType.nextLevel:
                levelToLoad = curBuildIndex + 1;
                break;
            case levelType.thisLevel:
                levelToLoad = curBuildIndex;
                break;
            case levelType.other:
                levelToLoad = otherLevel;
                break;

            default:
                levelToLoad = curBuildIndex;
                break;
        }
        
        if ((playerMask.value & 1 << c.gameObject.layer) == 1 << c.gameObject.layer)
            GameManager.Instance.LoadLevelCaller(levelToLoad);

    }

    private void OnTriggerStay2D(Collider2D c)
    {
        if (doing) return;
        doing = true;
        curBuildIndex = SceneManager.GetActiveScene().buildIndex;
        switch (level)
        {
            case levelType.nextLevel:
                levelToLoad = curBuildIndex + 1;
                break;
            case levelType.thisLevel:
                levelToLoad = curBuildIndex;
                break;
            case levelType.other:
                levelToLoad = otherLevel;
                break;

            default:
                levelToLoad = curBuildIndex;
                break;
        }

        if ((playerMask.value & 1 << c.gameObject.layer) == 1 << c.gameObject.layer)
            GameManager.Instance.LoadLevelCaller(levelToLoad);
    }
}
