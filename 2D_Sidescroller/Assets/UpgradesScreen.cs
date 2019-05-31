using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UpgradesScreen : MonoBehaviour
{
    public GameObject UpgradesCanvas;
    public TextMeshProUGUI collectedText;
    public TextMeshProUGUI multiplierText;
    public TextMeshProUGUI statsText;
    public TextMeshProUGUI modsText;
    public GameObject PauseCanvas;
    // Start is called before the first frame update
    void OnEnable()
    {
        UpgradesCanvas.SetActive(false);
    }

    // Update is called once per frame
    void OnGUI()
    {
        //collectedText.text = "Sins Collected:\n"+ GameManager.Instance.collectables.ToString();  
        //multiplierText.text = "Power Multiplier:\n"+ GameManager.Instance.powerMultiplier.ToString() + "x";  // USE THIS ONE FOR PROGESSION
        //multiplierText.text = "Note: This is still a work in progress";  // TODO JUST FOR TESTING

        statsText.text = "";
        modsText.text = "";

        List<Modifier> mods = Player.Instance.GetProMods().appliedMods;
        foreach (Modifier mod in mods) {
            statsText.text += mod.Stat.ToUpper() + "................................................................\n";
            modsText.text += mod.Mod + "x\n";
        }

    }

    public void OpenCloseUpgrades() {
        Player.Instance.playerController.playerDash.CalculateDashValues();
        PauseCanvas.SetActive(!PauseCanvas.activeSelf);
        UpgradesCanvas.SetActive(!UpgradesCanvas.activeSelf);

    }
}
