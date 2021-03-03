using System;
using UnityEngine;
using UnityEngine.UI;

public class OverlayControl : MonoBehaviour
{
    public Image overlay;
    public Color backgroundColor;
    public Color fadedBackgroundColor;
    
    public void FadeInFadeOut(float duration)
    {
        gameObject.SetActive(true);
        iTween.ValueTo(gameObject,iTween.Hash(
            "from",overlay.color,
            "to",backgroundColor,
            "time", duration,
            "onupdate","SetColor",
            "oncomplete", "FadeOut",
            "oncompleteparams", duration));
    }

    public void FadeIn(float duration)
    {
        gameObject.SetActive(true);
        iTween.ValueTo(gameObject,iTween.Hash(
            "from",overlay.color,
            "to",backgroundColor,
            "time", duration,
            "onupdate","SetColor"));
    }

    public void FadeOut(float duration)
    {
        gameObject.SetActive(true);
        iTween.ValueTo(gameObject,iTween.Hash(
            "from",overlay.color,
            "to",fadedBackgroundColor,
            "onupdate","SetColor",
            "time", duration,
            "oncomplete", "EndFadeOut"));
    }

    private void EndFadeOut()
    {
        gameObject.SetActive(false);
    }
    
    private void SetColor(Color c)
    {
        overlay.color = c;
    }
}
