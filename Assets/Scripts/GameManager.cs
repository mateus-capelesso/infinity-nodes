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
	public GameView gameView;


	private int _totalPiecesSwapped;
	private int _piecesOnCorrectPlaces;

	private int _level;
	private int _maxLevel;
	private int _points;

	public Action OnPuzzleOver;
	public Action OnNextPuzzleCalled;
	
	void Start ()
	{
		Screen.sleepTimeout = SleepTimeout.NeverSleep;

		LoadUserData();
		LoadPuzzle();
	}

	private void LoadPuzzle()
	{
		SaveUserData();
		gameView.SetLevelView(_level.ToString());
		
		if (_level > 30) _level = 1; // Adding a simple barrier, so the game don't breaks 
		var puzzle = Resources.Load<PuzzleLevels>(_level.ToString());
		Puzzle.Instance.BuildPuzzle(puzzle.seed, puzzle.width, puzzle.height);
	}

	public int GetMaxLevel()
	{
		return _maxLevel;
	}

	public void SetLevel(int value)
	{
		_level = value;
		OnNextPuzzleCalled?.Invoke();
		StartCoroutine(WaitHideAnimation(1f));
	}

	public void SetWinValue(int value)
	{
		_totalPiecesSwapped = value;
		_piecesOnCorrectPlaces = 0;
	}

	public void PieceOnCorrectPosition()
	{
		_piecesOnCorrectPlaces++;
		if (_piecesOnCorrectPlaces < _totalPiecesSwapped) return;
		OnPuzzleOver?.Invoke();
			
		PointsAndLevelOperations();
		StartCoroutine(WaitHideAnimation(2f));
	}
	
	private void PointsAndLevelOperations()
	{
		_points += _totalPiecesSwapped;
		_level++;
		
		if (_level > _maxLevel)
			_maxLevel = _level;
	}
	
	// TODO: change wait function, manager don't need to handle this kind of operation
	IEnumerator WaitHideAnimation(float time)
	{
		yield return new WaitForSeconds(0.5f);
		gameView.SetLevelView("");
		OnNextPuzzleCalled?.Invoke();
		yield return new WaitForSeconds(time);
		LoadPuzzle();
	}
	
	#region User Data

	private void LoadUserData()
	{
		_level = PlayerPrefs.HasKey("level") ? PlayerPrefs.GetInt("level") : 1;
		_points = PlayerPrefs.HasKey("points") ? PlayerPrefs.GetInt("points") : 1;
		_maxLevel = PlayerPrefs.HasKey("maxLevel") ? PlayerPrefs.GetInt("maxLevel") : 1;
	}

	private void SaveUserData()
	{
		PlayerPrefs.SetInt("level", _level);
		PlayerPrefs.SetInt("points", _points);
		PlayerPrefs.SetInt("maxLevel", _maxLevel);
		PlayerPrefs.Save();
	}

	#endregion
}
