using UnityEngine;
using UnityEngine.UI;

public class EventButtonScript : MonoBehaviour
{
    private Button myButton;
    private GameObject mainCanvas;
    private GameObject panelToOpen;

    private void Awake()
    {
        myButton = GetComponent<Button>();
        if (myButton != null)
        {
            myButton.onClick.AddListener(OnButtonClick);
        }
    }
    public void Setup(GameObject canvas, GameObject panel)
    {
        mainCanvas = canvas;
        panelToOpen = panel;
    }

    private void OnButtonClick()
    {
        if (mainCanvas != null)
        {
            mainCanvas.SetActive(true);
        }
        if (panelToOpen != null)
        {
            panelToOpen.SetActive(true);
        }
        Destroy(gameObject);
    }
}
