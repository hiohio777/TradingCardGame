using UnityEngine;
using Zenject;

public class ApplicationGame : MonoBehaviour
{
    private ILoaderDataGame gameDataManager;

    [Inject]
    public void Inject(ILoaderDataGame gameDataManager)
    {
        this.gameDataManager = gameDataManager;
    }

    private void Start()
    {
        gameDataManager.Load();
    }

    private void OnApplicationQuit()
    {
        Debug.Log("Quitting application");
        gameDataManager.Save();
    }
}