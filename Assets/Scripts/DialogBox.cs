using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class DialogBox : MonoBehaviour
{
    public enum PrintTextMethod
    {
        Typewriter,
        Instant
    }
    
    [SerializeField] private Color textColor = Color.white;
    [SerializeField] private float charPerSec = 60f;
    [SerializeField] private string debugString;
    [SerializeField] private AudioDatabase sound;

    [SerializeField] private Text textUI;

    public IEnumerator DrawText(string text)
    {
        yield return StartCoroutine(DrawText(text, -1, false));
    }

    public IEnumerator DrawText(string text, float secPerChar)
    {
        yield return StartCoroutine(DrawText(text, secPerChar, false));
    }

    public IEnumerator DrawTextSilent(string text)
    {
        yield return StartCoroutine(DrawText(text, -1, true));
    }

    public void DrawTextInstant(string text)
    {
         StartCoroutine(DrawText(text, 0, false));
    }

    public void DrawTextInstantSilentFunction(string text)
    {
        StartCoroutine(DrawTextInstantSilent(text));
    }
    
    public IEnumerator DrawTextInstantSilent(string text)
    {
        yield return StartCoroutine(DrawText(text, 0, true));
    }

    public IEnumerator DrawText(string text, float charPerSec, bool silent)
    {
        if (charPerSec <= -1)
        {
            charPerSec = this.charPerSec;
        }
        foreach (char t in text)
        {
            if (charPerSec > 0)
            {
                if (!silent)
                {
                    AudioHandler.Main.PlaySFX(sound.Get("battleText"));
                }
                textUI.text += t;
                yield return new WaitForSeconds(1/charPerSec);
            }
            else
            {
                textUI.text = text;
            }
        }
    }


    public void DrawDialogBox(Color color)
    {
        textUI.color = color;
        textUI.text = "";
    }
    
    public void UndrawDialogBox()
    {
        textUI.text = "";
    }
}
