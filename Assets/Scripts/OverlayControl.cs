using System;
using UnityEngine;
using UnityEngine.UI;

public class OverlayControl : MonoBehaviour
{
    public Image overlay;

    public void FadeInFadeOut(float duration = 1f)
    {
        gameObject.SetActive(true);
        
        iTween.ValueTo(gameObject,iTween.Hash(
            "from", 1f,
            "to", 0f,
            "time", duration,
            "onupdate","SetColorAlpha",
            "oncomplete", "FadeOut",
            "oncompleteparams", duration));
    }

    public void FadeIn(float duration = 1f, GameObject target = null, string method = null)
    {
        gameObject.SetActive(true);
        iTween.ValueTo(gameObject,iTween.Hash(
            "from", 0f,
            "to", 1f,
            "time", duration,
            "onupdate","SetColorAlpha",
            "oncompletetarget", target,
            "oncomplete", method));
    }

    public void FadeOut(float duration = 1f)
    {
        gameObject.SetActive(true);
        iTween.ValueTo(gameObject,iTween.Hash(
            "from", 1f,
            "to", 0f,
            "time", duration,
            "onupdate","SetColorAlpha",
            "oncomplete", "EndFadeOut"));
    }

    private void EndFadeOut()
    {
        gameObject.SetActive(false);
    }

    private void SetColorAlpha(float alphaValue)
    {
        var color = overlay.color;
        overlay.color = new Color(color.r, color.g, color.b, alphaValue);
    }
}
