using System;
using UnityEngine;
using System.Collections;
using System.Xml;
using TreeEditor;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PuzzlePiece : MonoBehaviour
{

	[SerializeField] private int[] possibleConnections;

	public int[] PossibleConnections
	{
		get => possibleConnections;
		set => possibleConnections = value;
	}

	public Vector2 CorrectPosition { get; set; }

	public bool swapped = false;
	private Vector2 _initiatePosition;
	private Vector3 _offset;

	public void RotatePiece()
	{
		transform.Rotate(new Vector3(0f, 0f , 90f));
		RotateValues ();
	}
	
	private void RotateValues()
	{
		int aux = possibleConnections [0];
		
		for (int i = 0; i < possibleConnections.Length-1; i++) {
			possibleConnections[i] = possibleConnections[i + 1];
		}
		possibleConnections[3] = aux;
	}

	public void ChangePosition(Vector2 newPosition)
	{
		transform.position = new Vector3(newPosition.x, newPosition.y, 0f);
		_initiatePosition = newPosition;
		swapped = true;
		GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.5f);

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
			PieceOnCorrectPosition();
		}
		else
		{
			transform.position = new Vector3(_initiatePosition.x, _initiatePosition.y, 0f);
		}
	}

	public void PieceOnCorrectPosition()
	{
		GameManager.Instance.OnPieceOnCorrectPosition();
		GetComponent<SpriteRenderer>().color = Color.white;
		DeactivatePiece();
	}

	public void DeactivatePiece()
	{
		GetComponent<BoxCollider>().enabled = false;
	}

	private void OnMouseDown()
	{
		var position = gameObject.transform.position;
		_offset = position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f));
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
}
