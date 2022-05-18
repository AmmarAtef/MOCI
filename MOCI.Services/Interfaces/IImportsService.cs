using MOCI.Core.DTOs;
using MOCI.Core.Entities;
using MOCI.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace MOCI.Services.Interfaces
{
    public interface IImportsService
    {
         
        void Add(ImportedData data);
        List<ImportedData> GetByFileName(string fileName);
        List<CustomeList> GetFilesHistory();
        List<ImportedData> GetbyGuid(string guid);
        List<ImportedData> GetImportedBySearch(Search searchParams);
        List<ImportedData> GetReport(Report report);
        ImportedData GetbyId(long id);

    }
}
