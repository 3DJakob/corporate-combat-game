using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARRaycastManager))]

public class ARTapToPlaceObject : MonoBehaviour
{
    public GameObject tableTop;
    public GameObject tableLeg;

    public GameObject spawnedTableTop;
    public GameObject spawnedTableLeg1;
    public GameObject spawnedTableLeg2;
    public GameObject spawnedTableLeg3;
    public GameObject spawnedTableLeg4;

    public bool running;

    private ARRaycastManager _arRaycastManager;
    private Vector2 touchPosition;
    private float initialScale = 1.0f;
    private float startPinchScale = 0.0f;
    private float initalAngle = 0.0f;
    private float startPinchAngle = 0.0f;
    private bool isPinching = false;
    private bool newPinchGrip = true;
    static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    private GameObject parentObject;
    
    // 3D Model config
    private static float legHeight = 2.0f;
    private static float tableWidth = 3.0f;
    private static float tableLenght = 4.0f;

    private float tableHeight = legHeight;

    private void Awake() {
        _arRaycastManager = GetComponent<ARRaycastManager>();

    }

    public void updateHeight(float value) {
        Debug.Log(value);
        tableHeight = value;
        if (spawnedTableTop != null) {
            // spawnedTableTop.transform.localScale = new Vector3(
            //     spawnedTableTop.transform.localScale.x,
            //     tableHeight,
            //     spawnedTableTop.transform.localScale.z
            // );
            spawnedTableTop.transform.localPosition = new Vector3(0, tableHeight, 0);
            spawnedTableLeg1.transform.localScale = new Vector3(1.0f, tableHeight/legHeight, 1.0f);
            spawnedTableLeg2.transform.localScale = new Vector3(1.0f, tableHeight/legHeight, 1.0f);
            spawnedTableLeg3.transform.localScale = new Vector3(1.0f, tableHeight/legHeight, 1.0f);
            spawnedTableLeg4.transform.localScale = new Vector3(1.0f, tableHeight/legHeight, 1.0f);
        }
    }

    void pinchZoom() {
        Vector2 touch0, touch1;
        float distance;
        touch0 = Input.GetTouch(0).position;
        touch1 = Input.GetTouch(1).position;
        distance = Vector2.Distance(touch0, touch1);



        float angle = -Vector2.SignedAngle(new Vector2(1.0f, 0.0f), touch1 - touch0);
        float scale = distance / 859;

        if (newPinchGrip) {
            startPinchScale = scale;
            startPinchAngle = angle;
            newPinchGrip = false;
        }

        scale = (scale / startPinchScale) * initialScale;
        angle = initalAngle + (angle - startPinchAngle);
        spawnedTableTop.transform.localScale = new Vector3(scale, 1.0f,scale);

        parentObject.transform.eulerAngles = new Vector3(
            parentObject.transform.eulerAngles.x,
            angle,
            parentObject.transform.eulerAngles.z
        );

        UpdateTableLegsPositions();
    }

    bool TryGetTouchPosition(out Vector2 touchPosition) {
        if (Input.touchCount > 0) {
            touchPosition = Input.GetTouch(0).position;
            return true;
        }

        touchPosition = default;
        return false;
    }

    void UpdateTableLegsPositions() {
        float scaleFactor = spawnedTableTop.transform.localScale.x;
        spawnedTableLeg1.transform.localPosition = new Vector3(tableWidth * scaleFactor / 2, 0, tableLenght * scaleFactor / 2);
        spawnedTableLeg2.transform.localPosition = new Vector3(-tableWidth * scaleFactor / 2, 0, tableLenght * scaleFactor / 2);
        spawnedTableLeg3.transform.localPosition = new Vector3(tableWidth * scaleFactor / 2, 0, -tableLenght * scaleFactor / 2);
        spawnedTableLeg4.transform.localPosition = new Vector3(-tableWidth * scaleFactor / 2, 0, -tableLenght * scaleFactor / 2);
    }

    // Update is called once per frame
    void Update() {
        if(running){
            // Debug.Log("looping");
            if (!TryGetTouchPosition(out Vector2 touchPosition)) {
                isPinching = false;
                if (parentObject != null) {
                    initialScale = spawnedTableTop.transform.localScale.x;
                    initalAngle = parentObject.transform.eulerAngles.y;
                }
                newPinchGrip = true;
                return;
            }

            if (Input.touchCount > 1) {
                // SCALE OBJECT
                if (parentObject != null) {
                    pinchZoom();
                    isPinching = true;
                }
            } else {
                // MOVE OBJECT

                // Prevent moving if user is releasing pinch grip
                if (isPinching) {
                    if (parentObject != null) {
                        initialScale = spawnedTableTop.transform.localScale.x;
                        initalAngle = parentObject.transform.eulerAngles.y;
                        newPinchGrip = true;
                    }
                    return;
                }

                // TrackableType.PlaneWithinPolygon = the type we're looking for
                if (_arRaycastManager.Raycast(touchPosition, hits, TrackableType.PlaneWithinPolygon)) {
                    var hitPose = hits[0].pose;

                    if (parentObject == null) {
                        parentObject = Instantiate(new GameObject(), hitPose.position, hitPose.rotation);

                        spawnedTableLeg1 = Instantiate(tableLeg, parentObject.transform, false);
                        spawnedTableLeg2 = Instantiate(tableLeg, parentObject.transform, false);
                        spawnedTableLeg3 = Instantiate(tableLeg, parentObject.transform, false);
                        spawnedTableLeg4 = Instantiate(tableLeg, parentObject.transform, false);
                        spawnedTableTop = Instantiate(tableTop, parentObject.transform, false);
                        UpdateTableLegsPositions();
                        spawnedTableTop.transform.localPosition = new Vector3(0, legHeight, 0);

                        // spawnedTableTop.transform.parent = parentObject.transform;
                        // spawnedTableTop.transform.position = new Vector3(spawnedTableTop.transform.position.x, spawnedTableTop.transform.position.y, spawnedTableTop.transform.position.z);
                    } else {
                        parentObject.transform.position = new Vector3(hitPose.position.x, hitPose.position.y, hitPose.position.z);
                    }
                }
            }

            if (spawnedTableTop != null) {
                PlayerInfo.PI.updateOrigin(spawnedTableTop.transform);
            }
        }

        if (spawnedObject != null) {
            PlayerInfo.PI.updateOrigin(spawnedObject.transform.position, spawnedObject.transform.rotation, spawnedObject.transform.localScale);
        }

    }
}
