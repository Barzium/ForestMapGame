using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Vector2 cameraRealSize => new Vector2(Camera.main.orthographicSize * 2 * Camera.main.aspect, Camera.main.orthographicSize * 2);
    private Vector3 GetCamCornerPosition => new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, 0) - (Vector3)cameraRealSize / 2;
    private void Awake() {
        float correctWidth = Camera.main.orthographicSize * (16f / 9f);
        Camera.main.orthographicSize = (correctWidth / Camera.main.aspect);
    }


}
