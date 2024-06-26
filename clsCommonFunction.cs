using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;

namespace KompassHR.Models
{
    public class clsCommonFunction
    {
        SqlCommand cmd = new SqlCommand();     
        SqlDataAdapter ado = new SqlDataAdapter();
        SqlDataReader read;
        SqlConnection con = new SqlConnection(DapperORM.connectionString);
        public DataTable GetDataTable(string Query)
        {
            DataTable dt = new DataTable();
            try
            {
                if (con != null && con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                con.Close();
                con.Open();
                DataSet ds = new DataSet();
                cmd.CommandText = Query;
                cmd.Connection = con;
                cmd.CommandTimeout = 0;
                ado.SelectCommand = cmd;

                cmd.ExecuteNonQuery();
                ado.Fill(ds);
                dt = ds.Tables[0];
                con.Close();
            }
            catch (Exception ex)
            {
               
            }
            finally
            {
                con.Close();
            }
            return dt;

        }



        public bool SaveStringBuilder(StringBuilder sqlQuery ,  out string  ErrorCheck)
        {
            ErrorCheck = "";
            if (con != null && con.State == ConnectionState.Closed)
            {
              
                con.Open();
            }
          
           
            SqlCommand cmd = new SqlCommand();
            SqlTransaction objTransaction = null/* TODO Change to default(_) if this is not a reference type */;
            bool result = false;
            try
            {
                if (sqlQuery.Length == 0)
                {
                    return false;
                }
                else
                {
                 
                    objTransaction = con.BeginTransaction();
                    cmd = new SqlCommand(sqlQuery.ToString(), con, objTransaction);
                    cmd.CommandTimeout = 0;
                    cmd.ExecuteNonQuery();
                    objTransaction.Commit();

                    // Close the connection.
                    con.Close();
                    result = true;
                }
            }
            catch (Exception ex)
            {
                ErrorCheck = ex.Message.ToString();
                if ((objTransaction) != null)
                {
                    objTransaction.Rollback();
                    result = false;
                }
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }
           
            return result;
        }




    }


   
}