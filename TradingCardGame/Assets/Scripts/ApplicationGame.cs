using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplicationGame : MonoBehaviour
{
    private void Start()
    {
        GameScenes.Init.StartNewScene();
        Destroy(gameObject);
    }
}
