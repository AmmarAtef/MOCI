using AutoMapper;
using MOCI.Core.Entities;
using MOCI.DAL.DbContexts;
using MOCI.DAL.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace MOCI.DAL.Repositories
{
    public class CustomerDataService : ICustomerDataService
    {
        private ICustomerDataRepository _customerDataRepository;
        
        public string Connection { get; set; }

        public CustomerData GetBySerialNumber(string serialNumber)
        {
            _customerDataRepository = new CustomerDataRepository(this.Connection);
            return _customerDataRepository.GetBySerialNumber(serialNumber);
        }

    }
}

