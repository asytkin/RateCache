using ChangeRateCache.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ChangeRateCache.Tests
{
    [TestFixture]
    public class ExternalRateApiTest
    {
        private const int RATES_COUNT = 171;

        [Test]
        [Description("Получение списка всех возможных валют")]
        public void Get_All_Rates()
        {
            var _ratesCache = RatesCache.GetInstance();
            MethodInfo methodInfo = typeof(RatesCache).GetMethod("GetAllRates", BindingFlags.NonPublic | BindingFlags.Instance);
            object[] parameters = { };
            var result = (List<string>)methodInfo.Invoke(_ratesCache, parameters);
            Assert.AreEqual(result.Count, RATES_COUNT);
        }


        [TestCase("USD")]
        [TestCase("EUR")]
        [Description("Получение курса валюты относительно других валют")]
        public void Get_RateInfo(string from)
        {
            var _ratesCache = RatesCache.GetInstance();
            MethodInfo methodInfo = typeof(RatesCache).GetMethod("GetRateInfo", BindingFlags.NonPublic | BindingFlags.Instance);
            object[] parameters = { from };
            var result = (List<RateInfo>)methodInfo.Invoke(_ratesCache, parameters);
            Assert.AreEqual(result.Count, RATES_COUNT);
        }
    }
}
