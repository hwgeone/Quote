using RDomain.Entity;
using RRepository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RRepository.Implement
{
    public class OrderRepository:RepositoryBase<Order>,IOrderRepository
    {
        public OrderRepository(IDatabaseFactory databaseFactory):base(databaseFactory)
        {
 
        }
    }
}
