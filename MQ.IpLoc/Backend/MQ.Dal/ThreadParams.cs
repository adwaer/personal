using System.Threading;

namespace MQ.Dal
{
    /// <summary>
    /// Multithead command param
    /// </summary>
    /// <typeparam name="TResult">Return value</typeparam>
    class ThreadParams<TResult>
    {
        /// <summary>
        /// Record count to read
        /// </summary>
        public int RecordCount;
        /// <summary>
        /// Buffer to read
        /// </summary>
        public byte[] Buffer;

        /// <summary>
        /// Return value
        /// </summary>
        public TResult[] Result;

        /// <summary>
        /// Sync waiter
        /// </summary>
        public ManualResetEventSlim Waiter;
    }
}
