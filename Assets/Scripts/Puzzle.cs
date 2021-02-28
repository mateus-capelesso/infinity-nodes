using System.Collections.Generic;
using System.Linq;
using TreeEditor;
using Unity.IO.LowLevel.Unsafe;
using UnityEditor.Scripting;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

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
    [SerializeField] private Transform puzzleParent;
    
    [Header("Slot Component")]
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private Transform slotParent;
    
    private PuzzlePiece[,] _pieces;
    private List<Slot> _slots;
    private List<PuzzlePiece> _availablePiecePositions;
    
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
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
                
                //tells us PuzzlePiece type
                int valueSum = auxValues[0] + auxValues[1] + auxValues[2] + auxValues [3];
				
                if (valueSum == 2 && auxValues[0] != auxValues[2])
                    valueSum = 5;


                var slot = Instantiate(slotPrefab, new Vector3(w, h, 0), Quaternion.identity, slotParent);
                var slotComponent = slot.GetComponent<Slot>();
                _slots.Add(slotComponent);


                var obj = Instantiate (piecesPrefabs[valueSum], new Vector3 (w, h, 0), Quaternion.identity, puzzleParent);

                while (obj.GetComponent<PuzzlePiece>().PossibleConnections [0] != auxValues [0] ||
                       obj.GetComponent<PuzzlePiece>().PossibleConnections [1] != auxValues [1] ||
                       obj.GetComponent<PuzzlePiece>().PossibleConnections [2] != auxValues [2] ||
                       obj.GetComponent<PuzzlePiece>().PossibleConnections [3] != auxValues [3])
                {
                    obj.GetComponent<PuzzlePiece>().RotatePiece ();
                } 
                
                var piece = obj.GetComponent<PuzzlePiece>();
                
                slotComponent.SetSlotValues(new Vector2(w, h), auxValues, piece);
                piece.CorrectPosition = new Vector2(w, h);

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

            var toBeSwappedList = _availablePiecePositions.Where(p => !p.swapped).ToList();
            if (toBeSwappedList.Count <= 1)
            {
                _availablePiecePositions[i].ChangePosition(_availablePiecePositions[i].CorrectPosition);
                break;
            }
            var random = Random.Range(0, toBeSwappedList.Count);
            
            var firstPiece = _availablePiecePositions[i];
            var secondPiece = toBeSwappedList[random];

            if (firstPiece != secondPiece)
            {
                firstPiece.ChangePosition(secondPiece.CorrectPosition);
                secondPiece.ChangePosition(firstPiece.CorrectPosition);
            }
            else
            {
                i--;
            }
        }
    }
    

    public void SwapPiecePosition(Vector2 newPosition, Vector2 initialPosition)
    {
        var piece = _availablePiecePositions.Where(p => p.GetInitiatePosition() == newPosition).ToList().First();
        piece.ChangePosition(initialPosition);
        
    }
    
    public bool ValidatePuzzlePiecePosition(Vector2 coordinates, int[] pieceConnection)
    {
        return GetSlotFromCoordinate(coordinates).CorrectConnections(pieceConnection);
    }

    private Slot GetSlotFromCoordinate(Vector2 coordinates)
    {
        return _slots.Where(s => s.GetSlotFromPosition(coordinates)).ToList().First();
    }
}