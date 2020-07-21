using System;

public abstract class ActionModuleEvent
{
    protected Action actNext;
    private int count = 0;

    protected ActionModuleEvent(Action actNext, int count) =>
        (this.actNext, this.count) = (actNext, count);

    protected void Final()
    {
        if (--count == 0)
        {
            actNext?.Invoke();
            actNext = null;
        }
    }
}