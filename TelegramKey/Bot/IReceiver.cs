using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;


namespace TelegramKey.Bot
{
    public interface IReceiver<T> : IEnumerable<T> where T : class
    {
        bool HasObjects { get; }

        void Receive(T obj);

        void Receive(params T[] objects);
        
        Task<T> PeekAsync(CancellationToken token);

        Task<T> PeekAsync();

        Task<T> GetAsync(CancellationToken token);

        Task<T> GetAsync();
    }
}
