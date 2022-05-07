using MOCI.Core.Entities;
using MOCI.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace MOCI.DAL.Interfaces
{
    public interface IFINHUB_REVENUE_HEADERRepository
    {
        public List<FINHUB_REVENUE_HEADER> GetAll();
        public bool Update(FINHUB_REVENUE_HEADER item);
        public bool Insert(FINHUB_REVENUE_HEADER item);
        List<FINHUB_REVENUE_HEADER> GetAllbyDate(DateTime from, DateTime to);
        List<FINHUB_REVENUE_DETAIL> GetAllDetails(DateTime from, DateTime to);
        List<string> GetAllAcounts();
        List<string> GetAllUnique(Cols colName);

       
    }
}
