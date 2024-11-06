using UnityEngine;

public class CameraController : MonoBehaviour
{
    public DungeonGenerator dungeonGenerator; // Reference to the DungeonGenerator script
    public float padding = 2f; // Extra space around the dungeon to ensure all cells are visible

    private void Start()
    {
        AdjustCamera();
    }

    public void AdjustCamera()
    {
        // Get the dungeon dimensions
        int width = dungeonGenerator.width;
        int height = dungeonGenerator.height;

        // Get the main camera
        Camera mainCamera = Camera.main;
        if (mainCamera != null)
        {
            // Calculate the aspect ratio of the screen
            float targetAspect = (float)width / (float)height;
            float screenAspect = (float)Screen.width / (float)Screen.height;

            // Adjust orthographic size based on the larger dimension
            if (screenAspect >= targetAspect)
            {
                // Screen is wider than the dungeon
                mainCamera.orthographicSize = (height / 2f) + padding;
            }
            else
            {
                // Screen is taller than the dungeon
                mainCamera.orthographicSize = (width / 2f / screenAspect) + padding;
            }

            // Center the camera on the dungeon
            Vector3 cameraPosition = new Vector3(width / 2f, height / 2f, -10f);
            mainCamera.transform.position = cameraPosition;
        }
    }
}
