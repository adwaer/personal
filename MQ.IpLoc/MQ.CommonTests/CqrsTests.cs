using System;
using System.Reflection;
using MQ.Cqrs.Query;
using MQ.Dal;
using NUnit.Framework;

namespace MQ.CommonTests
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

        [Test]
        [TestCase("123.234.0.7")]
        public void IpLocationTest(string ip)
        {
            var query = new LocationByIpQuery(EntityDataSet
                .Instance)
                .Execute(ip);

            Assert.NotNull(query.Result);
        }
    }
}
