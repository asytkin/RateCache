using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using ChangeRateCache.Injections;
using DBLayer;
using NLog;

namespace ChangeRateCache
{
    public class Logger
    {
        private readonly ILogger _log;
        
        public Logger()
        {
            _log =  LogManager.GetCurrentClassLogger();
        }

        public void Error(Exception ex)
        {
            _log.Error($"StackTrace: {GetErrorPlace()}, Type:{ex.GetType()}, Message:{ex.Message}");
        }

        /// <summary>
        /// Формирует короткий стек вызовов
        /// </summary>
        /// <returns>Возвращает класс и метод откуда вызван метод Logger.Error</returns>
        private string GetErrorPlace()
        {
            // метод, в котором вызван логгер
            var method = new StackFrame(2).GetMethod();
            var place = $"{method.DeclaringType.Name}.{method.Name}";

            // метод в котором вызван method
            var parentMethod = new StackFrame(3).GetMethod();
            if (parentMethod.DeclaringType != null)
                return string.Format("{0}{1}{2}.{3}", place, Environment.NewLine, parentMethod.DeclaringType.Name,
                    parentMethod.Name);

            return place;
        }

    }
}