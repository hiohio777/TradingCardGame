using System;
using UnityEngine;
using UnityEngine.UI;

public class BaseScene : MonoBehaviour
{
    private void Awake()
    {
        var canvas = GetComponent<Canvas>();
        canvas.worldCamera = Camera.main;
        canvas.sortingLayerName = "Default";
    }

    protected void StartNewScen(ScenesEnum scenes) { }
}