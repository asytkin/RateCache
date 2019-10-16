using DBLayer;
using Effort;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChangeRateCache.Tests
{
    [TestFixture]
    public class RepositoryTest
    {
        DbConnection connection;
        TestContext databaseContext;
        RateRepository _rateRepo;

        [SetUp]
        public void Initialize()
        {
            connection = DbConnectionFactory.CreateTransient();
            connection.ConnectionString = @"data source = (localdb)\MSSQLLocalDB; Initial Catalog = Test;MultipleActiveResultSets=True; Integrated Security = True;";
            databaseContext = new TestContext(connection);
            _rateRepo = new RateRepository(databaseContext);
        }
                
        [TestCase("USD")]
        [TestCase("EUR")]
        [TestCase("ALL")]
        [Description("Получение валюты из репозитория")]
        public void Get_Rate(string rate)
        {
            var rateResult = _rateRepo.GetAll().Where(r => r.From == rate).ToList();
            Assert.IsNotNull(rateResult);
            Assert.AreEqual(1, rateResult.Count);
        }

        [Test]
        [Description("Добавление валюты в репозиторий")]
        public void Add_Rate()
        {
            RateFrom rate = new RateFrom() { Id = 1, From = "AED", CreatingDate = DateTime.Now, To = new List<RateTo> { new RateTo { Id = 1, To = "EUR", Rate = 4.7m } } };
            var result = _rateRepo.Add(rate);
            Assert.DoesNotThrow(() => databaseContext.SaveChanges());
        }
    }
}

