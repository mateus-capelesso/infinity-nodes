using System;
using System.Collections;
using UnityEngine;
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
	
	public GameObject canvas;
	public Button nextLevelButton;
	public GameView gameView;


	private int _winValue;
	private int _piecesOnCorrectPlaces;

	private int level;
	private int maxLevel;
	private int points;

	public Action OnPuzzleOver;
	public Action OnNextPuzzleCalled;


	// Use this for initialization
	void Start ()
	{
		nextLevelButton.onClick.AddListener(() =>
		{
			OnNextPuzzleCalled?.Invoke();
			StartCoroutine(WaitHideAnimation());
		});
		
		LoadUserData();
		LoadPuzzle();
	}

	public void LoadPuzzle()
	{
		SaveUserData();
		nextLevelButton.gameObject.SetActive(false);
		gameView.SetLevelView(level);
		if (level > 30) level = 1; // Adding a simple barrier, so the game don't breaks 
		var puzzle = Resources.Load<PuzzleLevels>(level.ToString());
		Puzzle.Instance.BuildPuzzle(puzzle.seed, puzzle.width, puzzle.height);
	}

	public int GetMaxLevel()
	{
		return maxLevel;
	}
	
	public void SetLevel(int value)
	{
		level = value;
		OnNextPuzzleCalled?.Invoke();
		StartCoroutine(WaitHideAnimation());
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
		OnPuzzleOver?.Invoke();
		canvas.SetActive (true);
	}

	IEnumerator WaitHideAnimation()
	{
		yield return new WaitForSeconds(1f);
		LoadPuzzle();
	}

	#region User Data

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

	#endregion
}
