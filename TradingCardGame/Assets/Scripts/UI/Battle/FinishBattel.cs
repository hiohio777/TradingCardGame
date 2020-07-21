using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FinishBattel : MonoBehaviour, IFinishBattel
{
    private Action<object> continueAct;

    [SerializeField] private Button buttonContinue = null;
    [SerializeField] private Image background = null, imageTextBackground = null;
    [SerializeField] private LocalisationText textMessages = null;
    [SerializeField] private Text textInfoResult = null;
    [SerializeField] private Color colorVictory = new Color(), colorDefeat = new Color();
    [SerializeField] private float waitTime = 0.5f;
    [SerializeField] private float finishTransparency = 0.5f, timeTransparency = 0.5f;

    public FinishBattel Initialize(IBattel battel, Action<object> continueAct, IStatisticsBattele statistics)
    {
        var canvas = GetComponent<Canvas>();
        canvas.worldCamera = Camera.main;
        canvas.sortingLayerName = "MainUI";
        transform.SetAsLastSibling();

        this.continueAct = continueAct;
        buttonContinue.onClick.AddListener(Continue);

        if (battel.Winner == TypePersonEnum.player) DeclareVictory(battel, statistics);
        else DeclareDefeat(battel, statistics);

        background.gameObject.SetActive(false);
        StartCoroutine(DisplayBackground());

        return this;
    }

    private void Continue()
    {
        buttonContinue.interactable = false;
        continueAct.Invoke(this);
    }

    private void DeclareVictory(IBattel battel, IStatisticsBattele statistics)
    {
        imageTextBackground.color = colorVictory;
        textMessages.SetKey("victory");

        statistics.DeclareVictory();
        textInfoResult.text = $"Victory: {statistics.CountVictory}, Defeat: {statistics.CountDefeat} , Series Victories: {statistics.CountSeriesVictories}";
    }

    private void DeclareDefeat(IBattel battel, IStatisticsBattele statistics)
    {
        imageTextBackground.color = colorDefeat;
        textMessages.SetKey("defeat");

        statistics.DeclareDefeat();
        textInfoResult.text = $"Victory: {statistics.CountVictory}, Defeat: {statistics.CountDefeat} , Series Victories: {statistics.CountSeriesVictories}";
    }

    private IEnumerator DisplayBackground()
    {
        yield return new WaitForSeconds(waitTime);
        background.gameObject.SetActive(true);

        float currentTransparency = 0;
        float speed = finishTransparency / timeTransparency;

        while (currentTransparency < finishTransparency)
        {
            currentTransparency += Mathf.Clamp(speed * Time.deltaTime, 0, 1);
            background.color = new Color(0, 0, 0, currentTransparency);

            yield return new WaitForFixedUpdate();
        }
    }
}

