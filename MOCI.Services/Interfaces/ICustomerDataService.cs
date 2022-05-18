using MOCI.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MOCI.DAL.Interfaces
{
    public interface ICustomerDataService
    {
        string Connection { get; set; }
        CustomerData GetBySerialNumber(string serialNumber);
    }
}
