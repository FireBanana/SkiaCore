using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace SkiaCore
{
    internal class UIQueue
    {
        private readonly Queue<Action<Window>> _dispatcherQueue = new Queue<Action<Window>>();

        internal void CallDispatch(Window w)
        {
            while (_dispatcherQueue.Count > 0)
            {
                if (_dispatcherQueue.TryDequeue(out Action<Window> res))
                    res.Invoke(w);
            }
        }

        internal void AddToQueue(Action<Window> a)
        {
            _dispatcherQueue.Enqueue(a);
        }
    }
}
