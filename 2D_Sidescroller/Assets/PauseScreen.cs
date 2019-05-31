using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseScreen : MonoBehaviour
{
    public GameObject pauseCanvas;
    public GameObject upgradeCanvas;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void Pause (){
        pauseCanvas.SetActive(true);
    }
    public void UnPause() {
        pauseCanvas.SetActive(false);
        upgradeCanvas.SetActive(false);

    }
}
