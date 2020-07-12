using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FinishTrainingBattel : MonoBehaviour, IFinishBattel
{
    private Action continueAct;

    [SerializeField] private Button buttonContinue;
    [SerializeField] private Image background, imageTextBackground;
    [SerializeField] private LocalisationText textMessages;
    [SerializeField] private Text textInfoResult;
    [SerializeField] private Color colorVictory, colorDefeat;
    [SerializeField] private float waitTime = 0.5f;
    [SerializeField] private float finishTransparency = 0.5f, timeTransparency = 0.5f;

    public IFinishBattel Initialize(IBattel battel, TypePersonEnum loser, Action continueAct, IStatisticsBattele statistics)
    {
        var canvas = GetComponent<Canvas>();
        canvas.worldCamera = Camera.main;
        canvas.sortingLayerName = "MainUI";
        transform.SetAsLastSibling();

        this.continueAct = continueAct;
        buttonContinue.onClick.AddListener(Continue);

        if (loser == TypePersonEnum.enemy) DeclareVictory(battel, statistics);
        else DeclareDefeat(battel, statistics);

        background.gameObject.SetActive(false);
        StartCoroutine(DisplayBackground());

        return this;
    }

    private void Continue() => continueAct.Invoke();

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

