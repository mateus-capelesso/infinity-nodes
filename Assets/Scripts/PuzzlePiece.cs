using System;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class PuzzlePiece : MonoBehaviour
{

	[SerializeField] private int[] possibleConnections;

	public int[] PossibleConnections
	{
		get => possibleConnections;
		set => possibleConnections = value;
	}

	public Vector2 CorrectPosition { get; set; }

	public bool swapped;
	private Vector2 _initiatePosition;
	private Vector3 _offset;
	
	[Header("Color")]
	public Color pieceColor;
	
	#region RotateConnections
	
	public void RotatePiece()
	{
		transform.Rotate(new Vector3(0f, 0f , 90f));
		RotateValues ();
	}
	
	private void RotateValues()
	{
		int aux = PossibleConnections[0];
		
		for (int i = 0; i < PossibleConnections.Length - 1; i++) {
			PossibleConnections[i] = PossibleConnections[i + 1];
		}
		PossibleConnections[3] = aux;
	}
	
	#endregion

	public void ChangePosition(Vector2 newPosition)
	{
		_initiatePosition = newPosition;
		transform.position = new Vector3(newPosition.x, newPosition.y, 0f);
		swapped = true;
		GetComponent<SpriteRenderer>().color = new Color(pieceColor.r, pieceColor.g, pieceColor.b, 0.5f);

		// Check if new position fits slot requirements
		if(Puzzle.Instance.ValidatePuzzlePiecePosition(newPosition, possibleConnections))
			PieceOnCorrectPosition();
	}
	
	public Vector2 GetInitiatePosition()
	{
		return _initiatePosition;
	}

	private void AnalyzePiecePosition(Vector2 coordinates)
	{
		if (Puzzle.Instance.ValidatePuzzlePiecePosition(coordinates, possibleConnections))
		{
			Puzzle.Instance.SwapPiecePosition(coordinates, _initiatePosition);
			transform.position = new Vector3(coordinates.x, coordinates.y, 0);
			_initiatePosition = coordinates;
			PieceOnCorrectPosition();
			SoundController.Instance.PieceSound();
		}
		else
		{
			// Return to original position
			transform.position = new Vector3(_initiatePosition.x, _initiatePosition.y, 0f);
			Handheld.Vibrate();
		}
	}

	private void PieceOnCorrectPosition()
	{
		GameManager.Instance.OnPieceOnCorrectPosition();
		GetComponent<SpriteRenderer>().color = GetComponent<SpriteRenderer>().color = new Color(pieceColor.r, pieceColor.g, pieceColor.b, 0.8f);
		DeactivatePiece();
	}

	public void DeactivatePiece()
	{
		GetComponent<BoxCollider>().enabled = false;
	}

	#region MouseEvents
	private void OnMouseDown()
	{
		var position = gameObject.transform.position;
		_offset = position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f));
		SoundController.Instance.PieceSound();
	}

	public void OnMouseDrag()
	{
		Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
 
		Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + _offset;
		transform.position = curPosition;
	}

	public void OnMouseUp()
	{
		var position = transform.position;
		var x = Mathf.RoundToInt(position.x);
		var y = Mathf.RoundToInt(position.y);

		AnalyzePiecePosition(new Vector2(x, y));
	}
	
	#endregion
}
