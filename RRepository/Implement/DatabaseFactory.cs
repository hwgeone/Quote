using Infrastructure;
using RRepository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RRepository.Implement
{
    //工厂模式 实现控制反转
    public class DatabaseFactory : Disposable, IDatabaseFactory
    {
        private EFDbContext dataContext;
        public EFDbContext Get()
        {
            return dataContext ?? (dataContext = new EFDbContext());
        }
        protected override void DisposeCore()
        {
            if (dataContext != null)
            {
                dataContext.Dispose();
            }
        }
    }
}
