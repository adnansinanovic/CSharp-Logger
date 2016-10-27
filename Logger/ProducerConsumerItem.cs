using System.Threading;

namespace Logger
{
    internal class ProducerConsumerItem<T>
    {
        private readonly AutoResetEvent _waitEvent = new AutoResetEvent(false);

        public ProducerConsumerItem(T item)
        {
            Item = item;
        }

        public T Item { get; private set; }

        public void WaitOne()
        {
            _waitEvent.WaitOne();
        }

        public void Set()
        {
            _waitEvent.Set();
        }
    }
}
