﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UntitledA : LevelManager
{
    public override void Awake()
    {
        base.Awake();
        level = SceneManager.GetActiveScene().buildIndex;
        camSize = 8;


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
    }
}
