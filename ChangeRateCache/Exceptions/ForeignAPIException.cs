using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChangeRateCache.Exceptions
{
    /// <summary>
    /// Ошибка при вызове сторонеего API с курсом валют
    /// </summary>
    public class ForeignAPIException : Exception
    {
        public ForeignAPIException()
        {
        }

        public ForeignAPIException(string message)
            : base(message)
        {
        }

        public ForeignAPIException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}