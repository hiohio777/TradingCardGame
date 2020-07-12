using System;
using System.Collections;
using UnityEngine;

public class Specificity : MonoBehaviour, ISpecificity
{
    [SerializeField] private float time = 1.5f;
    [SerializeField] private float speed = 1;
    private Transform _transform;
    private Action destroySpecificity;
    private Action actFinish;

    private Vector3 scaleTarget = new Vector3(1.5f, 1.5f, 1.5f);

    public static ISpecificity CreatPrefab(TypeSpecificityEnum type, Transform parent, Action actFinish, Action destroySpecificity) =>
    Instantiate(Resources.Load<Specificity>($"Specificity/{type}")).Initialize(parent, actFinish, destroySpecificity);

    private ISpecificity Initialize(Transform parent, Action actFinish, Action destroySpecificity)
    {
        (this.actFinish, this.destroySpecificity) = (actFinish, destroySpecificity);
        (_transform = transform).SetParent(parent, false);
        StartCoroutine(DisplaySpecificity());
        return this;
    }

    public void Stop()
    {
        destroySpecificity?.Invoke();
        Destroy(gameObject);
        actFinish?.Invoke();
    }

    private IEnumerator DisplaySpecificity()
    {
        float carrentTime = 0;
        while (carrentTime < time)
        {
            carrentTime += Time.deltaTime;
            float step = speed * Time.deltaTime;
            _transform.localScale = Vector3.MoveTowards(_transform.localScale, scaleTarget, step);
            yield return new WaitForFixedUpdate();
        }

        Stop();
    }
}

public interface ISpecificity
{
    void Stop();
}