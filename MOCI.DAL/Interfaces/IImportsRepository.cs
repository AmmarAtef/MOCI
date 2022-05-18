﻿using MOCI.Core.DTOs;
using MOCI.Web.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MOCI.DAL.Interfaces
{
    public interface IImportsRepository : IGenericRepository<ImportedData, long>
    {
        public List<ImportedData> GetByFileName(string fileName);
        public List<CustomeList> GetFilesHistory();
        public List<ImportedData> GetbyGuid(string guid);
        public List<ImportedData> GetImportedBySearch(Search searchParams);
        List<ImportedData> GetReport(Report report);
        
    }

}
