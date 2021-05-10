using System;
using System.Threading;

public class ActionDisposable : IDisposable
{
    private Action mAction;

    public ActionDisposable(Action action)
    {
        mAction = action;
    }

    public void Dispose()
    {
        Interlocked.Exchange(ref mAction, null)?.Invoke();
    }
}
