using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rosyblueonline.Repository.Context
{
    public enum ResponseResult : short
    {
        ExceptionError = -1,
        Success = 0,
        Fail = 1,
        Unknown
    }

    public interface IDBSQLServer
    {
        //SqlParameterCollection Parameters;
        DataSet ExecuteCommand(string Text, CommandType CmdType);
        int ExecuteNonQuery(string Text, CommandType CmdType);
         
    }

    public class DBSQLServer : IDBSQLServer, IDisposable
    {
        string ConStr = string.Empty;
        SqlConnection objConn = null;
        //public List<SqlParameter> Parameters;
        SqlCommand objCmd = null;
        SqlDataAdapter objDA = null;
        DataSet dsResult = null;
        //private SqlParameterCollection _Parameters = null;
        public SqlParameterCollection Parameters;

        public DBSQLServer()
        {
            ConStr = ConfigurationManager.ConnectionStrings["RosyblueonlineEntities"].ConnectionString;
            if (string.IsNullOrEmpty(ConStr))
            {
                throw new Exception("Connection String Not Found");
            }
            objConn = new SqlConnection();
            objConn.ConnectionString = ConStr;
            objCmd = new SqlCommand();
            Parameters = objCmd.Parameters;
        }

        public DataSet ExecuteCommand(string Text, CommandType CmdType)
        {
            dsResult = new DataSet();
            try
            {
                objCmd.CommandType = CmdType;
                objCmd.CommandText = Text;
                objCmd.Connection = objConn;
                //if (Parameters != null && Parameters.Count > 0)
                //{
                //    objCmd.Parameters.Add(Parameters);
                //}

                objDA = new SqlDataAdapter(objCmd);
                objConn.Open();
                objDA.Fill(dsResult);
                objConn.Close();
                if (Parameters != null)
                {
                    Parameters.Clear();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (objConn.State == ConnectionState.Open)
                {
                    objConn.Close();
                }
            }
            return dsResult;
        }

        public int ExecuteNonQuery(string Text, CommandType CmdType)
        {
            int RowCount = 0;
            try
            {
                objCmd.CommandType = CmdType;
                objCmd.CommandText = Text;
                objCmd.Connection = objConn;
                //if (Parameters != null && Parameters.Count > 0)
                //{
                //    objCmd.Parameters.Add(Parameters);
                //}
                objConn.Open();
                RowCount = objCmd.ExecuteNonQuery();
                objConn.Close();
                if (Parameters != null)
                {
                    Parameters.Clear();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (objConn.State == ConnectionState.Open)
                {
                    objConn.Close();
                }
            }
            return RowCount;
        }

        public void Dispose()
        {
            if (this.objDA != null)
            {
                this.objDA.Dispose();
            }
            if (this.objCmd != null)
            {
                this.objCmd.Dispose();
            }
            if (this.objConn != null)
            {
                this.objConn.Dispose();
            }
        }

    }
}
