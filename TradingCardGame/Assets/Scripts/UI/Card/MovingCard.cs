using System;
using System.Collections;
using UnityEngine;

public class MovingCard : MonoBehaviour, IMovingCard
{
    public bool IsMoving { get; private set; }

    private Transform _transform;
    private Action execute;
    private float currentRotation = 0;
    private bool isStartPosition, isStartRotation, isStartScale;

    private Vector3 positionTarget;
    private float rotationTarget, scaleTarget;
    private float time = 0.3f, waitTime = 0, waitAfterTime = 0;

    public void SetRotations(float rotation) => currentRotation = rotation;

    public void Destroy()
    {
        StopAllCoroutines();
        IsMoving = false;
        time = 0.3f; waitTime = 0;
        currentRotation = 0;
        StopAllCoroutines();
    }

    public IMovingCard SetWaitTime(float waitTime = 0, float waitAfterTime = 0)
    {
        (this.waitTime, this.waitAfterTime) = (waitTime, waitAfterTime);
        return this;
    }

    public IMovingCard SetPosition(Vector3 positionTarget)
    {
        this.positionTarget = positionTarget;
        isStartPosition = true;
        return this;
    }

    public IMovingCard SetRotation(float rotationTarget)
    {
        this.rotationTarget = rotationTarget;
        isStartRotation = true;
        return this;
    }

    public IMovingCard SetScale(float scaleTarget)
    {
        this.scaleTarget = scaleTarget;
        isStartScale = true;
        return this;
    }

    public void Run(float time) => Run(time, null);
    public void Run(float time, Action execute)
    {
        (this.execute, this.time) = (execute, time);

        StopAllCoroutines();
        if (isStartPosition) StartPositionTarget();
        if (isStartRotation) StartRotationTarget();
        if (isStartScale) StartScaleTarget();
    }

    private void StartPositionTarget()
    {
        StartCoroutine(MoveTo());
        IsMoving = true;
    }

    private void StartRotationTarget()
    {
        StartCoroutine(Rotate());
        IsMoving = true;
    }

    private void StartScaleTarget()
    {
        StartCoroutine(Resize());
        IsMoving = true;
    }

    private IEnumerator MoveTo()
    {
        yield return new WaitForSeconds(waitTime);
        float speedMove = Vector2.Distance(_transform.position, positionTarget) / time;

        while (_transform.position != positionTarget)
        {
            float stepMove = speedMove * Time.deltaTime;
            _transform.position = Vector3.MoveTowards(_transform.position, positionTarget, stepMove);

            yield return new WaitForFixedUpdate();
        }

        isStartPosition = false;
        EndAnimation();
    }

    private IEnumerator Rotate()
    {
        yield return new WaitForSeconds(waitTime);
        float speedRotation = (new Vector3(0, 0, rotationTarget) - new Vector3(0, 0, currentRotation)).z / time;
        float timer = 0;

        while ((timer += Time.deltaTime) < time)
        {
            float stepRotation = speedRotation * Time.deltaTime;
            _transform.rotation = Quaternion.Euler(new Vector3(0, 0, _transform.rotation.eulerAngles.z + stepRotation));

            yield return new WaitForFixedUpdate();
        }

        _transform.localEulerAngles = new Vector3(0, 0, rotationTarget);
        currentRotation = rotationTarget;

        isStartRotation = false;
        EndAnimation();
    }

    private IEnumerator Resize()
    {
        yield return new WaitForSeconds(waitTime);
        var target = new Vector3(scaleTarget, scaleTarget, scaleTarget);
        float speedMove = Vector2.Distance(_transform.localScale, target) / time;

        while (_transform.localScale != target)
        {
            float stepMove = speedMove * Time.deltaTime;
            _transform.localScale = Vector3.MoveTowards(_transform.localScale, target, stepMove);

            yield return new WaitForFixedUpdate();
        }

        isStartScale = false;
        EndAnimation();
    }

    private void EndAnimation()
    {
        if (isStartPosition == false && isStartRotation == false && isStartScale == false)
        {
            if (waitAfterTime > 0) StartCoroutine(WaitAfter());
            else Stop();
        }  
    }

    private IEnumerator WaitAfter()
    {
        yield return new WaitForSeconds(waitAfterTime);
        waitAfterTime = 0;
        Stop();
    }

    private void Stop()
    {
        IsMoving = false;
        time = 0.3f; waitTime = 0;
        execute?.Invoke();
    }

    private void Awake()
    {
        _transform = GetComponent<Transform>();
    }
}