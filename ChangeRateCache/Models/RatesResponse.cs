using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ChangeRateCache.Controllers;

namespace ChangeRateCache.Models
{
    /// <summary>
    ///  Ответ на запрос курсов
    /// </summary>
    public class RatesResponse
    {
        /// <summary> Продаваемая валюта </summary>
        public string From { get; set; }

        /// <summary> Данные по запрошенным курсам </summary>
        public RateInfo[] Rates { get; set; }
    }
}