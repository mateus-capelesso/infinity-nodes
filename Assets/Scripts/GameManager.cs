using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{

	public static GameManager Instance;
	
	public bool GenerateRandom;
	public GameObject canvas;
	
	public Puzzle puzzle;


	// Use this for initialization
	void Start () {

		canvas.SetActive (false);

		if (!GenerateRandom) return;

		puzzle.BuildPuzzle("seed1", 5, 5);
		
		// puzzle.winValue = GetWinValue ();
		// puzzle.curValue=Sweep ();
	}
	
	// public int Sweep()
	// {
	// 	int value = 0;
	//
	// 	for (int h = 0; h < puzzle.height; h++) {
	// 		for (int w = 0; w < puzzle.width; w++) {
	// 			
	// 			//compares top
	// 			if(h!=puzzle.height-1)
	// 				if (puzzle.pieces [w, h].values [0] == 1 && puzzle.pieces [w, h + 1].values [2] == 1)
	// 					value++;
	// 			
	// 			//compare right
	// 			if(w!=puzzle.width-1)
	// 				if (puzzle.pieces [w, h].values [1] == 1 && puzzle.pieces [w + 1, h].values [3] == 1)
	// 					value++;
	// 		}
	// 	}
	// 	return value;
	// }


	public void Win()
	{
		canvas.SetActive (true);
	}

	// public int QuickSweep(int w,int h)
	// {
	// 	int value = 0;
	//
	// 	//compares top
	// 	if(h!=puzzle.height-1)
	// 		if (puzzle.pieces [w, h].values [0] == 1 && puzzle.pieces [w, h + 1].values [2] == 1)
	// 			value++;
	//
	//
	// 	//compare right
	// 	if(w!=puzzle.width-1)
	// 		if (puzzle.pieces [w, h].values [1] == 1 && puzzle.pieces [w + 1, h].values [3] == 1)
	// 			value++;
	//
	//
	// 	//compare left
	// 	if (w != 0)
	// 		if (puzzle.pieces [w, h].values [3] == 1 && puzzle.pieces [w - 1, h].values [1] == 1)
	// 			value++;
	//
	// 	//compare bottom
	// 	if (h != 0)
	// 		if (puzzle.pieces [w, h].values [2] == 1 && puzzle.pieces [w, h-1].values [0] == 1)
	// 			value++;
	// 	
	// 	return value;
	//
	// }

	// int GetWinValue()
	// {
	//
	// 	int winValue = 0;
	// 	foreach (var piece in puzzle.pieces) {
	// 		
	// 		foreach (var j in piece.values) {
	// 			winValue += j;
	// 		}
	//
	//
	// 	}
	// 	winValue /= 2;
	// 	return winValue;
	// }

	// void Shuffle()
	// {
	// 	foreach (var piece in puzzle.pieces) {
	//
	// 		int k = Random.Range (0, 4);
	//
	// 		for (int i = 0; i < k; i++) {
	// 			piece.RotatePiece ();
	// 		}
	//
	//
	// 	}
	// }
	
	// Vector2 CheckDimensions()
	// {
	// 	Vector2 aux = Vector2.zero;
	//
	// 	GameObject[] pieces = GameObject.FindGameObjectsWithTag ("Piece");
	//
	// 	foreach (var p in pieces) {
	// 		if (p.transform.position.x > aux.x)
	// 			aux.x = p.transform.position.x;
	//
	// 		if (p.transform.position.y > aux.y)
	// 			aux.y= p.transform.position.y;
	// 	}
	//
	// 	aux.x++;
	// 	aux.y++;
	//
	// 	return aux;
	// }
}
