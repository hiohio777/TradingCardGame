using System.Collections;
using UnityEngine;
using Zenject;

public class ApplicationGame : MonoBehaviour
{
    private ILoaderDataGame gameDataManager;
    private PanelsMenager panelsMenager;

    [Inject]
    public void Inject(ILoaderDataGame gameDataManager)
    {
        this.gameDataManager = gameDataManager;
    }

    public void StartGame()
    {
        StartCoroutine(Wait());
    }

    private void Start()
    {
        gameDataManager.Load(this);
        panelsMenager = GetComponent<PanelsMenager>();

    }

    private void OnApplicationQuit()
    {
        gameDataManager.Save();
        Debug.Log("Quitting application");
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(1);
        panelsMenager.OpenSubPanel(this, PanelNameEnum.MainMenu);
    }

    private void Awake()
    {
        var canvas = GetComponent<Canvas>();
        canvas.worldCamera = Camera.main;
        canvas.sortingLayerName = "Default";
    }
}
