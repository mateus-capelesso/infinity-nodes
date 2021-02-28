using UnityEngine;

[CreateAssetMenu(fileName = "Puzzle", menuName = "Puzzle", order = 0)]
public class PuzzleLevels : ScriptableObject
{
    public int width;
    public int height;

    public string seed;
}