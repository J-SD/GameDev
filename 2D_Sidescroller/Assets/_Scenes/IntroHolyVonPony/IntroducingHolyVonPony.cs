using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.SceneManagement;


public class IntroducingHolyVonPony : LevelManager
{
    public Transform playerTrigger;
    public Transform hvp;
    public CinemachineVirtualCamera hvpCam;
    bool afterTrigger;
    bool firstTime = true;
    public override void Awake()
    {
        base.Awake();
        level = SceneManager.GetActiveScene().buildIndex;

    }

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        if (hvp.position.x > playerTrigger.position.x && !afterTrigger) HvpAfterTrigger();
    
    }

    void HvpAfterTrigger() {
        afterTrigger = true;
        hvpCam.enabled = false;
    }

    public override void NotifyOfPlayerRespawn()
    {
        base.NotifyOfPlayerRespawn();
        if (firstTime) { firstTime = false; return; }

        hvp.gameObject.GetComponent<HolyvonPony>().Reset();

    }
}
