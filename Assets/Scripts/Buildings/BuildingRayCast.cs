using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.InputSystem;

public class BuildingRayCast : MonoBehaviour
{
    private RaycastHit _raycastHit;
    [SerializeField] GameObject _UICanvas;
    [SerializeField] LayerMask _layerMask;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        if(Input.GetMouseButtonDown(0)){
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out _raycastHit, Mathf.Infinity)){
                if(_raycastHit.collider.CompareTag("Building")){
                    _UICanvas.SetActive(true);
                }
            
            }
        }
    }
    
}
