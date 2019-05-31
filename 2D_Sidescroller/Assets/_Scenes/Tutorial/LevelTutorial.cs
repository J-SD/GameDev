using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTutorial : LevelManager
{
    public override void Awake()
    {
        base.Awake();
        level = -1;

    }

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        GameManager.Instance.collectables = 20;
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }
}
