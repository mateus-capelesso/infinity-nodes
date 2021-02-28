using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


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
	
	public Puzzle puzzle;

	private int _winValue;
	private int _piecesOnCorrectPlaces;


	// Use this for initialization
	void Start () {

		canvas.SetActive (false);

		if (!GenerateRandom) return;

		puzzle.BuildPuzzle("seed1111", 6, 6);
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
		}
	}
	
	private void Win()
	{
		canvas.SetActive (true);
		
	}

	private void LoadUserData()
	{
		
	}
}
