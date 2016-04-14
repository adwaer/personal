using System.Threading;

namespace MQ.Dal
{
    class ThreadParams<TResult>
    {
        public int RecordCount;
        public byte[] Buffer;
        public TResult[] Result;
        public ManualResetEventSlim Waiter;
        public int StartIndex;
    }
}
