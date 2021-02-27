using UnityEngine;
using UnityEngine.Serialization;

public class Puzzle : MonoBehaviour
{
    [SerializeField] private string seed;
    [SerializeField] private int width;
    [SerializeField] private int height;
    
    [SerializeField] private GameObject[] piecesPrefabs;
    
    private PuzzlePiece[,] _pieces;
    private int _winValue;
    private int _curValue;



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
        
        _pieces = new PuzzlePiece[width, height];

        int[] auxValues = { 0, 0, 0, 0 };
		
        for (int h = 0; h < height; h++) { 
            for (int w = 0; w < width; w++) {

                //width restrictions
                if (w == 0)
                    auxValues [3] = 0;
                else
                    auxValues [3] = _pieces [w - 1, h].values [1];

                if (w == width - 1) 
                    auxValues [1] = 0;
                else
                    auxValues [1] = Random.Range (0, 2);


                //heigth resctrictions

                if (h == 0) 
                    auxValues [2] = 0;
                else
                    auxValues [2] = _pieces [w, h - 1].values [0];

                if (h == height - 1)
                    auxValues [0] = 0;
                else
                    auxValues [0] = Random.Range (0, 2);


                //tells us PuzzlePiece type
                int valueSum = auxValues [0] + auxValues [1] + auxValues [2] + auxValues [3];
				
                if (valueSum == 2 && auxValues [0] != auxValues [2])
                    valueSum = 5;


                GameObject go = Instantiate (piecesPrefabs [valueSum], new Vector3 (w, h, 0), Quaternion.identity, transform);
                    
                while (go.GetComponent<PuzzlePiece>().values [0] != auxValues [0] ||
                       go.GetComponent<PuzzlePiece>().values [1] != auxValues [1] ||
                       go.GetComponent<PuzzlePiece>().values [2] != auxValues [2] ||
                       go.GetComponent<PuzzlePiece>().values [3] != auxValues [3])
                {
                    go.GetComponent<PuzzlePiece>().RotatePiece ();
                } 
                _pieces [w, h] = go.GetComponent<PuzzlePiece> ();
                
            }
        }
    }
}