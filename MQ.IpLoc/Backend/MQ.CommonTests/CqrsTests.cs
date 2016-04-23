using System;
using System.Linq;
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

            const string cityEmynysu = "cit_U Emynysu";
            Assert.AreEqual(cityEmynysu, query.Result.City);
        }

        [Test]
        [TestCase("cit_Iwes")]
        public void CityLocationTest(string city)
        {
            var query = new LocationByCityQuery(EntityDataSet
                .Instance)
                .Execute(city);

            var firstCity = query.Result
                .Select(l => l.City)
                .First();

            Assert.AreEqual(city, firstCity);
        }
    }
}
