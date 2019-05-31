using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Mod", menuName ="Mod")]
public class Modifier: ScriptableObject
{  
    [SerializeField]
    private string name;
    [SerializeField]
    private string stat;
    [SerializeField]
    private float mod;

    public void init(string name, string stat, float mod)
    {
        this.name = name;
        this.stat = stat;
        this.mod = mod;
    }

    public string Name { get => name; set => name = value; }
    public string Stat { get => stat; set => stat = value; }
    public float Mod { get => mod; set => mod = value; }
}
