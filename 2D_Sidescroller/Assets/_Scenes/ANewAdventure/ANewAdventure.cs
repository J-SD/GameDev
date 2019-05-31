using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Cinemachine;

public class ANewAdventure : LevelManager
{
    public TextMeshPro title;
    bool titleRevealed = true;
    public CinemachineVirtualCamera fallVcam;

    public override void Awake()
    {
        base.Awake();
        title.maxVisibleCharacters = 0;
        level = SceneManager.GetActiveScene().buildIndex;

    }
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        cameraFollowsPath = false;
        player.canDie = false;
        title.GetComponent<RevealText>().StartReveal(.8f, .1f);
        player.canDie = false;
        player.dead = true;
        title.transform.parent.GetComponent<Canvas>().worldCamera = MainCamera.Instance.mainCam;

        MainCamera.Instance.vcam.enabled = false;
        fallVcam.Follow = player.camTarget.transform;
        player.lastSpawnPoint = Vector3.zero;
        //MainCamera.Instance.ChangeSize(13);
        //cam.SetSmoothSpeed(cam.smoothSpeed / 10f);



    }
    bool pastDrop = false;
    public override void Update()
    {
        base.Update();

        if (player && player.transform.position.y < -1f && titleRevealed)
        {
            titleRevealed = false;
            title.GetComponent<RevealText>().StartUnReveal(.08f);
            player.dead = false;
            if (!player.canDash) player.canDash = true;

            MainCamera.Instance.vcam.enabled = true;
            fallVcam.enabled = false;
            

            //MainCamera.Instance.ChangeSize(8);

        }

        if (cam.transform.position.y < 1f && !didTitle) AfterTitle();

        //if (!pastDrop && player.transform.position.x < 2f)
        //{
        //    player.canDie = true;
        //    pastDrop = true;
        //}
    }


    bool didTitle = false;
    private void AfterTitle()
    {
        didTitle = true;
        player.canDie = true;
        //cam.RevertSmoothSpeed();
    }

}
