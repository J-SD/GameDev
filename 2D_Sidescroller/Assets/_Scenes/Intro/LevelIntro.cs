using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class LevelIntro : LevelManager
{
    public GameObject tree;
    public AudioClip fallClip;
    private AudioSource source;
    private float endPosition;


    // Start is called before the first frame update
    public override void Awake()
    {
        base.Awake();
        source = GameManager.Instance.GetComponent<AudioSource>();
        endPosition = tree.transform.position.x - 10f;
        cameraFollowsPath = false;
        level = SceneManager.GetActiveScene().buildIndex;

    }

    public override void Start()
    {
        base.Start();
        player.canDash = false;
        source.pitch = 0f;

        //cam.GetComponentInChildren<SpriteRenderer>().enabled = false; // background
    }
    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        if (player)
        {
            float p = Mathf.Clamp((player.transform.position.x) / endPosition, 0, 1);
            source.pitch = p;
        }
    }

    public void TakeApple() {
        player.canDash = true;
        AudioSource audioSource = GameManager.Instance.GetComponent<AudioSource>();
        audioSource.clip = fallClip;
        audioSource.Play();

        player.anim.SetLayerWeight(2,1);
        player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        player.anim.SetTrigger("TakeApple");
        tree.GetComponent<Animator>().SetTrigger("AppleTaken");
        player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        player.dashing = true;
        MainCamera.Instance.ResetCam();

    }
}
