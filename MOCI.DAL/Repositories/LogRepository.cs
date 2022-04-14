using MOCI.Core.Entities;
using MOCI.DAL.DbContexts;
using MOCI.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MOCI.DAL.Repositories
{
    public class LogRepository : LoggerGenericRepository<NLog, long>, ILogRepository
    {
        public LogRepository(LoggerDBContext context) : base(context)
        {
        }
    }
}
