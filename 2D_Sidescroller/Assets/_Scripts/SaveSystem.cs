using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveSystem
{
    public static void Save() {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/save.jax";
        FileStream stream = new FileStream(path, FileMode.Create);
        PlayerData data = new PlayerData(); // get data
        formatter.Serialize(stream, data);
        stream.Close();


    }


    public static PlayerData Load() {
        string path = Application.persistentDataPath + "/save.jax";


        if (File.Exists(path)) {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            PlayerData data = formatter.Deserialize(stream) as PlayerData;
            stream.Close();
            return data;

        }
        else {Debug.Log("No save file found at " + path); return null; }

    }
}
