using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARRaycastManager))]

public class ARTapToPlaceObject : MonoBehaviour
{
    public GameObject gameObjectToInstantiate;

    private GameObject spawnedObject;
    private ARRaycastManager _arRaycastManager;
    private Vector2 touchPosition;
    private float initalScale = 1.0f;
    private float startPinchScale = 0.0f;
    private bool isPinching = false;
    static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    private void Awake() {
        _arRaycastManager = GetComponent<ARRaycastManager>();

    }

    void pinchZoom() {
        Vector2 touch0, touch1;
        float distance;
        touch0 = Input.GetTouch(0).position;
        touch1 = Input.GetTouch(1).position;
        distance = Vector2.Distance(touch0, touch1);

        float scale = distance / 859;

        if (!isPinching) {
            startPinchScale = scale;
        }

        scale = (scale / startPinchScale) * initalScale;
        spawnedObject.transform.localScale = new Vector3(scale,scale,scale);
    }

    bool TryGetTouchPosition(out Vector2 touchPosition) {
        if (Input.touchCount > 0) {
            touchPosition = Input.GetTouch(0).position;
            return true;
        }

        touchPosition = default;
        return false;
    }

    // Update is called once per frame
    void Update() {
        if (!TryGetTouchPosition(out Vector2 touchPosition)) {
            isPinching = false;
            initalScale = spawnedObject.transform.localScale.x;
            return;
        }

        if (Input.touchCount > 1) {
            // SCALE OBJECT
            if (spawnedObject != null) {
                pinchZoom();
                isPinching = true;
            }
        } else {
            // MOVE OBJECT

            // Prevent moving if user is releasing pinch grip
            if (isPinching) {
                return;
            }

            // TrackableType.PlaneWithinPolygon = the type we're looking for
            if (_arRaycastManager.Raycast(touchPosition, hits, TrackableType.PlaneWithinPolygon)) {
                var hitPose = hits[0].pose;

                if (spawnedObject == null) {
                    spawnedObject = Instantiate(gameObjectToInstantiate, hitPose.position, hitPose.rotation);
                } else {
                    spawnedObject.transform.position = hitPose.position;
                }
            }
        }
    }
}
