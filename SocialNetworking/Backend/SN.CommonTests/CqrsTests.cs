using SN.Dal;
using NUnit.Framework;

namespace SN.CommonTests
{
    [TestFixture]
    public class CqrsTests
    {
        [SetUp]
        public void SetUp()
        {
            EntityDataSet
                .Instance
                .Fetch();
        }

        
    }
}
