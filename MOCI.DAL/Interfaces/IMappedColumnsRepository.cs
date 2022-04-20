using Microsoft.EntityFrameworkCore.Metadata;
using MOCI.Core.Entities;
using MOCI.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MOCI.DAL.Interfaces
{
    public interface IMappedColumnsRepository :IGenericRepository<MappedColumns, long>
    {
        List<string> GetColumnsNames();
    }
}
