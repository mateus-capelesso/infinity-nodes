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
        GeneratePuzzle();
    }

    private void GeneratePuzzle()
    {
        
        Random.InitState(seed.GetHashCode());
        camera.SetCameraPosition();
        
        _availablePiecePositions = new List<PuzzlePiece>();
        _slots = new List<Slot>();
        _pieces = new PuzzlePiece[width, height];

        int[] auxValues = { 0, 0, 0, 0 };
		
        for (var h = 0; h < height; h++) 
        { 
            for (var w = 0; w < width; w++) 
            {
                //width restrictions
                if (w == 0)
                    auxValues[3] = 0;
                else
                    auxValues[3] = _pieces[w - 1, h].PossibleConnections[1];

                if (w == width - 1) 
                    auxValues[1] = 0;
                else
                    auxValues[1] = Random.Range (0, 2);
                
                //heigth resctrictions

                if (h == 0) 
                    auxValues[2] = 0;
                else
                    auxValues[2] = _pieces[w, h - 1].PossibleConnections[0];

                if (h == height - 1)
                    auxValues[0] = 0;
                else
                    auxValues[0] = Random.Range(0, 2);
                
                //PuzzlePiece type
                int valueSum = auxValues[0] + auxValues[1] + auxValues[2] + auxValues [3];
				
                if (valueSum == 2 && auxValues[0] != auxValues[2])
                    valueSum = 5;


                var slot = Instantiate(slotPrefab, new Vector3(w, h, 0), Quaternion.identity, slotParent);
                var slotComponent = slot.GetComponent<Slot>();
                _slots.Add(slotComponent);

                
                // Rotate piece until it fits the connection requirements
                var obj = Instantiate (piecesPrefabs[valueSum], new Vector3 (w, h, 0), Quaternion.identity, puzzleAnimations.transform);

                while (obj.GetComponent<PuzzlePiece>().PossibleConnections [0] != auxValues [0] ||
                       obj.GetComponent<PuzzlePiece>().PossibleConnections [1] != auxValues [1] ||
                       obj.GetComponent<PuzzlePiece>().PossibleConnections [2] != auxValues [2] ||
                       obj.GetComponent<PuzzlePiece>().PossibleConnections [3] != auxValues [3])
                {
                    obj.GetComponent<PuzzlePiece>().RotatePiece();
                } 
                
                var piece = obj.GetComponent<PuzzlePiece>();
                
                slotComponent.SetSlotValues(new Vector2(w, h), auxValues, piece);
                piece.CorrectPosition = new Vector2(w, h);
                
                // Ignoring begin, null and 4-way piece, just to give a direction to the player
                if (valueSum > 1 && valueSum != 4)
                {
                    _availablePiecePositions.Add(piece);
                }
                else
                {
                    piece.DeactivatePiece();
                }
                
                _pieces[w, h] = piece;
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