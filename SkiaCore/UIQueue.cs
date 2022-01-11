using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace SkiaCore
{
    internal static class UIQueue
    {
        private static readonly Queue<Action> _dispatcherQueue = new Queue<Action>();

        internal static void CallDispatch()
        {
            while (_dispatcherQueue.Count > 0)
            {
                if (_dispatcherQueue.TryDequeue(out Action res))
                    res.Invoke();
            }
        }

        internal static void AddToQueue(Action a)
        {
            _dispatcherQueue.Enqueue(a);
        }
    }
}
