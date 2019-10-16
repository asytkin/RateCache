using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Mvc;
using Autofac;
using ChangeRateCache.Exceptions;
using ChangeRateCache.Injections;
using ChangeRateCache.Models;
using DBLayer;
using NLog;


namespace ChangeRateCache.Controllers
{
    [System.Web.Http.RoutePrefix("rates")]
    public class RatesController : ApiController
    {
        private RatesCache _ratesCache { get; set; }
        private Logger _logger;

        public RatesController()
        {
            _ratesCache = RatesCache.GetInstance();
            _logger = new Logger();
        }


        [System.Web.Http.Route("{from}/{to}")]
        [System.Web.Http.Route("{from}")]
        [ResponseType(typeof(RatesResponse))]
        public IHttpActionResult Get([FromUri] RatesRequest request)
        {
            try
            {
                List<RateInfo> rateInfo = _ratesCache.GetRateFromCache(request.From.ToUpper(), request.To?.ToUpper());
                return this.Ok(new RatesResponse { From = request.From.ToUpper(), Rates = rateInfo.ToArray() });
            }
            catch (RateNotFoundInCacheException ex)
            {
                _logger.Error(ex);
                return this.NotFound();
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return this.BadRequest();
            }
        }
    }
}
