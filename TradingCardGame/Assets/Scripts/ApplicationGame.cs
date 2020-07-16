using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class ApplicationGame : MonoBehaviour
{
    private ILoaderDataGame gameDataManager;

    [Inject]
    public void Inject(ILoaderDataGame gameDataManager)
    {
        this.gameDataManager = gameDataManager;
    }

    public void StartMainScen()
    {
        SceneManager.LoadScene(ScenesEnum.MainScene.ToString());
    }

    private void Start()
    {
        gameDataManager.Load(this);

    }

    private void OnApplicationQuit()
    {
        Debug.Log("Quitting application");
        gameDataManager.Save();
    }
}