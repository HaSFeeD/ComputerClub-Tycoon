using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class CameraController1 : MonoBehaviour
{
    public Camera mainCamera;
    public float panSpeed = 20f;
    public float panSmoothTime = 0.2f;
    public Vector2 panLimitMin;
    public Vector2 panLimitMax;
    public float zoomSpeed = 10f;
    public float minFOV = 15f;
    public float maxFOV = 90f;
    private Vector3 targetCameraPos;
    private Vector2 lastDragPos;
    private bool isDragging;
    private bool dragStartedOnUI;
    private bool hasDragged;
    private InputAction dragAction;
    private InputAction tapAction;
    private InputAction zoomScrollAction;
    private float targetFOV;
    private bool isPinching;
    private float previousPinchDistance;

    private void Awake()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;
        targetCameraPos = transform.position;
        targetFOV = mainCamera.fieldOfView;
        dragAction = new InputAction("Drag", binding: "<Pointer>/position");
        dragAction.started += OnDragStarted;
        dragAction.performed += OnDragPerformed;
        dragAction.canceled += OnDragCanceled;
        dragAction.Enable();
        tapAction = new InputAction("Tap", InputActionType.Button);
        tapAction.AddBinding("<Pointer>/press").WithInteraction("tap");
        tapAction.Enable();
        zoomScrollAction = new InputAction("ZoomScroll", binding: "<Mouse>/scroll");
        zoomScrollAction.Enable();
    }

    private void OnDestroy()
    {
        dragAction.Disable();
        tapAction.Disable();
        zoomScrollAction.Disable();
    }

    private void OnDragStarted(InputAction.CallbackContext ctx)
    {
        lastDragPos = ctx.ReadValue<Vector2>();
        dragStartedOnUI = EventSystem.current != null && EventSystem.current.IsPointerOverGameObject();
        isDragging = true;
        hasDragged = false;
    }

    private void OnDragPerformed(InputAction.CallbackContext ctx)
    {
        if (!isDragging || dragStartedOnUI)
            return;
        Vector2 currentPos = ctx.ReadValue<Vector2>();
        Vector2 delta = currentPos - lastDragPos;
        if (delta.magnitude > 1f)
            hasDragged = true;
        lastDragPos = currentPos;
        bool pointerPressed = (Mouse.current != null && Mouse.current.leftButton.isPressed)
                              || (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed);
        if (!pointerPressed)
            return;
        Vector3 viewportDelta = mainCamera.ScreenToViewportPoint(new Vector3(delta.x, delta.y, 0));
        Vector3 move = new Vector3(-viewportDelta.x, 0f, -viewportDelta.y) * panSpeed;
        targetCameraPos += move;
        targetCameraPos.x = Mathf.Clamp(targetCameraPos.x, panLimitMin.x, panLimitMax.x);
        targetCameraPos.z = Mathf.Clamp(targetCameraPos.z, panLimitMin.y, panLimitMax.y);
    }

    private void OnDragCanceled(InputAction.CallbackContext ctx)
    {
        isDragging = false;
        dragStartedOnUI = false;
    }

    private void Update()
    {
        if (Mouse.current != null && !(EventSystem.current != null && EventSystem.current.IsPointerOverGameObject()))
        {
            Vector2 scrollValue = zoomScrollAction.ReadValue<Vector2>();
            if (scrollValue.y != 0)
            {
                targetFOV -= scrollValue.y * zoomSpeed * Time.deltaTime;
                targetFOV = Mathf.Clamp(targetFOV, minFOV, maxFOV);
            }
        }
        if (Touchscreen.current != null)
        {
            var touches = Touchscreen.current.touches;
            if (touches.Count >= 2 && touches[0].isInProgress && touches[1].isInProgress)
            {
                if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
                {
                }
                else
                {
                    Vector2 touch0Pos = touches[0].position.ReadValue();
                    Vector2 touch1Pos = touches[1].position.ReadValue();
                    float currentDistance = Vector2.Distance(touch0Pos, touch1Pos);
                    if (!isPinching)
                    {
                        previousPinchDistance = currentDistance;
                        isPinching = true;
                    }
                    else
                    {
                        float pinchDelta = previousPinchDistance - currentDistance;
                        targetFOV -= pinchDelta * zoomSpeed * Time.deltaTime;
                        targetFOV = Mathf.Clamp(targetFOV, minFOV, maxFOV);
                        previousPinchDistance = currentDistance;
                    }
                }
            }
            else
            {
                isPinching = false;
            }
        }
        transform.position = Vector3.Lerp(transform.position, targetCameraPos, Time.deltaTime / panSmoothTime);
        mainCamera.fieldOfView = Mathf.Lerp(mainCamera.fieldOfView, targetFOV, Time.deltaTime / panSmoothTime);
    }
}
