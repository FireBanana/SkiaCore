using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace SkiaCore
{
    internal class UIQueue
    {
        private readonly Queue<Action> _dispatcherQueue = new Queue<Action>();

        internal void CallDispatch()
        {
            while (_dispatcherQueue.Count > 0)
            {
                if (_dispatcherQueue.TryDequeue(out Action res))
                    res.Invoke();
            }
        }

        internal void AddToQueue(Action a)
        {
            _dispatcherQueue.Enqueue(a);
        }
    }
}
