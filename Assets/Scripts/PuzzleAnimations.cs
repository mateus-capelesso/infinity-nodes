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
            "onupdate","SetPiecesColor",
            "time", 0.5f));
    }

    private void PlayParticles()
    {
        Puzzle.Instance.PlayParticlesFromPieces();
    }

    private void SetPiecesColor(Color c)
    {
        var renderers = GetComponentsInChildren<SpriteRenderer>();
        foreach (var r in renderers)
        {
            r.color = c;
        }
    }
}
