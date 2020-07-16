using System;
using UnityEngine;

public interface ISFXFactory
{
    ISpecificity GetSpecificity(TypeSpecificityEnum type, Transform parent, Action actFinish = null, Action destroySpecificity = null);
}