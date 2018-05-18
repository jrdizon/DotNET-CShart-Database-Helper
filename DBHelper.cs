using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Helpders
{
    //
    // Database Helper
    // Joseph Dizon 2017-07
    //
    public class DBs
    {
        // **** Sample Usage ***
        //   
        //   sql string = "select * from table where field1 = {1} and field2 = {2}"
        //
        //   dt DataTable = DBs.dbSample.GetDataTableSQL(sql, "value1", "value2")
        //   if (dt = null)
        //       ex Exception = DBs.dbSample.GetException()
        //       print(ex.Message)
        //   } if
        //----------------------------------
        //
        //   sql string = "select ParentItem from Kits WHERE CompanyID = {1}"
        //   sqlCmd System.Data.SqlClient.SqlCommand = DBs.dbSample.NewSQLCommandSQL(sql, DOXCookie.CompanyID)
        //   dt DataTable
        //
        //   if (DBs.dbSample.LoadSQLCommand(sqlCmd))
        //       dt = DBs.dbSample.datatbl
        //   } else {
        //        //Error!
        //        fncBackMsg.Value = "Error Getting Kit Items!"
        //        return False
        //   } if
        //----------------------------------
        //
        //   dt DataTable = DBs.dbSample.GetDataTableSP("sp_GetValues", "@startDate,@}Date", "2017-01-01", "2017-12-31")
        //   if (dt = null)
        //       ex Exception = DBs.dbSample.GetException()
        //       print(ex.Message)
        //   } if


        public static DBClass dbSample = new DBClass("");
    }


    public class DBClass
    {
        public string connStr = null;
        public Exception exception = null;
        public DataTable datatbl = null;
        public DataSet dataset = null;

        //public MustOverride void Initialize()
        //public MustOverride Function LoadSQLCommand(oSqlCommand SqlCommand) DataTable
        public DBClass(string Connectionstring)
        {
            connStr = Connectionstring;
        }

        public void SetConnectionString(string ConnectionName)
        {
            connStr = ConfigurationManager.ConnectionStrings[ConnectionName].ConnectionString;
        }

        public Exception GetException()
        {
            return exception;
        }

        //Get datatable Using SQL script
        public SqlCommand NewSQLCommandSQL(string sql, params object[] parm)
        {
            exception = null;
            try
            {
                SqlCommand oSqlCommand = new SqlCommand();

                oSqlCommand.CommandType = CommandType.Text;

                string sqlFinal = sql;

                if (parm.Length > 0)
                {
                    Int32 index;
                    for (index = 1; index <= parm.Length; index++)
                    {
                        string index1 = index.ToString();
                        string bracket = "{" + index1 + "}";
                        string amper = "@parm" + index1;
                        sqlFinal = sqlFinal.Replace(bracket, amper);
                        oSqlCommand.Parameters.AddWithValue(amper, parm[index - 1]);
                    }
                }

                oSqlCommand.CommandText = sqlFinal;

                return oSqlCommand;

            }
            catch (Exception ex)
            {
                exception = ex;
                return null;
            }
        }

        //Get datatable Using SQL script
        public SqlCommand NewSQLCommandSP(string storedProc, string spParams, params object[] parm)
        {
            exception = null;
            try
            {
                SqlCommand oSqlCommand = new SqlCommand(storedProc);

                oSqlCommand.CommandType = CommandType.StoredProcedure;

                string[] spParm = spParams.Split(',');

                if (parm.Length > 0)
                {
                    Int32 index;
                    for (index = 0; index < parm.Length; index++)
                    {
                        oSqlCommand.Parameters.AddWithValue(spParm[index], parm[index]);
                    }
                }

                return oSqlCommand;
            }
            catch (Exception ex)
            {
                exception = ex;
                return null;
            }
        }

        public DataTable GetDataTableSQL(string sql, params object[] parm)
        {
            SqlCommand oSqlCommand = NewSQLCommandSQL(sql, parm);
            if (LoadSQLCommand(oSqlCommand))
            {
                return datatbl;
            }
            else
            {
                return null;
            }
        }

        public DataTable GetDataTableSP(string storedProc, string spParams, params object[] parm)
        {
            SqlCommand oSqlCommand = NewSQLCommandSP(storedProc, spParams, parm);
            if (LoadSQLCommand(oSqlCommand))
            {
                return datatbl;
            }
            else
            {
                return null;
            }
        }

        public Boolean ExecSQLCommand(SqlCommand oSqlCommand)
        {
            exception = null;

            if (oSqlCommand == null)
            {
                return false;
            }

            try
            {
                using (SqlConnection oSQLConnection = new SqlConnection(connStr))
                {
                    oSQLConnection.Open();
                    oSqlCommand.Connection = oSQLConnection;
                    oSqlCommand.ExecuteNonQuery();
                    return true;
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                return false;
            }
        }

        //Protected Function _LoadSQLCommand(oSqlCommand SqlCommand) DataTable
        public Boolean LoadSQLCommand(SqlCommand oSqlCommand)
        {
            exception = null;
            datatbl = null;
            dataset = new DataSet();

            if (oSqlCommand == null)
            {
                return false;
            }

            try
            {
                using (SqlConnection oSQLConnection = new SqlConnection(connStr))
                {
                    oSQLConnection.Open();
                    oSqlCommand.Connection = oSQLConnection;

                    //datatbl = new DataTable()
                    using (SqlDataAdapter da = new SqlDataAdapter(oSqlCommand))
                    {
                        da.Fill(dataset);
                    }
                    if (dataset != null && dataset.Tables.Count > 0)
                    {
                        datatbl = dataset.Tables[0];
                    }

                    return true;
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                datatbl = null;
                return false;
            }
        }
    }
}
