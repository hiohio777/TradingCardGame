using System;
using UnityEngine;

public interface ISpecificityFactory
{
    ISpecificity GetSpecificity(TypeSpecificityEnum type, Transform parent, Action actFinish = null, Action destroySpecificity = null);
}