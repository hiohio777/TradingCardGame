using System;
using System.Collections;
using UnityEngine;

public class AttackBattelCell : MonoBehaviour, ICellBattel
{
    [SerializeField] private int id;
    private Transform _transform;
    private SpriteRenderer sprite;
    private bool isMouse;
    private Action<ICellBattel> click;

    private void OnMouseEnter() => isMouse = true;
    private void OnMouseExit() => isMouse = false;

    private IEnumerator OnMouseButtonUp()
    {
        while (true)
        {
            if (Input.GetMouseButtonUp(0))
                if (isMouse) click?.Invoke(this);
            yield return null;
        }
    }

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        _transform = transform;
        StartCoroutine(OnMouseButtonUp());
    }

    public int Id => id;
    public Vector3 Position => _transform.position;
    public bool IsExist => Unit == null ? false : true;
    public IAttackCard Unit { get; set; }

    public void SetClickListener(Action<ICellBattel> click) => this.click = click;
    public void ClearClickListener() => click = null;
}

public interface ICellBattel
{
    int Id { get; }
    Vector3 Position { get; }
    bool IsExist { get; }
    IAttackCard Unit { get; set; }
    void SetClickListener(Action<ICellBattel> click);
    void ClearClickListener();
}