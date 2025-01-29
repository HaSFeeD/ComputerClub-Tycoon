using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
public class SceneLoader : MonoBehaviour
{
    [SerializeField]
    private GameObject _loadScreen;
    private Image _loadBar;
    void Start()
    {
        _loadBar = _loadScreen.GetComponentInChildren<Image>();
        LoadSceneAsync("FirstLevelScene");
    }
    private async Task LoadSceneAsync(string sceneName){
        AsyncOperation sceneLoadAsyncOperation = SceneManager.LoadSceneAsync(sceneName);
        while(!sceneLoadAsyncOperation.isDone){
            _loadBar.fillAmount = sceneLoadAsyncOperation.progress;
            await Task.Yield();
        }
    }
    
}
