using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileModifiers
{
    public List<Modifier> mods = new List<Modifier>();
    public List<Modifier> appliedMods = new List<Modifier>();

    public float rate = 1f;
    public float duration = 1f;
    public float force = 15f;
    public float damage = 1f;

    public float baseRate = 1f;
    public float baseDuration = 1f;
    public float baseForce = 15f;
    public float baseDamage = 1f;


    public TarModes tarMode;



    public enum TarModes
    {
        mouse
    }

    public ProjectileModifiers()
    {
        Modifier so = ScriptableObject.CreateInstance<Modifier>();
        so.init("CurrentRateMod", "rate", 1f);
        appliedMods.Add(so);

        so = ScriptableObject.CreateInstance<Modifier>();
        so.init("CurrentDurationMod", "duration", 1f);
        appliedMods.Add(so);

        so = ScriptableObject.CreateInstance<Modifier>();
        so.init("CurrentForceMod", "force", 1f);
        appliedMods.Add(so);

        so = ScriptableObject.CreateInstance<Modifier>();
        so.init("CurrentDamageMod", "damage", 1f);
        appliedMods.Add(so);

        so = ScriptableObject.CreateInstance<Modifier>();
        so.init("CurrentKnockbackMod", "knockback", 1f);
        appliedMods.Add(so);

    }

    public void AddMod(Modifier mod) {
        mods.Add(mod);
        ResetMods();
        foreach (Modifier m in mods) {
            foreach (Modifier am in appliedMods) {
                if (am.Stat == m.Stat) am.Mod *= m.Mod;
                    
            }
        }
    }

    private void ResetMods() {
        foreach (Modifier am in appliedMods) {
            am.Mod = 1f;
        }
    }

    public float GetModValue(string stat) {
        foreach (Modifier am in appliedMods)
        {
            if (am.Stat == stat) return am.Mod;
        }
        //didnt find stat
        return 1f;
    }

}
