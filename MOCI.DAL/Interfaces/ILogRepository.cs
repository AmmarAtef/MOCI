using MOCI.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MOCI.DAL.Interfaces
{
    public interface ILogRepository : IGenericRepository<NLog, long>
    {
    }
}
