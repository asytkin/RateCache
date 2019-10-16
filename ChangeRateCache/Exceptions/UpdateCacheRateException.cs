using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChangeRateCache.Exceptions
{
    /// <summary>
    /// Ошибка при обновлении кэша с курсом валют
    /// </summary>
    public class UpdateCacheRateException : Exception
    {
        public UpdateCacheRateException()
        {
        }

        public UpdateCacheRateException(string message)
            : base(message)
        {
        }

        public UpdateCacheRateException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}