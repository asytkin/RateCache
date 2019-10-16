using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChangeRateCache.Exceptions
{
    /// <summary>
    /// Валюта не найдена в кэше валют
    /// </summary>
    public class RateNotFoundInCacheException : Exception
    {
        public RateNotFoundInCacheException()
        {
        }

        public RateNotFoundInCacheException(string message)
            : base(message)
        {
        }

        public RateNotFoundInCacheException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}