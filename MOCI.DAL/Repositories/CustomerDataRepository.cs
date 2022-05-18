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
    public class CustomerDataRepository : ICustomerDataRepository
    {
        private string _connection;
        public CustomerDataRepository(string connection)
        {
            _connection = connection;
        }

        public CustomerData GetBySerialNumber(string serialNumber)
        {
            try
            {
                string query = "SELECT * FROM [FINHUB].[dbo].[FINHUB_CUSTOMER_DATA] where [SERIAL_NUMBER] ='" + serialNumber + "'";

                SqlConnection conn = new SqlConnection(_connection);
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                DataTable dataTable = new DataTable();
                // create data adapter
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                // this will query your database and return the result to your datatable
                da.Fill(dataTable);
                conn.Close();
                da.Dispose();

                string JSONresult;
                JSONresult = JsonConvert.SerializeObject(dataTable);
                List<CustomerData> o = JsonConvert.DeserializeObject<List<CustomerData>>(JSONresult);
                return o[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }

    }
}

