using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;


namespace TelegramKey.Bot
{
    public class Receiver<T> : IReceiver<T> where T : class
    {
        protected ConcurrentQueue<T> Objects { get; }

        public bool HasObjects => !this.Objects.IsEmpty;

        public Receiver()
        {
            this.Objects = new ConcurrentQueue<T>();
        }

        public void Receive(T obj)
        {
            this.Objects.Enqueue(obj);
        }

        public void Receive(params T[] objects)
        {
            foreach (var obj in objects)
                this.Receive(obj);
        }

        public Task<T> PeekAsync(CancellationToken token)
        {
            return Task.Factory.StartNew(() =>
            {
                T result;

                while (this.Objects.IsEmpty)
                {
                    token.ThrowIfCancellationRequested();
                    Thread.Sleep(50);
                }


                while (!this.Objects.TryPeek(out result))
                {
                    token.ThrowIfCancellationRequested();
                }

                return result;
            }, token);
        }

        public Task<T> PeekAsync()
        {
            return Task.Factory.StartNew(this.Peek);
        }

        public Task<T> GetAsync(CancellationToken token)
        {
            return Task.Factory.StartNew(() =>
            {
                T result;

                while (this.Objects.IsEmpty)
                {
                    token.ThrowIfCancellationRequested();
                    Thread.Sleep(50);
                }


                while (!this.Objects.TryDequeue(out result))
                {
                    token.ThrowIfCancellationRequested();
                }

                return result;
            }, token);
        }

        public Task<T> GetAsync()
        {
            return Task.Factory.StartNew(this.Get);
        }

        private T Peek()
        {
            T result;

            while (this.Objects.IsEmpty)
                Thread.Sleep(50);

            while (!this.Objects.TryPeek(out result))
            { }

            return result;
        }

        private T Get()
        {
            T result;

            while (this.Objects.IsEmpty)
                Thread.Sleep(50);

            while (!this.Objects.TryDequeue(out result))
            { }

            return result;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return this.Objects.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}