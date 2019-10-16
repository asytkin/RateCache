using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBLayer
{
    public interface IRateContext : IDbContext
    {
        DbSet<RateFrom> RatesFrom { get; set; }
    }
}
