using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameView : MonoBehaviour
{
    public Transform gridParent;
    public TextMeshProUGUI levelText;
    public Button levelButton;
    public Button closeGridButton;
    public GameObject levelButtonPrefab;
    public OverlayControl overlay;

    private List<GameObject> _buttons = new List<GameObject>();

    private void Start()
    {
        levelButton.onClick.AddListener(ActivateGrid);
        closeGridButton.onClick.AddListener((() =>
        {
            CloseGridLevel();
            overlay.FadeOut(0.5f);
        }));
        
        GameManager.Instance.OnNextPuzzleCalled += (() =>
        {
            overlay.gameObject.SetActive(true);
            overlay.FadeInFadeOut(1f);
        });
        
        overlay.FadeOut(0.5f);
    }

    public void SetLevelView(int level)
    {
        levelText.text = $"{level}";
    }

    private void ActivateGrid()
    {
        var maxLevel = GameManager.Instance.GetMaxLevel();
        overlay.FadeIn(0.5f);
        
        for (int i = 1; i < maxLevel; i++)
        {
            var desiredLevel = i;
            var button = Instantiate(levelButtonPrefab, gridParent);
            button.GetComponentInChildren<TextMeshProUGUI>().text = desiredLevel.ToString();
            _buttons.Add(button);
            button.GetComponent<Button>().onClick.AddListener((() =>
            {
                GoToLevel(desiredLevel);
            }));
        }
        gridParent.gameObject.SetActive(true);
        closeGridButton.gameObject.SetActive(true);
    }

    private void GoToLevel(int level)
    {
        GameManager.Instance.SetLevel(level);
        CloseGridLevel();
    }

    private void CloseGridLevel()
    {
        gridParent.gameObject.SetActive(false);
        closeGridButton.gameObject.SetActive(false);
        foreach (var b in _buttons)
        {
            Destroy(b);
        }
        _buttons.Clear();
    }
    
}