using System;
using UnityEngine;

public interface IMovingCard
{
    bool IsMoving { get; }
    IMovingCard SetPosition(Vector3 positionTarget);
    IMovingCard SetRotation(float rotationTarget);
    IMovingCard SetScale(float scaleTarget);
    IMovingCard SetWaitTime(float waitTime = 0, float waitAfterTime = 0);

    void Run(float time);
    void Run(float time, Action execute);
}