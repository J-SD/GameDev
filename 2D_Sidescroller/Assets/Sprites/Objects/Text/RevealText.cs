using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RevealText : MonoBehaviour
{
    private TextMeshPro tmp;
    public float timeBetweenChars = 0.05f;
    private float delay = 0f;
    public bool revealed = false;
    public bool revealing = false;

    public int maxChars;

    private void Awake()
    {
        tmp = GetComponent<TextMeshPro>();

        //maxChars = tmp.textInfo.characterCount;
        int count = 0;
        foreach (char c in tmp.text) {
            count++;
        }
        maxChars = count;
    }

    int visibleCount;
    public IEnumerator Reveal()
    {
        if (revealed) yield break;

        revealing = true;
        int totalVisableChars = maxChars;
        int counter = 0;
        visibleCount = 0;
        tmp.maxVisibleCharacters = 0;
        yield return new WaitForSeconds(delay);
        print(totalVisableChars);
        while (totalVisableChars > visibleCount) {
            visibleCount = counter % (totalVisableChars + 1);
            tmp.maxVisibleCharacters = visibleCount;
            counter++;
            yield return new WaitForSeconds(timeBetweenChars);

        }

        revealed = true;

    }

    public IEnumerator UnReveal()
    {
        if (!revealed) yield break;

        tmp = GetComponent<TextMeshPro>();
        int totalVisableChars = maxChars;
        int counter = totalVisableChars;
        visibleCount = totalVisableChars;
        while (visibleCount > 0)
        {
            visibleCount = counter % (totalVisableChars + 1);
            tmp.maxVisibleCharacters = visibleCount;
            counter--;
            yield return new WaitForSeconds(timeBetweenChars);
        }
        tmp.enabled = false;

    }

    public void StartReveal(float d = 0f, float tbc = 0.05f)
    {
        if (revealing) return;
        if (tbc != 0.05f)
        {
            timeBetweenChars = tbc;
        }
        delay = d;
        StartCoroutine("Reveal");
    }
    public void StartUnReveal(float tbc)
    {
        timeBetweenChars = tbc;
        StartCoroutine("UnReveal");
    }

    private void Update()
    {
        print(tmp.maxVisibleCharacters);


        if (revealed && tmp.textInfo.characterCount < tmp.maxVisibleCharacters) {
            tmp.maxVisibleCharacters = tmp.textInfo.characterCount;

        }
    }
}
