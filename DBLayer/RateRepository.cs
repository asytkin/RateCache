using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBLayer
{
    public class RateRepository : GenericRepository<RateFrom>, IRateRepository
    {
        public RateRepository(DbContext context)
            : base(context)
        {

        }

        public override IEnumerable<RateFrom> GetAll()
        {
            return _entities.Set<RateFrom>().Include(x => x.To).AsEnumerable();
        }

        public RateFrom GetById(long id)
        {
            return _dbset.Include(x => x.To).FirstOrDefault(x => x.Id == id);
        }
    }
}
