using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public interface IRoomClickHandler
{
    void HandleRoomClick(Vector2 clickPosition);
}

public class ClickManager : MonoBehaviour
{
    public Camera mainCamera;
    private InputAction clickAction;

    private Vector2 pointerDownPosition;
    private Vector2 pointerUpPosition;
    private bool shouldProcessClick = false;

    private float clickThreshold = 10f;
    [SerializeField] private GameObject upgradePanel; 

    private void Awake()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;

        clickAction = new InputAction("Click", binding: "<Pointer>/press");
        clickAction.started += ctx => pointerDownPosition = Pointer.current.position.ReadValue();
        clickAction.canceled += ctx => 
        {
            pointerUpPosition = Pointer.current.position.ReadValue();
            shouldProcessClick = true;
        };
        clickAction.Enable();
    }

    private void Update()
    {
        if (!shouldProcessClick)
            return;

        shouldProcessClick = false;

        if (Vector2.Distance(pointerDownPosition, pointerUpPosition) > clickThreshold)
            return;

        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            return;
        if (EventSystem.current != null && !EventSystem.current.IsPointerOverGameObject())
            if(upgradePanel.activeSelf == true){
                upgradePanel.SetActive(false);
                return;
            }
        Ray ray = mainCamera.ScreenPointToRay(pointerUpPosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            IRoomClickHandler roomHandler = hit.collider.gameObject.GetComponent<IRoomClickHandler>();
            if (roomHandler != null)
                roomHandler.HandleRoomClick(pointerUpPosition);
        }
    }

    private void OnDestroy()
    {
        clickAction.Disable();
    }
}