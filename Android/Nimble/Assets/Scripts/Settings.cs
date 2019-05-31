using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Settings : MonoBehaviour {
    public Toggle musicToggle;
    public bool musicOn;
    bool pressed = true;
    public Text musicToggleText;
    public Text winStreak;
    AudioSource musicSource;

   

    public void MusicToggleChanged() {
        if (pressed) pressed = false;
        else if (!pressed) pressed = true;

        if (musicOn)
        {
            musicOn = false;
            musicToggleText.text = "Off";
        }
        else if (!musicOn) {
            musicOn = true;
            musicToggleText.text = "On";
        }

        musicSource.mute = !musicOn;
        Game.current.music = musicOn;
        SaveLoad.Save();
    }


	// Use this for initialization
	void Start () {
        musicSource = GameObject.FindGameObjectWithTag("game_master").GetComponent<AudioSource>();

        musicOn = Game.current.music;
        winStreak.text = Game.current.highestWinStreak.ToString();

        if (musicOn) musicToggleText.text = "On";
        else musicToggleText.text = "Off";
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
