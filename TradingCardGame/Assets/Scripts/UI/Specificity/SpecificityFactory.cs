using System;
using UnityEngine;

public class SpecificityFactory : ISpecificityFactory
{
    public ISpecificity GetSpecificity(TypeSpecificityEnum type, Transform parent, 
        Action actFinish = null, Action destroySpecificity = null)
    {
        if (type != TypeSpecificityEnum.Default)
            return Specificity.CreatPrefab(type, parent, actFinish, destroySpecificity);
        return null;
    }
}
