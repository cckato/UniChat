using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SpicyPixel.Threading;
using SpicyPixel.Threading.Tasks;

namespace Fictbox
{
    public class ObservableModel<T> : ConcurrentBehaviour
    {

        public event Action<T> Updated;

        private T data;

        protected void NotifyObservers(T data)
        {
            this.data = data;
            taskFactory.StartNew(Notify);
        }

        private void Notify()
        {
            Updated(data);
        }

    }
}

