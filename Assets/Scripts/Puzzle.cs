using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class Puzzle : MonoBehaviour
{

    #region SINGLETON

    private static Puzzle _instance;

    public static Puzzle Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = GameObject.FindObjectOfType<Puzzle>();
            }

            return _instance;
        }
    }

    #endregion
    
    [SerializeField] private string seed;
    [SerializeField] private int width;
    [SerializeField] private int height;
    
    [SerializeField] private GameObject[] piecesPrefabs;
    [SerializeField] private PuzzleAnimations puzzleAnimations;
    
    [Header("Slot Component")]
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private Transform slotParent;

    [Header("Camera")] public CameraController camera;
    
    private PuzzlePiece[,] _pieces;
    private List<Slot> _slots;
    private List<PuzzlePiece> _availablePiecePositions;
    
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        GameManager.Instance.OnNextPuzzleCalled += () =>
        {
            StartCoroutine(WaitHideAnimation(DeletePuzzle));
        };
    }

    public void BuildPuzzle(string puzzleSeed, int puzzleWidth, int puzzleHeight)
    {
        seed = puzzleSeed;
        width = puzzleWidth;
        height = puzzleHeight;
        camera.SetCameraPosition();
        Random.InitState(seed.GetHashCode());
        GeneratePuzzle();
    }

    private void GeneratePuzzle()
    {
        _availablePiecePositions = new List<PuzzlePiece>();
        _slots = new List<Slot>();
        _pieces = new PuzzlePiece[width, height];

        // Array is sorted as {Top, Right, Bottom, Left}
        int[] connections = { 0, 0, 0, 0 };
		
        // It uses last piece connection values to create a new piece
        for (var i = 0; i < height; i++) 
        { 
            for (var j = 0; j < width; j++) 
            {
                // Width
                connections[3] = j == 0 ? 0 : _pieces[j - 1, i].PossibleConnections[1];
                connections[1] = j == width - 1 ? 0 : Random.Range (0, 2);
                
                // Height
                connections[2] = i == 0 ? 0 : _pieces[j, i - 1].PossibleConnections[0];
                connections[0] = i == height - 1 ? 0 : Random.Range(0, 2);
                
                //PuzzlePiece type
                var desiredConnections = connections[0] + connections[1] + connections[2] + connections [3];
				
                // If has 2 connections and top and bottom are different, its a corner piece 
                if (desiredConnections == 2 && connections[0] != connections[2])
                    desiredConnections = 5;
                
                var slot = Instantiate(slotPrefab, new Vector3(j, i, 0), Quaternion.identity, slotParent).GetComponent<Slot>();
                _slots.Add(slot);
                
                // Rotate piece until it fits the connection requirements
                var piece = Instantiate (piecesPrefabs[desiredConnections], new Vector3 (j, i, 0), Quaternion.identity, puzzleAnimations.transform).GetComponent<PuzzlePiece>();
                piece.RotateUntilConnected(connections);

                slot.SetSlotValues(new Vector2(j, i), connections, piece);
                piece.CorrectPosition = new Vector2(j, i);
                
                // Ignoring begin, null and 4-way piece, just to give a direction to the player
                if (desiredConnections > 1 && desiredConnections != 4)
                {
                    _availablePiecePositions.Add(piece);
                }
                else
                {
                    piece.DeactivatePiece();
                }
                
                _pieces[j, i] = piece;
            }
        }
        
        GameManager.Instance.SetWinValue(_availablePiecePositions.Count);
        ShufflePiecePositions();
    }

    private void ShufflePiecePositions()
    {
        for (int i = 0; i < _availablePiecePositions.Count; i++)
        {
            if(_availablePiecePositions[i].swapped) continue;

            // variable swapped controls which pieces weren't swapped
            var toBeSwappedList = _availablePiecePositions.Where(p => !p.swapped).ToList();
            
            // Avoid error when trying to compare the last item on the list. It happens when the list has a odd lenght
            if (toBeSwappedList.Count <= 1)
            {
                _availablePiecePositions[i].SetNewPosition(_availablePiecePositions[i].CorrectPosition);
                break;
            }
            var random = Random.Range(0, toBeSwappedList.Count);
            
            var firstPiece = _availablePiecePositions[i];
            var secondPiece = toBeSwappedList[random];

            if (firstPiece != secondPiece)
            {
                SwapPieceByPieces(firstPiece, secondPiece);
            }
            else
            {
                i--;
            }
        }
    }
    
    public void SwapPieceByPieces(PuzzlePiece firstPiece, PuzzlePiece secondPiece)
    {
        firstPiece.SetNewPosition(secondPiece.CorrectPosition);
        secondPiece.SetNewPosition(firstPiece.CorrectPosition);
    }
    
    // returns the slot that has the same coordinate as the dragged piece
    private Slot GetSlotFromCoordinate(Vector2 coordinates)
    {
        return _slots.Where(s => s.GetSlotFromPosition(coordinates)).ToList().First();
    }
    
    // checks if dragged piece has the correct connection values
    public bool ValidatePuzzlePiecePosition(Vector2 coordinates, int[] pieceConnection)
    {
        return GetSlotFromCoordinate(coordinates).CorrectConnections(pieceConnection);
    }
    
    // swap position between dragged piece and the one that sits on spot
    public void SwapPieceByPosition(Vector2 newPosition, Vector2 initialPosition)
    {
        var piece = _availablePiecePositions.Where(p => p.GetInitiatePosition() == newPosition).ToList().First();
        piece.SetNewPosition(initialPosition);
    }

    public Vector2 GetPuzzleSize()
    {
        return new Vector2(width, height);
    }

    private void DeletePuzzle()
    {
        foreach (var p in _pieces)
        {
            Destroy(p.gameObject);
        }

        foreach (var s in _slots)
        {
            Destroy(s.gameObject);
        }

        _pieces = new PuzzlePiece[0, 0];
        _slots.Clear();
        _availablePiecePositions.Clear();
    }

    public void PlayParticlesFromPieces()
    {
        foreach (var p in _pieces)
        {
            
            p.PlayParticles();
        }
    }

    IEnumerator WaitHideAnimation(Action callback)
    {
        yield return new WaitForSeconds(1f);
        callback?.Invoke();
    }
}