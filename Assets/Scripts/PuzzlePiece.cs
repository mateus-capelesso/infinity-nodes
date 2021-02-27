using UnityEngine;
using System.Collections;
using TreeEditor;

public class PuzzlePiece : MonoBehaviour {


	public int[] values;

	public void RotatePiece()
	{
		transform.Rotate(new Vector3(0f, 0f , 90f));
		RotateValues ();
	}
	
	public void RotateValues()
	{
		int aux = values [0];
		
		for (int i = 0; i < values.Length-1; i++) {
			values[i] = values[i + 1];
		}
		values[3] = aux;
	}



}
