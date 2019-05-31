using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class JoystickText : MonoBehaviour
{
    public string keyboardText = "";
    public string xboxText = "";

    bool hasJoystick = false;
    bool usedKeyboard = false;
    bool lastFrameJoystick;
    TextMeshPro text;
    RevealText rt;

    enum InputType {
        keyboard,xbox,playstation
    }

    InputType currentType = InputType.keyboard;

    // Start is called before the first frame update
    void Awake()
    {
        text = GetComponent<TextMeshPro>();
        rt = GetComponent<RevealText>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!usedKeyboard) usedKeyboard = Input.anyKey;
        if (!usedKeyboard) { hasJoystick = Input.GetJoystickNames().Length > 0; }
        else hasJoystick = false;

        if (hasJoystick) {
            if (Input.GetJoystickNames()[0].Length == 0) {
                hasJoystick = false;
            }
        }
        if (rt)
        {
            if (lastFrameJoystick != hasJoystick)
            { // we changed input methods
                if (rt.revealed || rt.revealing)
                {
                    if (hasJoystick)
                    {
                        rt.maxChars = xboxText.Length;
                        text.text = xboxText;
                    }
                    else
                    {
                        rt.maxChars = keyboardText.Length;
                        text.text = keyboardText;
                    }

                    rt.StartReveal();
                }
            }
        }

        lastFrameJoystick = hasJoystick;
            if (hasJoystick)
            {
                if(rt) rt.maxChars = xboxText.Length;
                text.text = xboxText;
            }
            else
            {
                if (rt) rt.maxChars = keyboardText.Length;
                text.text = keyboardText;
            }
        
    }


}
