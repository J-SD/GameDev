using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifierPickup : MonoBehaviour
{
    [SerializeField]
    LayerMask playerMask;

    [SerializeField]
    private Modifier mod;

    private void OnTriggerEnter2D(Collider2D c)
    {
        if ((playerMask.value & 1 << c.gameObject.layer) == 1 << c.gameObject.layer)
            {
                Player.Instance.GetProMods().AddMod(mod);
                Destroy(gameObject);
            }
    }
}
