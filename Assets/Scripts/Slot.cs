using System.Linq;
using UnityEngine;

public class Slot : MonoBehaviour
{
    private Vector2 _positionValues;
    [SerializeField] private int[] connectionValues;

    public void SetSlotValues(Vector2 position, int[] values, PuzzlePiece piece)
    {
        _positionValues = position;
        
        connectionValues = new int[4];
        values.CopyTo(connectionValues, 0);
    }
    
    public bool GetSlotFromPosition(Vector2 position)
    {
        return _positionValues == position;
    }

    public bool CorrectConnections(int[] values)
    {
        return connectionValues.SequenceEqual(values);
    }
    
}
