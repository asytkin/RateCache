using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Caching;
using System.Web;
using System.Web.Http;
using Autofac;
using ChangeRateCache.Controllers;
using ChangeRateCache.Exceptions;
using ChangeRateCache.Injections;
using ChangeRateCache.Models;
using DBLayer;
using DBLayer.Abstraction;
using Newtonsoft.Json;
using WebGrease.Css.Extensions;

namespace ChangeRateCache
{
    public class RatesCache
    {
        const string APPID = "c02c27fbcb6449279a97e99685feaf10";
        const string urlGetAllPossibleCurrencies = "https://openexchangerates.org/api/currencies.json";
        const string urlGetRates = "https://openexchangerates.org/api/latest.json";

        private Logger _logger;
        private static RatesCache _instance;
        public MemoryCache Cache { get; private set; }

        private RatesCache()
        {
            Cache = new MemoryCache("RateCache");
            _logger = new Logger();
        }

        public static RatesCache GetInstance()
        {
            if (_instance != null) return _instance;

            return _instance ?? (_instance = new RatesCache());
        }

        /// <summary>
        /// Получение курса валют из локальной БД
        /// </summary>
        /// <param name="from">продаваемая валюта</param>
        /// <param name="to">покупаемая валюта</param>
        /// <returns>курс валют</returns>
        public List<RateInfo> GetRateFromCache(string from, string to)
        {
            var actualRatesInCache = Cache.Get(from);

            if ((List<RateInfo>)actualRatesInCache == null)
            {
                throw new RateNotFoundInCacheException("Rate From not found in cache");
            }

            var actualRates = (List<RateInfo>)actualRatesInCache;

            if (to != null)
            {
                var ratesTo = actualRates.Where(r => r.To == to).ToList();

                if (!ratesTo.Any())
                    throw new RateNotFoundInCacheException("Rate To not found in cache");

                if (ratesTo.Count() > 1)
                    throw new MultipleTransferInfoException($"Cache has several course From {from} To {to}");

                var actualRate = ratesTo.Single();

                if (actualRate.ExpireAt < DateTime.Now)
                    throw new UpdateCacheRateException("Cache hasnt actual course");

                return new List<RateInfo> { actualRate };
            }

            if (!actualRates.Any())
                throw new RateNotFoundInCacheException($"Cache hasnt ratesTo for From {from}");

            return actualRates;

        }

        #region Interaction with Hangfire - create, update cache

        /// <summary>
        /// Создание кэша с курсами валют в первый раз
        /// </summary>
        public void CreateCache()
        {
            var allСurrencies = GetAllRates();
            foreach (var currency in allСurrencies)
            {
                var ratesInfo = GetRateInfo(currency);
                Cache.Add(currency, ratesInfo, new CacheItemPolicy());
            }

            return;
        }

        /// <summary>
        /// Обновление кэша с курсами валют
        /// Новый курс помещаем в кэш, старый - в БД
        /// </summary>
        public void UpdateCache()
        {
            if (!Cache.Any())
                return;

            var allСurrencies = GetAllRates();
            var container = GetIoCContainer();

            using (var scope = container.BeginLifetimeScope())
            {
                var _rateService = scope.Resolve<IRateService>();

                foreach (var currency in allСurrencies)
                {
                    var ratesTo = new List<RateTo>();

                    var oldCourse = (List<RateInfo>)Cache.Get(currency);

                    try
                    {
                        if (oldCourse == null)
                            throw new RateNotFoundInCacheException("Rate From not found in cache");
                    }
                    catch (Exception ex)
                    {
                        _logger.Error(ex);
                        throw;
                    }

                    oldCourse.ForEach(r => ratesTo.Add(new RateTo { To = r.To, Rate = r.Rate }));
                    var oldRate = new RateFrom { From = currency, CreatingDate = DateTime.Now, To = ratesTo };
                    _rateService.Create(oldRate);

                    var newCourse = GetRateInfo(currency);
                    Cache.Remove(currency);
                    Cache.Add(currency, newCourse, new CacheItemPolicy());

                }
            }
        }


        private IContainer GetIoCContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<RateService>().As<IRateService>();
            builder.RegisterModule(new RepositoryModule());
            builder.RegisterModule(new ServiceModule());
            builder.RegisterModule(new EFModule());
            var container = builder.Build();
            return container;
        }
        #endregion

        #region Interaction with openexchangerates.org

        /// <summary>
        /// Получение информации по актуальному курсу с openexchangerates.org
        /// </summary>
        /// <param name="from">продаваемая валюта</param>
        /// <returns>курсы валют относительно заданной</returns>
        private List<RateInfo> GetRateInfo(string from)
        {
            using (var webClient = new WebClient())
            {
                webClient.QueryString.Add("app_id", APPID);
                webClient.QueryString.Add("base", from);
                RateApiResponse rateApiResponse;
                try
                {
                    string jsonResponse;
                    try
                    {
                        jsonResponse = webClient.DownloadString(urlGetRates);
                    }
                    catch (WebException ex)
                    {
                        throw new ForeignAPIException();
                    }

                    rateApiResponse = JsonConvert.DeserializeObject<RateApiResponse>(jsonResponse);
                }
                catch (Exception ex)
                {
                    _logger.Error(ex);
                    throw;
                }

                var ratesToInfo = new List<RateInfo>();

                rateApiResponse.rates.ForEach(rate => ratesToInfo.Add(new RateInfo
                {
                    To = rate.Key,
                    Rate = rate.Value,
                    ExpireAt = DateTime.Now.AddMinutes(Consts.MINUTES_AMOUNT_TO_UPDATE_CACHE)
                }));

                return ratesToInfo;
            }
        }


        /// <summary>
        /// Получение всех существующих валют с openexchangerates.org
        /// </summary>
        /// <returns>список валют</returns>
        private List<string> GetAllRates()
        {
            using (var webClient = new WebClient())
            {
                try
                {
                    string jsonResponse;
                    try
                    {
                        jsonResponse = webClient.DownloadString(urlGetAllPossibleCurrencies);
                    }
                    catch (WebException ex)
                    {
                        throw new ForeignAPIException();
                    }
                    var response = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonResponse);
                    return response.Keys.ToList();
                }
                catch (Exception ex)
                {
                    _logger.Error(ex);
                    throw;
                }
            }
        }
        #endregion
    }
}
