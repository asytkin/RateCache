using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace ChangeRateCache.Models
{
    /// <summary>
    /// Ответ на запрос курсов от  https://openexchangerates.org
    /// </summary>
    public class RateApiResponse
    {
        [JsonProperty(PropertyName = "rates")]
        public Dictionary<string, decimal> rates { get; set; }
    }
}