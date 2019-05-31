using UnityEngine;
using System.IO;

[System.Serializable]
public class PlayerData
{
    public int level;
    public int collectables;
    public PlayerData() {
        level = GameManager.Instance.level;
        collectables = GameManager.Instance.collectables;
    }

   
}
