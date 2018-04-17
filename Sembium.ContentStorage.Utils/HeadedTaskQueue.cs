using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Utils
{
    public class HeadedTaskQueue<T>
    {
        private readonly int _maxRunningCount;
        private readonly Action<Task<T>> _headCompleteAction;

        private Queue<Task<T>> _queue = new Queue<Task<T>>();

        public HeadedTaskQueue(int maxRunningCount, Action<Task<T>> headCompleteAction)
        {
            _maxRunningCount = maxRunningCount;
            _headCompleteAction = headCompleteAction;
        }

        public void Add(Task<T> task)
        {
            WaitForEmptySlot();

            _queue.Enqueue(task);

            RemoveFirstCompletedTasks();
        }

        private void WaitForEmptySlot()
        {
            while (GetRunningTaskCount() >= _maxRunningCount)
            {
                Thread.Sleep(20);
                Task.WhenAny(_queue).Wait();
            }
        }

        private int GetRunningTaskCount()
        {
            return _queue.Where(x => !x.IsCompleted).Count();
        }

        public void Wait()
        {
            while (_queue.Any())
            {
                Thread.Sleep(20);
                Task.WhenAny(_queue).Wait();
                RemoveFirstCompletedTasks();
            }
        }

        private void RemoveFirstCompletedTasks()
        {
            while (_queue.Any() && (_queue.Peek().IsCompleted))
            {
                var task = _queue.Peek();
                _queue.Dequeue();
                _headCompleteAction(task);
            }
        }
    }
}
