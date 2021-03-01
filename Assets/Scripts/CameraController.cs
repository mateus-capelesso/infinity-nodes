using UnityEngine;

public class CameraController : MonoBehaviour
{
    
    public void SetCameraPosition()
    {
        var size = Puzzle.Instance.GetPuzzleSize();
        transform.position = new Vector3((float) (size.x * 0.5) - 0.5f, (float) (size.y * 0.5) - 0.5f, -10f);
        
        GetComponent<Camera>().orthographicSize = (float) (size.x * Screen.height / Screen.width * 0.5) + 2f ;
    }
    
}