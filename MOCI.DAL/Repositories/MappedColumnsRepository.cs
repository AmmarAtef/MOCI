using Microsoft.EntityFrameworkCore.Metadata;
using MOCI.Core.DTOs;
using MOCI.Core.Entities;
using MOCI.DAL.DbContexts;
using MOCI.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MOCI.DAL.Repositories
{
    public class MappedColumnsRepository : GenericRepository<MappedColumns, long>,
        IMappedColumnsRepository
    {
        private readonly MTRSDBContext _context;
        public MappedColumnsRepository(MTRSDBContext context) : base(context)
        {
            _context = context;
        }

        public async Task<bool> AddColumns(List<MappedColumns> mappedColumns)
        {
            try
            {
                AddRange(mappedColumns);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<List<string>> GetColumnsNames()
        {
            IEnumerable<IProperty> properties = _context.ImportedData.EntityType.GetProperties();
            List<string> columnsNames = new List<string>();
            foreach (IProperty property in properties)
            {
                string name = property.Name;
                if (name != "Id")
                    columnsNames.Add(name);
            }
            return columnsNames;
        }

        


    }
}
