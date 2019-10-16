using ChangeRateCache.Exceptions;
using ChangeRateCache.Models;
using DBLayer.Abstraction;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;
using Assert = NUnit.Framework.Assert;
using DescriptionAttribute = NUnit.Framework.DescriptionAttribute;

namespace ChangeRateCache.Tests
{
    [TestFixture]
    public class CacheTest
    {
        private RatesCache _rateCache;
        private const string from = "USD";
        private const string to1 = "EUR";
        private const string to2 = "AED";

        [SetUp]
        public void Initialize()
        {
            _rateCache = RatesCache.GetInstance();
            _rateCache.Cache.Add(from, new List<RateInfo> { new RateInfo { To = to1, Rate = 1.1m, ExpireAt = DateTime.Now.AddMinutes(2)},
                new RateInfo { To = to2, Rate = 1.8m, ExpireAt = DateTime.Now.AddMinutes(2) } }, new CacheItemPolicy());
        }

        [TestCase(from, to1, "1.1")]
        [TestCase(from, to2, "1.8")]
        [Description("Получение валюты из кэша")]
        public void Get_Rate_From_Cache(string from, string to, decimal expectedRate)
        {
            var result = _rateCache.GetRateFromCache(from, to);
            Assert.AreEqual(1, result.Count);
            var resultRate = result.Single();
            Assert.AreEqual(expectedRate, resultRate.Rate);
        }

        [TestCase(from, null)]
        [Description("Получение валюты из кэша без заданной валюты в которую конвертируем")]
        public void Get_Rate_From_Cache_Null_To(string from, string to)
        {
            var result = _rateCache.GetRateFromCache(from, to);
            Assert.AreEqual(2, result.Count);
        }

        [TestCase(from, "YTE")]
        [Description("Получение несуществующей валюты из кэша")]
        public void Get_Rate_From_Cache_To_Not_Exist(string from, string to)
        {
            Assert.Throws(Is.InstanceOf(typeof(RateNotFoundInCacheException)), () => _rateCache.GetRateFromCache(from, to));
        }

    }
}
