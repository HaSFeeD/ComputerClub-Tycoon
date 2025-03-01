using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float panSpeed = 20f;
    public float zoomSpeedTouch = 0.1f;
    public float zoomSpeedMouse = 10f;

    [Header("Zoom Limits")]
    public float minZoom = 20f;
    public float maxZoom = 80f;

    [Header("Camera Bounds (X,Z)")]
    public float minX = -50f;
    public float maxX =  50f;
    public float minZ = -50f;
    public float maxZ =  50f;

    private Vector3 lastPanPosition;
    private int panFingerId; 

    private bool isPanning;
    private bool isZooming;

    private Camera cam;
    private LayerMask layerMask;

    void Start()
    {
        layerMask = LayerMask.GetMask("UI");
        cam = GetComponent<Camera>();
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
                return;
        }
        else
        {
            if (EventSystem.current.IsPointerOverGameObject())
                return;
        }

        if (Input.touchCount == 2)
        {
            isPanning = false;
            isZooming = true;

            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

            Zoom(deltaMagnitudeDiff * zoomSpeedTouch);
        }
        else if (Input.touchCount == 1)
        {
            isZooming = false;

            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                lastPanPosition = touch.position;
                panFingerId = touch.fingerId;
            }
            else if (touch.fingerId == panFingerId && touch.phase == TouchPhase.Moved)
            {
                PanCamera(touch.position);
            }
        }
        else
        {
            isZooming = false;

            if (Input.GetMouseButtonDown(0))
            {
                isPanning = true;
                lastPanPosition = Input.mousePosition;
            }
            else if (Input.GetMouseButton(0) && isPanning)
            {
                PanCamera(Input.mousePosition);
            }
            else if (Input.GetMouseButtonUp(0))
            {
                isPanning = false;
            }

            float scroll = Input.GetAxis("Mouse ScrollWheel");
            if (Mathf.Abs(scroll) > 0.001f)
            {
                Zoom(-scroll * zoomSpeedMouse);
            }
        }
    }

    void PanCamera(Vector3 newPanPosition)
    {
        Vector3 offset = cam.ScreenToViewportPoint(lastPanPosition - newPanPosition);

        Vector3 move = new Vector3(offset.x * panSpeed, 0f, offset.y * panSpeed);

        transform.Translate(move, Space.World);

        Vector3 clampedPos = transform.position;
        clampedPos.x = Mathf.Clamp(clampedPos.x, minX, maxX);
        clampedPos.z = Mathf.Clamp(clampedPos.z, minZ, maxZ);
        transform.position = clampedPos;

        lastPanPosition = newPanPosition;
    }

    void Zoom(float delta)
    {
        if (cam.orthographic)
        {
            cam.orthographicSize += delta;
            cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, minZoom, maxZoom);
        }
        else
        {
            cam.fieldOfView += delta;
            cam.fieldOfView = Mathf.Clamp(cam.fieldOfView, minZoom, maxZoom);
        }
    }
}
