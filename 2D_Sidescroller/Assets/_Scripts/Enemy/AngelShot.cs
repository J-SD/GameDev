using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngelShot : EnemyProjectile
{

    override public void Awake()
    {
        base.Awake();
        createTime = Time.time;
        damage = 10f;
    }

    
  
   

}
