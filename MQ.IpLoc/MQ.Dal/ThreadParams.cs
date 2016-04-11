using System.Threading;

namespace MQ.Dal
{
    class ThreadParams<TResult>
    {
        public int RecordCount;
        public int Position;
        public byte[] Buffer;
        public TResult[] Result;
        public ManualResetEventSlim Waiter;
    }
}
