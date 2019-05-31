using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEnd : LevelManager
{
    public override void Awake()
    {
        base.Awake();
        level = -1;
        camSize = 8f;
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
