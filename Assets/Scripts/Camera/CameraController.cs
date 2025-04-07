using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class CameraController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float panSpeed = 20f;
    public float zoomSpeedTouch = 0.1f;
    public float zoomSpeedMouse = 10f;
    public float panLerpSpeed = 10f;
    public float zoomLerpSpeed = 10f;

    [Header("Zoom Limits")]
    public float minZoom = 20f;
    public float maxZoom = 80f;

    [Header("Camera Bounds (X,Z)")]
    public float minX = -50f;
    public float maxX =  50f;
    public float minZ = -50f;
    public float maxZ =  50f;

    private Vector3 lastPanPosition;
    private int panTouchId;
    private bool isPanning;
    private bool isZooming;

    private Camera cam;

    private Vector3 targetPosition;
    private float targetZoom;

    void Start()
    {
        cam = GetComponent<Camera>();
        targetPosition = transform.position;
        if (cam.orthographic)
            targetZoom = cam.orthographicSize;
        else
            targetZoom = cam.fieldOfView;
    }

    void Update()
    {
        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed)
        {
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject(Touchscreen.current.primaryTouch.touchId.ReadValue()))
                return;
        }
        else if (Mouse.current != null)
        {
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
                return;
        }

        if (Touchscreen.current != null)
        {
            var touches = Touchscreen.current.touches;
            int activeTouchCount = 0;
            foreach (var touch in touches)
            {
                if (touch.press.isPressed)
                    activeTouchCount++;
            }

            if (activeTouchCount >= 2)
            {
                isPanning = false;
                isZooming = true;

                var activeTouches = new System.Collections.Generic.List<TouchControl>();
                foreach (var touch in touches)
                {
                    if (touch.press.isPressed)
                        activeTouches.Add(touch);
                }
                if (activeTouches.Count >= 2)
                {
                    var touch0 = activeTouches[0];
                    var touch1 = activeTouches[1];

                    Vector2 touch0Pos = touch0.position.ReadValue();
                    Vector2 touch1Pos = touch1.position.ReadValue();

                    Vector2 touch0PrevPos = touch0Pos - touch0.delta.ReadValue();
                    Vector2 touch1PrevPos = touch1Pos - touch1.delta.ReadValue();

                    float prevTouchDeltaMag = (touch0PrevPos - touch1PrevPos).magnitude;
                    float touchDeltaMag = (touch0Pos - touch1Pos).magnitude;
                    float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

                    UpdateZoom(deltaMagnitudeDiff * zoomSpeedTouch);
                }
            }
            else if (activeTouchCount == 1)
            {
                isZooming = false;
                foreach (var touch in touches)
                {
                    if (touch.press.isPressed)
                    {
                        if (touch.phase.ReadValue() == UnityEngine.InputSystem.TouchPhase.Began)
                        {
                            lastPanPosition = touch.position.ReadValue();
                            panTouchId = touch.touchId.ReadValue();
                        }
                        else if (touch.touchId.ReadValue() == panTouchId && touch.phase.ReadValue() == UnityEngine.InputSystem.TouchPhase.Moved)
                        {
                            PanCamera(touch.position.ReadValue());
                        }
                    }
                }
            }
        }
        if (Mouse.current != null)
        {
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                isPanning = true;
                lastPanPosition = Mouse.current.position.ReadValue();
            }
            else if (Mouse.current.leftButton.isPressed && isPanning)
            {
                PanCamera(Mouse.current.position.ReadValue());
            }
            else if (Mouse.current.leftButton.wasReleasedThisFrame)
            {
                isPanning = false;
            }

            float scroll = Mouse.current.scroll.ReadValue().y;
            if (Mathf.Abs(scroll) > 0.001f)
            {
                UpdateZoom(-scroll * zoomSpeedMouse);
            }
        }

        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * panLerpSpeed);

        if (cam.orthographic)
        {
            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetZoom, Time.deltaTime * zoomLerpSpeed);
        }
        else
        {
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, targetZoom, Time.deltaTime * zoomLerpSpeed);
        }
    }

    void PanCamera(Vector3 newPanPosition)
    {
        Vector3 offset = cam.ScreenToViewportPoint(lastPanPosition - newPanPosition);
        Vector3 move = new Vector3(offset.x * panSpeed, 0f, offset.y * panSpeed);

        targetPosition += move;
        targetPosition.x = Mathf.Clamp(targetPosition.x, minX, maxX);
        targetPosition.z = Mathf.Clamp(targetPosition.z, minZ, maxZ);

        lastPanPosition = newPanPosition;
    }

    void UpdateZoom(float delta)
    {
        if (cam.orthographic)
        {
            targetZoom = cam.orthographicSize + delta;
            targetZoom = Mathf.Clamp(targetZoom, minZoom, maxZoom);
        }
        else
        {
            targetZoom = cam.fieldOfView + delta;
            targetZoom = Mathf.Clamp(targetZoom, minZoom, maxZoom);
        }
    }
}
