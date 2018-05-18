# DotNET-CShart-Database-Helper

This helper will allow you to do database reads and write with a just a SQL string and a single command.

        // **** Sample Usage ***
        //   
        //   sql string = "select * from table where field1 = {1} and field2 = {2}";
        //
        //   DataTable dt = DBs.dbSample.GetDataTableSQL(sql, "value1", "value2");
        //   if (dt == null)
        //   {
        //       Exception ex = DBs.dbSample.GetException();
        //       print(ex.Message);
        //   }
        //----------------------------------
        //
        //   sql string = "select ParentItem from Kits WHERE CompanyID = {1}";
        //   System.Data.SqlClient.SqlCommand sqlCmd  = DBs.dbSample.NewSQLCommandSQL(sql, "parameter1");
        //   DataTable dt;
        //
        //   if (DBs.dbSample.LoadSQLCommand(sqlCmd))
        //       dt = DBs.dbSample.datatbl;
        //   } else {
        //        //Error!
        //        ErrorMsg = "Error Getting Items!";
        //        return False;
        //   }
        //----------------------------------
        //
        //   DataTable dt = DBs.dbSample.GetDataTableSP("sp_GetValues", "@startDate,@endDate", "2017-01-01", "2017-12-31");
        //   if (dt == null)
        //       Exception ex = DBs.dbSample.GetException();
        //       print(ex.Message);
        //   }
