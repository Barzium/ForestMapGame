using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private float minimumDragDistance;
    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private float discardDistance;

    bool beingHeld = false, wasMoved = false, highlighted = false;
    Vector3 dragStartPos;
    private static Camera mainCamera;


    private Vector2 cameraRealSize;
    private Vector2 GetCameraRealSize {
        get {
            if (cameraRealSize == Vector2.zero)
                cameraRealSize = new Vector2(mainCamera.orthographicSize * 2 * mainCamera.aspect, mainCamera.orthographicSize * 2);
            return cameraRealSize;
        }
    }
    private Vector2 GetworldMousePosition()
        => mainCamera.ScreenToWorldPoint(Input.mousePosition);
    private Vector3 GetCamCornerPosition => new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y, 0) - (Vector3)GetCameraRealSize / 2;
    // Start is called before the first frame update
    void Start() {
        if (mainCamera == null)
            mainCamera = Camera.main;
        BoardManager._instance.spawners.Add(this);
    }
    public bool CheckDiscard( Vector2 pos) {
        bool validDiscard =  Vector2.Distance(pos, transform.position) <= discardDistance;
        SetHighlight(validDiscard);
        return validDiscard;
    }

    private void SetHighlight(bool state) {
        if (state && !highlighted) {
            Debug.Log("Spawner Highlighted!");
        }
        else if(!state && highlighted) {
            Debug.Log("Spawner DE-Highlighted!");
        }
        highlighted = state;
    }

    // Update is called once per frame
    void Update() {
        if (beingHeld && !wasMoved && Vector2.Distance(GetworldMousePosition(), dragStartPos) >= minimumDragDistance) {
            wasMoved = true;
            Vector3 pos = GetworldMousePosition();
            pos.z = transform.position.z;
            var tile = Instantiate(tilePrefab, pos, Quaternion.identity, transform.parent);
            tile.GetComponent<MapTileController>().StartDrag();
        }
    }

    private void OnMouseDown() {
        dragStartPos = transform.position;
        beingHeld = true;
        wasMoved = false;
    }
    private void OnMouseUp() {
        beingHeld = false;
    }

    private Vector3 ClampToScreen() {
        Vector3 localMaxCameraCorner = transform.TransformPoint(GetCamCornerPosition);
        Vector3 localMinCameraCorner = transform.TransformPoint(GetCamCornerPosition);
        return new Vector3(Mathf.Clamp(transform.position.x, localMinCameraCorner.x + 0.5f, localMaxCameraCorner.x - 0.5f),
            Mathf.Clamp(transform.position.y, localMinCameraCorner.y + 0.5f, localMaxCameraCorner.y - 0.5f),
            0);
    }
}
