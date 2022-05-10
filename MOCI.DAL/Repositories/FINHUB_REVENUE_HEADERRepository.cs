using MOCI.Core.Entities;
using MOCI.DAL.DbContexts;
using MOCI.DAL.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace MOCI.DAL.Repositories
{
    public class FINHUB_REVENUE_HEADERRepository : IFINHUB_REVENUE_HEADERRepository
    {
        private string _connection;
        public FINHUB_REVENUE_HEADERRepository(string connection)
        {
            _connection = connection;
        }


        public List<FINHUB_REVENUE_HEADER> GetAll()
        {

            string query = "select * from  FINHUB_REVENUE_HEADER";

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
            List<FINHUB_REVENUE_HEADER> o = JsonConvert.DeserializeObject<List<FINHUB_REVENUE_HEADER>>(JSONresult);
            return o;
        }

        public bool Update(FINHUB_REVENUE_HEADER item)
        {
            try
            {
                string query = " UPDATE  FINHUB_REVENUE_HEADER SET ImportedRowId='" + item.ImportedRowId + "', UserId ='" + item.UserId + "', UserName ='" + item.UserName + "',ACCOUNT_NUMBER='" + item.ACCOUNT_NUMBER + "', MatchedTime ='" + DateTime.Now.ToString() + "', Ismatched ='1'  WHERE   (INVOICE_NO = '" + item.INVOICE_NO + "') AND (CARD_NUMBER = '" + item.CARD_NUMBER + "')  AND (SERIAL_NUMBER = '" + item.SERIAL_NUMBER + "') ";

                SqlConnection conn = new SqlConnection(_connection);
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                DataTable dataTable = new DataTable();

                cmd.ExecuteNonQuery();
                conn.Close();

                string JSONresult;
                JSONresult = JsonConvert.SerializeObject(dataTable);
                List<FINHUB_REVENUE_HEADER> o = JsonConvert.DeserializeObject<List<FINHUB_REVENUE_HEADER>>(JSONresult);
            }
            catch
            {
                return true;
            }

            return false;
        }

        public bool Insert(FINHUB_REVENUE_HEADER item)
        {
            try
            {
                string query = @" INSERT INTO FINHUB_REVENUE_HEADER
                                     (SOURCE, SERIAL_NUMBER, TRANSACTION_DATE, TRANSACTION_TIME, TRANSACTION_AMOUNT, APPROVED_CODE, INVOICE_NO, RESPONSE_CODE, TERMINAL_ID, CARD_NUMBER, ACCOUNT_NUMBER, LOAD_DATE,IsManual)
                            VALUES   ('" + item.SOURCE + "','" + item.SERIAL_NUMBER + "','" + item.TRANSACTION_DATE + "',CONVERT(TIME(0),GETDATE())  ,'" + item.TRANSACTION_AMOUNT + "','" + item.APPROVED_CODE + "','" + item.INVOICE_NO + "','" + item.RESPONSE_CODE + "','" + item.TERMINAL_ID + "','" + item.CARD_NUMBER + "','" + item.ACCOUNT_NUMBER + "',GETDATE(),1)  ";
                SqlConnection conn = new SqlConnection(_connection);
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();


                cmd.ExecuteNonQuery();
                conn.Close();

                query = @"INSERT INTO FINHUB_REVENUE_DETAIL
                                  (SOURCE, SERIAL_NUMBER, SERVICE_NAME, ENTITY, LEDGER_ACCOUNT, DEPARTMENT, FEES_AMOUNT, LOAD_DATE)
                         VALUES   ('" + item.Details[0].SOURCE + "','" + item.Details[0].SERIAL_NUMBER + "',N'" + item.Details[0].SERVICE_NAME + "',N'" + item.Details[0].ENTITY + "',N'" + item.Details[0].LEDGER_ACCOUNT + "',N'" + item.Details[0].DEPARTMENT + "','" + item.Details[0].FEES_AMOUNT + "',GetDate())";
                cmd = new SqlCommand(query, conn);
                conn.Open();


                cmd.ExecuteNonQuery();
                conn.Close();


            }
            catch
            {
                return true;
            }

            return false;
        }


        public bool InsertWithUser(FINHUB_REVENUE_HEADER item)
        {
            try
            {
                string query = @" INSERT INTO FINHUB_REVENUE_HEADER
                                     (SOURCE, SERIAL_NUMBER, TRANSACTION_DATE, TRANSACTION_TIME, TRANSACTION_AMOUNT, APPROVED_CODE, INVOICE_NO, RESPONSE_CODE, TERMINAL_ID, CARD_NUMBER, ACCOUNT_NUMBER, LOAD_DATE,IsManual,UserId,UserName)
                            VALUES   ('" + item.SOURCE + "','" + item.SERIAL_NUMBER + "','" + item.TRANSACTION_DATE + "',CONVERT(TIME(0),GETDATE())  ,'" + item.TRANSACTION_AMOUNT + "','" + item.APPROVED_CODE + "','" + item.INVOICE_NO + "','" + item.RESPONSE_CODE + "','" + item.TERMINAL_ID + "','" + item.CARD_NUMBER + "','" + item.ACCOUNT_NUMBER + "',GETDATE(),'" + item.IsManual + "','" + item.UserId + "','" + item.UserName + "')";
                SqlConnection conn = new SqlConnection(_connection);
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();


                cmd.ExecuteNonQuery();
                conn.Close();

                query = @"INSERT INTO FINHUB_REVENUE_DETAIL
                                  (SOURCE, SERIAL_NUMBER, SERVICE_NAME, ENTITY, LEDGER_ACCOUNT, DEPARTMENT, FEES_AMOUNT, LOAD_DATE)
                         VALUES   ('" + item.Details[0].SOURCE + "','" + item.Details[0].SERIAL_NUMBER + "',N'" + item.Details[0].SERVICE_NAME + "',N'" + item.Details[0].ENTITY + "',N'" + item.Details[0].LEDGER_ACCOUNT + "',N'" + item.Details[0].DEPARTMENT + "','" + item.Details[0].FEES_AMOUNT + "',GetDate())";
                cmd = new SqlCommand(query, conn);
                conn.Open();


                cmd.ExecuteNonQuery();
                conn.Close();


            }
            catch
            {
                return true;
            }

            return false;
        }

        public List<FINHUB_REVENUE_HEADER> GetAllbyDate(DateTime from, DateTime to)
        {

            string query = "select * from  FINHUB_REVENUE_HEADER where TRANSACTION_DATE>='" + from.AddDays(-60).ToString("yyyy-MM-dd") + "' and TRANSACTION_DATE<='" + to.AddDays(1).ToString("yyyy-MM-dd") + "' ";




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
            List<FINHUB_REVENUE_HEADER> o = JsonConvert.DeserializeObject<List<FINHUB_REVENUE_HEADER>>(JSONresult);
            return o;
        }
        public List<FINHUB_REVENUE_DETAIL> GetAllDetails(DateTime from, DateTime to)
        {

            string query = "select d.* FROM  FINHUB_REVENUE_DETAIL d inner JOIN FINHUB_REVENUE_HEADER h on h.SERIAL_NUMBER=d.SERIAL_NUMBER where TRANSACTION_DATE>='" + from.AddDays(-60).ToString("yyyy-MM-dd") + "' and TRANSACTION_DATE<='" + to.AddDays(1).ToString("yyyy-MM-dd") + "'";

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
            List<FINHUB_REVENUE_DETAIL> o = JsonConvert.DeserializeObject<List<FINHUB_REVENUE_DETAIL>>(JSONresult);
            return o;
        }


        public List<string> GetAllAcounts()
        {
            string query = "SELECT distinct [ACCOUNT_NUMBER] FROM [FINHUB_REVENUE_HEADER] where[ACCOUNT_NUMBER] is not null";
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
            List<string> o = (from row in dataTable.AsEnumerable()
                              select row.Field<string>("ACCOUNT_NUMBER")).ToList<string>();
            return o;
        }

        public List<string> GetAllUnique(Cols colName)
        {
            string query = "SELECT   distinct " + colName.ToString() + "  FROM [FINHUB_REVENUE_DETAIL]";
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

            List<string> o = (from row in dataTable.AsEnumerable()
                              select row.Field<string>(colName.ToString())).ToList<string>();
            return o;
        }




    }

    public enum Cols
    {
        LEDGER_ACCOUNT, DEPARTMENT, SERVICE_NAME
    }
}
