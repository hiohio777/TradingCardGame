using System;
using UnityEngine;

public interface IMovingCard
{
    bool IsMoving { get; }
    IMovingCard SetPosition(Vector3 positionTarget);
    IMovingCard SetRotation(float rotationTarget);
    IMovingCard SetScale(float scaleTarget);
    IMovingCard SetTarget(Vector3 target, float targetRotation, float scaleTarget);
    IMovingCard SetWaitTime(float waitTime = 0, float waitAfterTime = 0);

    void Run(float time);
    void Run(float time, Action execute);

    void Stop();

    void SetRotations(float rotation);
    void ResetData();
}