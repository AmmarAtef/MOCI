using AutoMapper;
using MOCI.Core.DTOs;
using MOCI.Core.Entities;
using MOCI.DAL.Interfaces;
using MOCI.Services.Interfaces;
using MOCI.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace MOCI.Services
{
    public class ImportsService : IImportsService
    {
        private readonly IImportsRepository _importsRepository;
        private readonly IMapper _mapper;

        public ImportsService(IImportsRepository importsRepository, IMapper mapper)
        {
            _mapper = mapper;
            _importsRepository = importsRepository;
        }
        public List<ImportedData> GetByFileName(string fileName)
        {
            return _importsRepository.GetByFileName(fileName);
        }
        public List<CustomeList> GetFilesHistory()
        {
            return _importsRepository.GetFilesHistory();
        }
        public void Add(ImportedData data)
        {

            _importsRepository.Add(data);
        }

        public List<ImportedData> GetbyGuid(string guid)
        {
            return _importsRepository.GetbyGuid(guid);
        }

        public List<ImportedData> GetImportedBySearch(SearchParams searchParams)
        {
            return _importsRepository.GetImportedBySearch(searchParams);
        }

        public List<ImportedData> GetReport(Report report)
        {
            report.TransactionDateFrom = report.TransactionDateFrom.Value.AddDays(-1 * (report.TransactionDateFrom.Value.Day - 1));
            int days = System.DateTime.DaysInMonth(report.TransactionDateTo.Value.Year,
                report.TransactionDateTo.Value.Month);
            report.TransactionDateTo = report.TransactionDateTo.Value.AddDays(days-1);
            return _importsRepository.GetReport(report);
        }

    }
}
