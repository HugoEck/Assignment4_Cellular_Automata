using UnityEngine;

public class CameraController : MonoBehaviour
{
    public DungeonGenerator dungeonGenerator; 
    public float padding = 2f; 

    private void Start()
    {
        AdjustCamera();
    }

    public void AdjustCamera()
    {
        int width = dungeonGenerator.width;
        int height = dungeonGenerator.height;

        Camera mainCamera = Camera.main;
        if (mainCamera != null)
        {
            float targetAspect = (float)width / (float)height;
            float screenAspect = (float)Screen.width / (float)Screen.height;

            if (screenAspect >= targetAspect)
            {
                mainCamera.orthographicSize = (height / 2f) + padding;
            }
            else
            {
                mainCamera.orthographicSize = (width / 2f / screenAspect) + padding;
            }

            // Center the camera
            Vector3 cameraPosition = new Vector3(width / 2f, height / 2f, -10f);
            mainCamera.transform.position = cameraPosition;
        }
    }
}
