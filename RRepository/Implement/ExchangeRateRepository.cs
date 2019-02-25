using RDomain.Entity;
using RRepository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RRepository.Implement
{
    public class ExchangeRateRepository:RepositoryBase<ExchangeRate>,IExchangeRateRepository
    {
        public ExchangeRateRepository(IDatabaseFactory databaseFactory):base(databaseFactory)
        {

        }
    }
}
