using Microsoft.EntityFrameworkCore.Metadata;
using MOCI.Core.DTOs;
using MOCI.Core.Entities;
using MOCI.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MOCI.DAL.Interfaces
{
    public interface IMappedColumnsRepository : IGenericRepository<MappedColumns, long>
    {
        Task<List<string>> GetColumnsNames();

        Task<bool> AddColumns(List<MappedColumns> mappedColumns);

       
    }
}
