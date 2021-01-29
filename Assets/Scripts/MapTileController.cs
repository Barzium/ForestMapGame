using UnityEngine;

public class MapTileController : MonoBehaviour
{
    [SerializeField] private bool spawner;
    [SerializeField] private float zoomTime;
    [SerializeField] private float minimumDragDistance;
    [SerializeField] private float minimumDragTime;
    [SerializeField] private float screenOffset;
    [SerializeField] private bool zoomable;
    static bool aTileIsZoomed;
    bool isRotating = false, beingHeld = false, wasMoved = false, isZoomed = false;
    bool canBeDragged => !isZoomed && !isRotating;
    bool insideBoard => !(transform.localPosition.x < -4.5f || transform.localPosition.x > 3.5f || transform.localPosition.y < -4.5f || transform.localPosition.y > 3.5f);
    float dragStartTime;
    Vector2 mouseDragStartPosition;
    Vector3 dragStartPos;
    private static Camera mainCamera;


    private Vector2 cameraRealSize;
    private Vector2 posBeforeZoom;
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
    }

    // Update is called once per frame
    void Update() {
        if (beingHeld && canBeDragged) {
            transform.position = dragStartPos + (Vector3)(GetworldMousePosition() - mouseDragStartPosition);
            if (!wasMoved && (Time.time - dragStartTime) >= minimumDragTime)
                wasMoved = true;
            else if (!wasMoved && Vector2.Distance(transform.position, dragStartPos) >= minimumDragDistance)
                wasMoved = true;
            if (wasMoved && insideBoard)
                transform.localPosition = new Vector3(Mathf.Round(transform.localPosition.x), Mathf.Round(transform.localPosition.y), transform.localPosition.z);
        }
        if (isZoomed && !isRotating && Input.GetKeyUp(KeyCode.Mouse0))
            ZoomOut();
    }

    private void OnMouseDown() {
        if (canBeDragged) {
            mouseDragStartPosition = GetworldMousePosition();
            dragStartPos = transform.position;
            dragStartTime = Time.time;
            beingHeld = true;
            wasMoved = false;
        }
    }
    private void OnMouseUp() {
        beingHeld = false;
        if (!wasMoved && zoomable && !isRotating && !aTileIsZoomed && !isZoomed)
            ZoomIn();
    }
    private void ZoomIn() {
        isRotating = true;
        aTileIsZoomed = true;
        posBeforeZoom = transform.localPosition;
        if (insideBoard)
            posBeforeZoom = Vector2Int.RoundToInt(posBeforeZoom);
        LTDescr rotationLean = LeanTween.rotate(gameObject, transform.rotation.eulerAngles + Vector3.up * 180, zoomTime);
        LeanTween.scale(gameObject, transform.localScale * GetCameraRealSize.y * (1 - screenOffset), zoomTime);
        LeanTween.move(gameObject, new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y, mainCamera.nearClipPlane), zoomTime).setEaseOutSine();
        rotationLean.setOnComplete(() => {
            isRotating = false;
            isZoomed = true;
        });
    }
    private void ZoomOut() {
        isRotating = true;
        LTDescr rotationLean = LeanTween.rotate(gameObject, transform.rotation.eulerAngles + Vector3.up * 180, zoomTime);
        LeanTween.scale(gameObject, Vector3.one, zoomTime);
        LeanTween.moveLocal(gameObject, posBeforeZoom, zoomTime).setEaseInSine();
        rotationLean.setOnComplete(() => {
            isRotating = false;
            isZoomed = false;
            aTileIsZoomed = false;
        });
    }

    private Vector3 ClampToScreen() {
        Vector3 localMaxCameraCorner = transform.TransformPoint(GetCamCornerPosition);
        Vector3 localMinCameraCorner = transform.TransformPoint(GetCamCornerPosition);
        return new Vector3(Mathf.Clamp(transform.position.x, localMinCameraCorner.x + 0.5f, localMaxCameraCorner.x - 0.5f),
            Mathf.Clamp(transform.position.y, localMinCameraCorner.y + 0.5f, localMaxCameraCorner.y - 0.5f),
            0);
    }
}
