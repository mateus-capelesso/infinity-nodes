using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PuzzleAnimations : MonoBehaviour
{
    public Color pieceColor;
    

    private void Start()
    {
        GameManager.Instance.OnPuzzleOver += SetToWhite;
    }
    

    private void SetToWhite()
    {
        PlayParticles();
        
        iTween.ValueTo(gameObject,iTween.Hash(
            "from",pieceColor,
            "to",Color.white,
            "onupdate","SetColor",
            "time", 1));
    }

    private void PlayParticles()
    {
        Puzzle.Instance.PlayParticles();
    }

    private void SetColor(Color c)
    {
        var renderers = GetComponentsInChildren<SpriteRenderer>();
        foreach (var r in renderers)
        {
            r.color = c;
        }
    }
}
