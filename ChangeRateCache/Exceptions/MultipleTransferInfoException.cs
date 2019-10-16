using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChangeRateCache.Exceptions
{
    /// <summary>
    /// Множественные значения для перевода из одной валюты в другую
    /// </summary>
    public class MultipleTransferInfoException : Exception
    {
        public MultipleTransferInfoException()
        {
        }

        public MultipleTransferInfoException(string message)
            : base(message)
        {
        }

        public MultipleTransferInfoException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}