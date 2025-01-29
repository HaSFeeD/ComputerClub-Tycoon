using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraControl : MonoBehaviour
{
    [SerializeField]
    private Canvas _canvas;
    [SerializeField]
    private Camera _camera = null;
    [SerializeField]

    [Header("Camera Moving Configurations")]
    private float _moveSpeed = 50;
    [SerializeField]
    private float _moveSmooth = 5;
    [Header("Camera Zoom Configurations")]
    [SerializeField]
    private float _zoomSpeed = 5f;
    [SerializeField]
    private float _zoomSmooth = 5f;


    [Header("Camera Bound Limits")]
    [SerializeField]
    private float _zMin = 0;
    [SerializeField] 
    private float _zMax = 100;
    [SerializeField]
    private float _xMin = 0;
    [SerializeField]
    private float _xMax = 100;


    [Header("Camera Zoom Limits")]
    [SerializeField]
    private float _zoomMin = 1f;
    [SerializeField]
    private float _zoomMax = 10f;
    private UnityEngine.Vector2 _zoomPositionOnScreen = UnityEngine.Vector2.zero;
    private UnityEngine.Vector3 _zoomPositionInWorld = UnityEngine.Vector3.zero;
    private float _zoomBaseValue = 0;
    private float _zoomBaseDistance = 0;


    private UnityEngine.Vector3 _position;
    private Controls _inputs;


    private bool _zooming = false;
    private bool _moving = false;


    private Transform _target = null;
    private Transform _pivot = null;
    private Transform _root = null;

    private UnityEngine.Vector3 _center = UnityEngine.Vector3.zero;

    private float _right = 10f;
    private float _up = 10f;
    private float _left = 10f;
    private float _down = 10f;
    private float _angle = 45f;
    private float _zoom = 5f;
    private void Awake(){
    if (_camera == null)
    {
        Debug.LogError("Camera is not assigned in the CameraController script.");
        return;
    }
    

    _inputs = new Controls();
    _root = new GameObject("CameraHelper").transform;
    _pivot = new GameObject("CameraPivot").transform;
    _target = new GameObject("CameraTarget").transform;
    
    _position = _camera.transform.position;

    _camera.orthographic = true;
    _camera.nearClipPlane = 0;
    Initialize(UnityEngine.Vector3.zero, 100, 100, 100, 100, 45, 5, 3, 10);
}

    private void Start(){
        
    }
    public void Initialize(UnityEngine.Vector3 center, float right, float left, float up, float down, float angle, float zoom, float zoomMin, float zoomMax){
        _center = center;
        _right = right;
        _up = up;
        _left = left;
        _angle = angle;

        _zoom = zoom;
        _zoomMin = zoomMin;
        _zoomMax = zoomMax;

        _camera.orthographicSize = _zoom;

        _zooming = false;
        _moving = false;
        _pivot.SetParent(_root);
        _target.SetParent(_pivot);

        _root.position = center;
        _root.localEulerAngles = UnityEngine.Vector3.zero;
        AdjustBounds();

        _pivot.localPosition = UnityEngine.Vector3.zero;
        _pivot.localEulerAngles = new UnityEngine.Vector3(_angle, 0, 0);

        _target.localPosition = new UnityEngine.Vector3(0, 0, -500);
        _target.localEulerAngles = UnityEngine.Vector3.zero;
    }

    private void OnEnable(){
        _inputs.Enable();
        _inputs.Main.Move.started += _ => MoveStarted();
        _inputs.Main.Move.canceled += _ => MoveCanceled();
        _inputs.Main.TouchZoom.started += _ => ZoomStarted();
        _inputs.Main.TouchZoom.canceled += _ => ZoomCanceled();
    }
    private void OnDisable(){
        _inputs.Disable();
        _inputs.Main.Move.started += _ => MoveStarted();
        _inputs.Main.Move.canceled += _ => MoveCanceled();
        _inputs.Main.TouchZoom.started += _ => ZoomStarted();
        _inputs.Main.TouchZoom.canceled += _ => ZoomCanceled();
    }
    private void MoveStarted()
    {
        _moving = true;
    }
    private void ZoomStarted()
    {
        UnityEngine.Vector2 touch0 = _inputs.Main.TouchPosition0.ReadValue<UnityEngine.Vector2>();
        UnityEngine.Vector2 touch1 = _inputs.Main.TouchPosition1.ReadValue<UnityEngine.Vector2>();
        _zoomPositionOnScreen = UnityEngine.Vector2.Lerp(touch0, touch1, 0.5f);
        _zoomPositionInWorld = CameraScreenPositionToWorldPosition(_zoomPositionOnScreen);
        _zoomBaseValue = _zoom;

        touch0.x /= Screen.width;
        touch1.x /= Screen.width;

        touch0.y /= Screen.height;
        touch1.y /= Screen.height;
        _zooming = true;
    }
    private void ZoomCanceled(){
        _zooming = false;
    }
    private void MovePressed(){
        if(_canvas.isActiveAndEnabled){
         _moving = true;   
        }
    }
    private void MoveCanceled(){
        _moving = false;
        _target.position = _camera.transform.position; 
}

    private void Update(){
        if(Input.touchSupported == false){
            float mouseScroll = _inputs.Main.MouseScroll.ReadValue<float>();
            if(mouseScroll > 0){
                _zoom -= _zoomSpeed * Time.deltaTime;
            }
            else if(mouseScroll < 0){
                _zoom += _zoomSpeed *Time.deltaTime;
            }
        _camera.orthographicSize = _zoom;
        }
        AdjustBounds();
        if(_zooming){
            UnityEngine.Vector2 touch0 = _inputs.Main.TouchPosition0.ReadValue<UnityEngine.Vector2>();
            UnityEngine.Vector2 touch1 = _inputs.Main.TouchPosition1.ReadValue<UnityEngine.Vector2>();

            touch0.x /= Screen.width;
            touch1.x /= Screen.width;
            touch0.y /= Screen.height;
            touch1.y /= Screen.height;

            float currentDistance = UnityEngine.Vector2.Distance(touch0, touch1);
            float deltaDistance = currentDistance - _zoomBaseDistance;
            _zoom = _zoomBaseDistance - (deltaDistance * _zoomSpeed);

            UnityEngine.Vector3 zoomCenter = CameraScreenPositionToWorldPosition(_zoomPositionOnScreen);
            _root.position += (_zoomPositionInWorld - zoomCenter);
            }
        else if(_moving){
            CameraMoving();
        }
        
        PositionLimits();
    }
    private void CameraMoving(){
        if(_moving){
            UnityEngine.Vector2 move = _inputs.Main.MoveDelta.ReadValue<UnityEngine.Vector2>();
            if(move != UnityEngine.Vector2.zero)
            {
                move.x /= Screen.width;
                move.y /= Screen.height;
                _root.position -= _root.right.normalized * move.x * _moveSpeed;
                _root.position -= _root.forward.normalized * move.y * _moveSpeed;
            }
        }
        if(_camera.orthographicSize != _zoom)
        {
            _camera.orthographicSize = Mathf.Lerp(_camera.orthographicSize, _zoom, _zoomSmooth * Time.deltaTime);
        }
        if(_camera.transform.position != _target.position)
        {
            _camera.transform.position = UnityEngine.Vector3.Lerp(_camera.transform.position, _target.position, _moveSmooth * Time.deltaTime);
        }
        if(_camera.transform.rotation != _target.rotation)
        {
            _camera.transform.rotation = _target.rotation;
        }
    }

    private void AdjustBounds(){
        if (_zoom < _zoomMin)
        {
            _zoom = _zoomMin;
        }
        if (_zoom > _zoomMax)
        {
            _zoom = _zoomMax;
        }
    }
    private UnityEngine.Vector3 CameraScreenPositionToWorldPosition(UnityEngine.Vector2 position)
        {
            float h = _camera.orthographicSize * 2f;
            float w = _camera.aspect * h;
            UnityEngine.Vector3 ancher = _camera.transform.position - (_camera.transform.right.normalized * w / 2f) - (_camera.transform.up.normalized * h / 2f);
            UnityEngine.Vector3 world = ancher + (_camera.transform.right.normalized * position.x / Screen.width * w) + (_camera.transform.up.normalized * position.y / Screen.height * h);
            world.z = 0;
            return world;
        }
    
    private void PositionLimits(){
        _position.x = Mathf.Clamp(_camera.transform.position.x, _xMin, _xMax);
        _position.z = Mathf.Clamp(_camera.transform.position.z, _zMin, _zMax);
        
        _camera.transform.position = _position;
    }

    
}