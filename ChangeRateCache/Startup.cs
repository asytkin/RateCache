using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;
using System.Web.Caching;
using System.Web.Http;
using DBLayer;
using Hangfire;
using Hangfire.Storage;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(ChangeRateCache.Startup))]

namespace ChangeRateCache
{
    public class Startup
    {
        public const string CONNECTION_STRING =
           @"data source = (localdb)\MSSQLLocalDB; Initial Catalog = RateDbConnection; Integrated Security = True;";
        public void Configuration(IAppBuilder app)
        {
            Hangfire.GlobalConfiguration.Configuration.UseSqlServerStorage(CONNECTION_STRING);
            app.UseHangfireDashboard();
            app.UseHangfireServer(new BackgroundJobServerOptions { WorkerCount = 1, ServerName = Environment.MachineName });
            BackgroundJob.Enqueue(() => CreateRateCache());
            BackgroundJob.Schedule(() => AddUpdateJob(), TimeSpan.FromSeconds(45));
        }

        /// <summary>
        /// Задача на создание повторяющейся джобы для обновления кэша
        /// </summary>
        public void AddUpdateJob()
        {
            var cronMinutes = String.Format("*/{0} * * * *", Consts.MINUTES_AMOUNT_TO_UPDATE_CACHE);
            RecurringJob.AddOrUpdate(() => UpdateRateCache(), cronMinutes);
            return;
        }

        /// <summary>
        /// Задача на обновление кэша каждые Consts.MINUTES_AMOUNT_TO_UPDATE_CACHE минут
        /// </summary>
        public void UpdateRateCache()
        {
            var cache = RatesCache.GetInstance();
            cache.UpdateCache();
            return;
        }

        /// <summary>
        /// Задача на создание кэша при старте приложения
        /// </summary>
        public void CreateRateCache()
        {
            var cache = RatesCache.GetInstance();
            cache.CreateCache();
            return;
        }
    }
}
