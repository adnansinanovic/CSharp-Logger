using System;
using System.Collections.Generic;
using System.Threading;

namespace Logger
{
    internal sealed class ProducerConsumer<T> : IDisposable where T : class
    {
        private readonly Queue<ProducerConsumerItem<T>> _items;
        private readonly object _lock;
        private readonly ManualResetEvent _resetEvent;
        private readonly Thread _thread;
        private bool _running;

        public ProducerConsumer()
        {
            _lock = new object();
            _items = new Queue<ProducerConsumerItem<T>>();

            _resetEvent = new ManualResetEvent(false);
            _thread = new Thread(Worker);
            _thread.Name = $"{AppDomain.CurrentDomain.FriendlyName}.logger";
            _thread.IsBackground = true;
            _thread.Start();
        }

        public void Dispose()
        {
            _running = true;

            _resetEvent.Set();

            _thread.Join();

            _items.Clear();
        }

        public event Action<T> ItemProcessed;
        public event Action<T> ItemAdded;

        private void OnItemAdded(T item)
        {
            ItemAdded?.Invoke(item);
        }

        private void OnItemProcessed(T item)
        {
            ItemProcessed?.Invoke(item);
        }

        private void Worker(object obj)
        {
            while (!_running)
            {
                _resetEvent.WaitOne();

                ProducerConsumerItem<T> consumerProducerItem = null;

                lock (_lock)
                {
                    if (_items.Count > 0)
                        consumerProducerItem = _items.Dequeue();

                    if (_items.Count == 0)
                        _resetEvent.Reset();
                }

                if (consumerProducerItem != null)
                {
                    consumerProducerItem.Set();
                    OnItemProcessed(consumerProducerItem.Item);
                }
            }
        }

        public void Add(T item, bool wait = false)
        {
            ProducerConsumerItem<T> consumerProducerItem = new ProducerConsumerItem<T>(item);
            lock (_lock)
            {
                _items.Enqueue(consumerProducerItem);

                OnItemAdded(item);

                _resetEvent.Set();
            }

            if (wait)
                consumerProducerItem.WaitOne();
        }
    }
}
