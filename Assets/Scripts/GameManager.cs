using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
	#region SINGLETON

	private static GameManager _instance;

	public static GameManager Instance
	{
		get
		{
			if(_instance == null)
			{
				_instance = FindObjectOfType<GameManager>();
			}

			return _instance;
		}
	}

	#endregion
	
	public bool GenerateRandom;
	public GameObject canvas;
	public Button nextLevelButton;
	public TextMeshProUGUI levelText;
	

	private int _winValue;
	private int _piecesOnCorrectPlaces;

	private int level;
	private int maxLevel;
	private int points;


	// Use this for initialization
	void Start ()
	{
		nextLevelButton.onClick.AddListener(() =>
		{
			Puzzle.Instance.DeletePuzzle();
			LoadPuzzle();
		});
		
		LoadUserData();
		LoadPuzzle();
	}

	private void LoadPuzzle()
	{
		SaveUserData();
		nextLevelButton.gameObject.SetActive(false);
		levelText.text = level.ToString();
		var puzzle = Resources.Load<PuzzleLevels>(level.ToString());
		Puzzle.Instance.BuildPuzzle(puzzle.seed, puzzle.width, puzzle.height);
	}

	public void SetWinValue(int value)
	{
		_winValue = value;
		_piecesOnCorrectPlaces = 0;
	}

	public void OnPieceOnCorrectPosition()
	{
		_piecesOnCorrectPlaces++;
		if (_piecesOnCorrectPlaces >= _winValue)
		{
			Win();
			points += _winValue;
			level++;
			if (level > maxLevel)
			{
				maxLevel = level;
			}
			nextLevelButton.gameObject.SetActive(true);
		}
	}
	
	private void Win()
	{
		canvas.SetActive (true);
	}

	private void LoadUserData()
	{
		level = PlayerPrefs.HasKey("level") ? PlayerPrefs.GetInt("level") : 1;
		points = PlayerPrefs.HasKey("points") ? PlayerPrefs.GetInt("points") : 1;
		maxLevel = PlayerPrefs.HasKey("maxLevel") ? PlayerPrefs.GetInt("maxLevel") : 1;
	}

	private void SaveUserData()
	{
		PlayerPrefs.SetInt("level", level);
		PlayerPrefs.SetInt("points", points);
		PlayerPrefs.SetInt("maxLevel", maxLevel);
		PlayerPrefs.Save();
	}
}
