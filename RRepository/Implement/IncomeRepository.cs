using RDomain.Entity;
using RRepository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RRepository.Implement
{
    public class IncomeRepository : RepositoryBase<Income>, IIncomeRepository
    {
        public IncomeRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        { }
    }
}
