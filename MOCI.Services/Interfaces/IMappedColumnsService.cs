using Microsoft.EntityFrameworkCore.Metadata;
using MOCI.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MOCI.Services.Interfaces
{
    public interface IMappedColumnsService
    {
        Task<List<string>> GetColumnsNames();

        Task<bool> AddColumns(List<MappedColumns> mappedColumns);

        IEnumerable<MappedColumns> GetAllMappedColumns();
    }
}
