using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BuffUIParameters : MonoBehaviour, IBuffUIParameters
{
    [SerializeField] private Text buffText;
    [SerializeField] private Color buffUp, debuff;
    [SerializeField] private Vector3 startScale, targetScale, startPosition;
    [SerializeField] private float speedUp, speedDown, timeWait;
    private Action<BuffUIParameters> buffered;
    private Transform _transform;

    public static BuffUIParameters CreatPrefab(Action<BuffUIParameters> buffered)
    {
        var prefab = Instantiate(Resources.Load<BuffUIParameters>($"Card/BuffUIParameters"));
        (prefab.buffered, prefab._transform) = (buffered, prefab.transform);
        return prefab;
    }

    public void SetBuff(Transform parent, string count, bool ifBuff)
    {
        gameObject.SetActive(true);

        _transform.position = startPosition + parent.position;
        _transform.localScale = startScale;

        if (ifBuff) DisplayBuffUp(count);
        else DisplayBuffDown(count);
        StartCoroutine(ShowAnimationUp());
    }

    private void DisplayBuffUp(string count)
    {
        buffText.text = $"+{count}";
        buffText.color = buffUp;
    }
    private void DisplayBuffDown(string count)
    {

        buffText.text = count;
        buffText.color = debuff;
    }

    private void Destroy()
    {
        StopAllCoroutines();
        _transform.SetParent(null, false);
        gameObject.SetActive(false);
        buffered?.Invoke(this); // Поместить\вернуть в буфер для переиспользования
    }

    private IEnumerator ShowAnimationUp()
    {
        while (_transform.localScale != targetScale)
        {
            _transform.localScale = Vector3.MoveTowards(_transform.localScale, targetScale, speedUp);
            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(timeWait);
        StartCoroutine(ShowAnimationDown());
    }

    private IEnumerator ShowAnimationDown()
    {
        while (_transform.localScale != startScale)
        {
            _transform.localScale = Vector3.MoveTowards(_transform.localScale, startScale, speedDown);
            yield return new WaitForFixedUpdate();
        }

        Destroy();
    }
}
