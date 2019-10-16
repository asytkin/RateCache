using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ChangeRateCache.Models
{
    /// <summary>
    /// Запрос на получение курсов валют 
    /// </summary>
    public class RatesRequest
    {
        /// <summary> Отдаваемая валюта </summary>
        [MinLength(3), MaxLength(3)]
        public string From { get; set; }

        /// <summary> Получаемая валюта </summary>
        [MinLength(3), MaxLength(3)]
        public string To { get; set; }
    }
}