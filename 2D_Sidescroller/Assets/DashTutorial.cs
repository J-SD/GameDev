using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DashTutorial : MonoBehaviour
{
    public LayerMask playerMask;
    private TextMeshPro playerText;
    public GameObject angelito;
    bool done = false;
    // Start is called before the first frame update
    void Start()
    {
        playerText = Player.Instance.playerTextObject.GetComponent<TextMeshPro>();
    }

    // Update is called once per frame
    void Update()
    {

        if (angelito == null)
        {
            done = true;
        }
        if(Input.GetButtonUp("Dash")) {
            playerText.text = "";
        }

    }


    float startTime = 0f;
    private void OnTriggerStay2D(Collider2D c)
    {
        if (done) { playerText.text = ""; return; }
        playerText = Player.Instance.playerTextObject.GetComponent<TextMeshPro>();

        if ((playerMask.value & 1 << c.gameObject.layer) == 1 << c.gameObject.layer) {
            if (Input.GetButton("Dash") && !Player.Instance.rechargingDash)
            {
                if (startTime == 0f)
                {
                    startTime = Time.time;

                }
                else if (Time.time - startTime < 2f)
                {
                    playerText.text = "Aim";
                }
                else if (Time.time - startTime >= 2f) {
                    playerText.text = "Release";
                }
            }
            else {
                if (Player.Instance.rechargingDash) { playerText.text = "Recharging"; }
                else playerText.text = "Hold Shift";
            }
            if (!Player.Instance.rechargingDash && Input.GetButtonUp("Dash")) {
                playerText.text = "";
                startTime = 0f;
            }
        }

    }

}
