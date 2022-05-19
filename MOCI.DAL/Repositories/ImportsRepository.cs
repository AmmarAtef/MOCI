using Microsoft.EntityFrameworkCore;
using MOCI.Core.DTOs;
using MOCI.DAL.DbContexts;
using MOCI.DAL.Interfaces;
using MOCI.Web.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace MOCI.DAL.Repositories
{
    public class ImportsRepository : GenericRepository<ImportedData, long>, IImportsRepository
    {
        public ImportsRepository(MTRSDBContext context) : base(context)
        {
        }
        public List<ImportedData> GetByFileName(string fileName)
        {
            return _context.ImportedData.Where(e => e.FileName == fileName).ToList();
        }
        public List<ImportedData> GetbyGuid(string guid)
        {
            return _context.ImportedData.Where(e => e.Guid == guid).ToList();
        }
        public List<CustomeList> GetFilesHistory()
        {


            var x = _context.ImportedData.Select(e => new { e.FileName, e.Guid, e.Posting_DateValue }).Distinct().ToList();


            return _context.ImportedData.Select(e =>
            new CustomeList
            {

                File = e.FileName,
                Account = e.ACCOUNT_NUMBER,
                Guid = e.Guid,
                Date = e.Posting_DateValue.Value.ToString("dd-MM-yyyy"),
                User = e.User.FirstName + " " + e.User.LastName

            }).Distinct().ToList();


        }


        public List<ImportedData> GetImportedBySearch(DateTime from , DateTime to)
        {
            var query = _context.ImportedData.AsQueryable();


            query = from == null ? query : query.Where(c =>
                 to != null &&
                 c.Trxn_DateValue >= from && c.Trxn_DateValue <=to
                   );

            //query = searchParams.APPROVED_CODE == null ? query :
            //    query.Where(c => searchParams.APPROVED_CODE != null &&
            //    c.Approved_Code == searchParams.APPROVED_CODE.Trim());

            //query = searchParams.CARD_NUMBER == null ? query :
            //    query.Where(c => c.Card_Number == searchParams.CARD_NUMBER.Trim());


            //query = searchParams.INVOICE_NO == null ? query :
            //    query.Where(c => c.Invoice_No == searchParams.INVOICE_NO.Trim());



            return query.ToList();

        }

        public List<ImportedData> GetReport(Report report)
        {
            var query = _context.ImportedData.AsQueryable();


            query = report.TransactionDateFrom == null ? query : query.Where(c =>
                 report.TransactionDateFrom != null &&
                 c.Trxn_DateValue >= report.TransactionDateFrom
                   );

            query = report.TransactionDateTo == null ? query : query.Where(c =>
                  report.TransactionDateTo != null &&
                  c.Trxn_DateValue <= report.TransactionDateTo
                    );


            return query.ToList();

        }


    }
}
