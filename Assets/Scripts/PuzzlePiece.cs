using UnityEngine;
using System.Collections;
using TreeEditor;

public class PuzzlePiece : MonoBehaviour
{

	private PuzzlePieceType _type;
	public int[] values;
	
	public Vector2 TargetPosition { get; set; }

	public bool Swaped { get; set; }

	public void RotatePiece()
	{
		transform.Rotate(new Vector3(0f, 0f , 90f));
		RotateValues ();
	}
	
	private void RotateValues()
	{
		int aux = values [0];
		
		for (int i = 0; i < values.Length-1; i++) {
			values[i] = values[i + 1];
		}
		values[3] = aux;
	}

	public void ChangePosition(Vector2 newPosition)
	{
		transform.position = new Vector3(newPosition.x, newPosition.y, 0f);
		Swaped = true;
	}

	public void SetPieceType(int value)
	{
		_type = (PuzzlePieceType)value;
		if (_type == PuzzlePieceType.Begin || _type == PuzzlePieceType.Null || _type == PuzzlePieceType.Cross)
		{
			Swaped = true;
		}
	} 

	public PuzzlePieceType PieceType => _type;

}
