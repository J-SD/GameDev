using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelMainMenu : LevelManager
{
    public int levelFromSave;
    public int collectablesFromSave;
    public GameObject continueObject;
    public TextMeshProUGUI restartText;
    public Transform camTar;


    public override void Awake() {
        base.Awake();
        level = 0;
        camSize = 8f;
    }
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        cam.SetTarget(camTar);

        PlayerData data = SaveSystem.Load();
        if (data != null)
        {
            if (data.level > 0)
            {
                EnableContinue();
                levelFromSave = data.level;
                collectablesFromSave = data.collectables;
            }
            else DisableContinue();
        }
        else DisableContinue();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }

    void EnableContinue() {
        continueObject.SetActive(true);
    }
    void DisableContinue() {
        continueObject.SetActive(false);
        restartText.text = "Start";
        SaveSystem.Save();
    }

    public void Continue() {
        GameManager.Instance.collectables = collectablesFromSave;
        GameManager.Instance.LoadLevelCaller(levelFromSave);

    }
    public void Restart() {
        GameManager.Instance.collectables = 0;
        GameManager.Instance.LoadLevelCaller(2);
    }
    public void Tutorial() {
        GameManager.Instance.LoadLevelCaller(1);
    }
    public void Quit() {
        Application.Quit();
    }


}
