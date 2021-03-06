﻿using UnityEngine;

public class MapTileController : MonoBehaviour
{
    [SerializeField] private float zoomTime;
    [SerializeField] private float minimumDragDistance;
    [SerializeField] private float minimumDragTime;
    [SerializeField] private float screenOffset;
    [SerializeField] private bool zoomable;
    [SerializeField] private bool discardable;
    [SerializeField] private GameObject highlightObject;
    static bool aTileIsZoomed;
    bool isRotating = false, beingHeld = false, wasMoved = false, isZoomed = false, highlighted = false;
    bool canBeDragged => !isZoomed && !isRotating;
    bool insideBoard => !(transform.localPosition.x < -4.5f || transform.localPosition.x > 3.5f || transform.localPosition.y < -4.5f || transform.localPosition.y > 3.5f);
    float dragStartTime;
    Vector2 mouseDragStartPosition;
    private BoardManager boardManager;


    private Vector2 cameraRealSize => new Vector2(Camera.main.orthographicSize * 2 * Camera.main.aspect, Camera.main.orthographicSize * 2);
    private Vector3 posBeforeZoom;
    private Vector3 GetCamCornerPosition => new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, 0) - (Vector3)cameraRealSize / 2;
    private Vector2 GetworldMousePosition()
        => Camera.main.ScreenToWorldPoint(Input.mousePosition);

    private void Start() {
        boardManager = BoardManager._instance;
    }
    // Update is called once per frame
    void Update() {
        if (beingHeld && canBeDragged) {
            SetHighlight(boardManager.CheckDiscard(transform.position) && discardable);
            if (!wasMoved && (Time.time - dragStartTime) >= minimumDragTime)
                wasMoved = true;
            else if (!wasMoved && Vector2.Distance(GetworldMousePosition(), mouseDragStartPosition) >= minimumDragDistance)
                wasMoved = true;
            if(wasMoved)
                transform.position = (Vector3)GetworldMousePosition() + Vector3.forward * transform.position.z;
            if (wasMoved && insideBoard)
                transform.localPosition = new Vector3(Mathf.Round(transform.localPosition.x), Mathf.Round(transform.localPosition.y), transform.localPosition.z);
        }
        if (isZoomed && !isRotating && Input.GetKeyUp(KeyCode.Mouse0))
            ZoomOut();
        if (beingHeld && Input.GetKeyUp(KeyCode.Mouse0))
            StopDrag();

    }

    private void SetHighlight(bool state) {
        if (state && !highlighted) {
            highlightObject.SetActive(true);
        }
        else if (!state && highlighted) {
            highlightObject.SetActive(false);
        }
        highlighted = state;
    }
    private void OnMouseDown() {
        if (canBeDragged) {
            StartDrag();
        }
    }

    public void StartDrag() {
        mouseDragStartPosition = (Vector3)GetworldMousePosition();
        dragStartTime = Time.time;
        beingHeld = true;
        wasMoved = false;
    }

    private void OnMouseUp() {
        StopDrag();
    }

    private void StopDrag() {
        beingHeld = false;
        if (insideBoard)
            boardManager.PlaySnapSound();

        if (highlighted)
            Destroy(gameObject);
        Debug.Log("!wasMoved: " + !wasMoved + ", zoomable: " + zoomable + ", !isRotating: " + !isRotating + ", !aTileIsZoomed: " + !aTileIsZoomed + ", !isZoomed: " + !isZoomed);
        if (!wasMoved && zoomable && !isRotating && !aTileIsZoomed && !isZoomed)
            ZoomIn();
    }

    public void ZoomIn() {
        isRotating = true;
        aTileIsZoomed = true;
        posBeforeZoom = transform.localPosition;
        if (insideBoard)
            posBeforeZoom = Vector3Int.RoundToInt(posBeforeZoom);
        LTDescr rotationLean = LeanTween.rotate(gameObject, transform.rotation.eulerAngles + Vector3.up * 180, zoomTime);
        LeanTween.scale(gameObject, transform.localScale * Camera.main.orthographicSize * 2 * (1 - screenOffset), zoomTime);
        LeanTween.move(gameObject, new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, Camera.main.nearClipPlane), zoomTime).setEaseOutSine();
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
