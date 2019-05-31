using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InstText : MonoBehaviour {
    private Animator anim;
    private Text text;
    private int index = 0;

    private List<string> texts = new List<string>();

    void Awake() {
        anim = GetComponent<Animator>();
        text = GetComponent<Text>();
        texts.Add("1. Take turns sliding one coin at a time along the board to the left");
        texts.Add("2. Coins may be removed from the board by sliding them all the way to the left");
        texts.Add("3. Coins may not pass other coins or share the same box");
        texts.Add("4. The player who slides the last coin off the board wins!");
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Next() {
        if (index +1 < texts.Count) {
            index++;
            UpdateText();
        }
    }


    public void Previous() {
        if (index - 1 >= 0)
        {
            index--;
            UpdateText();
        }
    }

    void UpdateText()
    {
        text.text = (string)texts[index];
    }

    public void SlideLeft() {
        anim.Play("SlideText_L");
    }
}
