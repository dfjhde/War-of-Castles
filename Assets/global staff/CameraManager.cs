using UnityEngine;
using System.Collections;

public class CameraManager : MonoBehaviour
{
	void Start ()
    {
        SetupCamera();
    }
	
    private void SetupCamera()
    {
        float screenRadio = (float)Screen.width / (float)Screen.height;
        float picRadio = 1280f / 800f;
        Camera.main.orthographicSize = 4 * (picRadio / screenRadio);
    }
}
