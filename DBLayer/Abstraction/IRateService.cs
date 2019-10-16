using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBLayer.Abstraction
{
    public interface IRateService : IEntityService<RateFrom>
    {
        RateFrom GetById(long Id);
    }
}
