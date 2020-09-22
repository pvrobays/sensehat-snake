using System.Collections.Generic;
using System.Linq;

namespace SenseHat.Snake.Utilities.DataStructures
{
    public class CircularQueue<T>
    {
        private readonly Queue<T> _queue;

        public CircularQueue()
        {
            _queue = new Queue<T>();
        }

        public void Enqueue(T item)
        {
            _queue.Enqueue(item);
            _queue.Dequeue();
        }

        public void EnqueueWithoutDequeue(T item)
        {
            _queue.Enqueue(item);
        }

        public T GetHead() => _queue.LastOrDefault();
        public T GetTail() => _queue.FirstOrDefault();

        public T Get(int index)
        {
            return _queue.Count > index ? (T) _queue.Reverse().ToArray().GetValue(index) : default;
        }
        public T[] ToArray() => _queue.Reverse().ToArray();
    }
}