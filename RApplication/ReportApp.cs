using RDomain.Entity;
using RDomain.Flat;
using RRepository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RApplication
{
    public class ReportApp
    {
        private IProjectRepository _projectRepository;
        private IOrderRepository _orderRepository;
        private IIncomeRepository _incomeRepository;

        public ReportApp(IProjectRepository projectRepository,IOrderRepository orderRepository,IIncomeRepository incomeRepository)
        {
            _projectRepository = projectRepository;
            _orderRepository = orderRepository;
            _incomeRepository = incomeRepository;
        }

        public IEnumerable<Project> GetProjects()
        {
            var query = _projectRepository.GetAll();
            return query;
        }

        public IEnumerable<Order> GetOrders(Guid projectId)
        {
            var query = _orderRepository.GetBy(o => o.ProjectId == projectId);
            return query;
        }

        public IEnumerable<Income> GetIncomes(Guid projectId)
        {
            var query = _incomeRepository.GetBy(i=>i.ProjectId == projectId);
            return query;
        }

        public IEnumerable<Income> GetIncomes(Guid[] projectIds)
        {
            var query = _incomeRepository.GetBy(i => projectIds.Contains(i.ProjectId));
            return query;
        }

        public IEnumerable<Order> GetOrders(Guid[] projectIds)
        {
            var query = _orderRepository.GetBy(o => projectIds.Contains(o.ProjectId));
            return query;
        }
    }
}
