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


        public List<ImportedData> GetImportedBySearch(SearchParams searchParams)
        {
            var query = _context.ImportedData.AsQueryable();


            query = searchParams.TransactionDate == null ? query : query.Where(c =>
                 searchParams.TransactionDate != null &&
                 c.Trxn_DateValue == searchParams.TransactionDate
                   );

            query = searchParams.ApprovedCode == null ? query : 
                query.Where(c => searchParams.ApprovedCode != null &&
                c.Approved_Code == searchParams.ApprovedCode.Trim());

            query = searchParams.CardNumber == null ? query : 
                query.Where(c => c.Card_Number == searchParams.CardNumber.Trim());


            query = searchParams.InvoiceNo == null ? query :
                query.Where(c => c.Invoice_No == searchParams.InvoiceNo.Trim());



            return query.ToList();

        }

    }
}
