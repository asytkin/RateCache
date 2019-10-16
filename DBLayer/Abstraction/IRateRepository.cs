using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DBLayer.Abstraction;

namespace DBLayer
{
    public interface IRateRepository : IGenericRepository<RateFrom>
    {
        RateFrom GetById(long id);
    }
}


