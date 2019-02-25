using Infrastructure;
using RRepository.Implement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RRepository.Interface
{
    public interface IDatabaseFactory : IDisposable
    {
        EFDbContext Get();
    }
}
