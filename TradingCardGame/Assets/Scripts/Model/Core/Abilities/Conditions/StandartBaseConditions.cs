using UnityEngine;

public abstract class StandartBaseConditions : MonoBehaviour
{
    [SerializeField] private ConditionsEnum conditions = ConditionsEnum.equally;
    [SerializeField] private int count = 0;

    protected bool SetResult(int param)
    {
        bool result = false;
        switch (conditions)
        {
            case ConditionsEnum.more: if (param > count) result = true; break;
            case ConditionsEnum.less: if (param < count) result = true; break;
            case ConditionsEnum.equally: if (param == count) result = true; break;
            case ConditionsEnum.not_equally: if (param != count) result = true; break;
        }

        return result;
    }
}