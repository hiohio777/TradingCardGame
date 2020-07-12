using System;

public abstract class ActionModule
{
    protected Action actNext;
    private int count = 0;

    protected ActionModule(Action actNext, int count) =>
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