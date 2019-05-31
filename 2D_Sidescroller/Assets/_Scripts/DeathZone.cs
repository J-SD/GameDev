using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    public LayerMask playerMask;
    private void OnTriggerEnter2D(Collider2D c)
    {
        print(GameManager.Instance);
        if ((playerMask.value & 1 << c.gameObject.layer) == 1 << c.gameObject.layer)
            GameManager.Instance.KillPlayer();
    }
}
