using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DBLayer.Abstraction;

namespace DBLayer
{
    public class RateService : EntityService<RateFrom>, IRateService
    {
        IUnitOfWork _unitOfWork;
        IRateRepository _rateRepository;

        public RateService(IUnitOfWork unitOfWork, IRateRepository personRepository)
            : base(unitOfWork, personRepository)
        {
            _unitOfWork = unitOfWork;
            _rateRepository = personRepository;
        }


        public RateFrom GetById(long Id)
        {
            return _rateRepository.GetById(Id);
        }
    }
}
