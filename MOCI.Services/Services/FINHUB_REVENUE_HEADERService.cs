using AutoMapper;
using MOCI.Core.DTOs;
using MOCI.Core.Entities;
using MOCI.DAL.Interfaces;
using MOCI.DAL.Repositories;
using MOCI.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MOCI.Services
{
    public class FINHUB_REVENUE_HEADERDService : IFINHUB_REVENUE_HEADERService
    {
        private IFINHUB_REVENUE_HEADERRepository _fINHUB_REVENUE_HEADERRepository;
        private readonly IMapper _mapper;

        public FINHUB_REVENUE_HEADERDService(IMapper mapper)
        {
            _mapper = mapper;

        }
        public string Connection { get; set; }

        public bool Update(FINHUB_REVENUE_HEADER item)
        {
            _fINHUB_REVENUE_HEADERRepository = new FINHUB_REVENUE_HEADERRepository(this.Connection);
            return _fINHUB_REVENUE_HEADERRepository.Update(item);
        }
        public bool Insert(FINHUB_REVENUE_HEADER item)
        {
            _fINHUB_REVENUE_HEADERRepository = new FINHUB_REVENUE_HEADERRepository(this.Connection);
            return _fINHUB_REVENUE_HEADERRepository.Insert(item);
        }

        public bool InsertWithUser(FINHUB_REVENUE_HEADER item)
        {
            _fINHUB_REVENUE_HEADERRepository = new FINHUB_REVENUE_HEADERRepository(this.Connection);
            return _fINHUB_REVENUE_HEADERRepository.InsertWithUser(item);
        }
        public List<FINHUB_REVENUE_HEADER> GetAll()
        {
            _fINHUB_REVENUE_HEADERRepository = new FINHUB_REVENUE_HEADERRepository(this.Connection);
            return _fINHUB_REVENUE_HEADERRepository.GetAll();
        }
        public List<FINHUB_REVENUE_HEADER> GetAllbyDate(DateTime from, DateTime to)
        {
            _fINHUB_REVENUE_HEADERRepository = new FINHUB_REVENUE_HEADERRepository(this.Connection);
            return _fINHUB_REVENUE_HEADERRepository.GetAllbyDate(from, to);
        }
        public List<FINHUB_REVENUE_DETAIL> GetAllDetails(DateTime from, DateTime to)
        {
            _fINHUB_REVENUE_HEADERRepository = new FINHUB_REVENUE_HEADERRepository(this.Connection);
            return _fINHUB_REVENUE_HEADERRepository.GetAllDetails(from, to);
        }

        public List<FINHUB_REVENUE_HEADER> GetFinHub(DateTime from, DateTime to)
        {
            _fINHUB_REVENUE_HEADERRepository = new FINHUB_REVENUE_HEADERRepository(this.Connection);
            return _fINHUB_REVENUE_HEADERRepository.GetFinHub(from, to);
        }

        public List<FINHUB_REVENUE_DETAIL> GetFinHubDetails(DateTime from, DateTime to)
        {
            _fINHUB_REVENUE_HEADERRepository = new FINHUB_REVENUE_HEADERRepository(this.Connection);
            return _fINHUB_REVENUE_HEADERRepository.GetFinHubDetails(from, to);
        }

        public List<string> GetAllAcounts()
        {
            _fINHUB_REVENUE_HEADERRepository = new FINHUB_REVENUE_HEADERRepository(this.Connection);
            return _fINHUB_REVENUE_HEADERRepository.GetAllAcounts();
        }
        public List<string> GetAllUnique(Cols col)
        {
            _fINHUB_REVENUE_HEADERRepository = new FINHUB_REVENUE_HEADERRepository(this.Connection);
            return _fINHUB_REVENUE_HEADERRepository.GetAllUnique(col);
        }

        public List<FINHUB_REVENUE_HEADER> GetFinHubBySearchParams(Search searchParams)
        {
            _fINHUB_REVENUE_HEADERRepository = new FINHUB_REVENUE_HEADERRepository(this.Connection);
            return _fINHUB_REVENUE_HEADERRepository.GetFinHubBySearchParams(searchParams);
        }
       

    }
}
