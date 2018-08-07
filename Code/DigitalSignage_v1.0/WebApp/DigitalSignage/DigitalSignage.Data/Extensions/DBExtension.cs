
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Reflection;


namespace DigitalSignage.Data
{
    public static class DBExtension
    {
       //private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="db"></param>
        /// <param name="storedProcedureName"></param>
        /// <param name="parameters"></param>
       public static int ExecuteNonQuery(this Database db, string storedProcedureName, params SqlParameter[] parameters)
       {
           var conn = db.Connection;
           var initialState = conn.State;
           DataSet dataSet = new DataSet();
           try
           {
               if (initialState != ConnectionState.Open)
                   conn.Open();

               SqlCommand cmd = new SqlCommand(storedProcedureName, (SqlConnection)conn);
               cmd.CommandType = CommandType.StoredProcedure;
               cmd.CommandTimeout = 0;
               foreach (var parameter in parameters)
               {
                   cmd.Parameters.Add(parameter);
               }
               return cmd.ExecuteNonQuery();
           }
           catch (Exception exception)
           {
               //log.Error(exception.Message + "\n" + exception.StackTrace);
               throw new MyException();
           }
       }


        //function that set the given object from the given data row
        public static T GetEntity<T>(this DataRow row) where T : new()
        {
            T item = new T();
            // go through each column
            foreach (DataColumn c in row.Table.Columns)
            {
                // find the property for the column
                PropertyInfo pInfo = item.GetType().GetProperty(c.ColumnName);
               
                if (pInfo != null && row[c] != DBNull.Value)
                {
                    Type pType = pInfo.PropertyType;
                    var targetType = pType.IsNullableType() ? Nullable.GetUnderlyingType(pType) : pType;
                    var pVal = Convert.ChangeType(row[c], targetType);
                    // if exists, set the value
                    pInfo.SetValue(item, pVal, null);
                }
            }
            return item;
        }

        public static List<T> ToList<T>(this DataTable dt) where T : new()
        {
            List<T> data = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                data.Add(row.GetEntity<T>());
            };
            return data;
        }

        private static bool IsNullableType(this Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition().Equals(typeof(Nullable<>));
        }

   

        class MyException : Exception
        {
            public MyException()
            {
            }
        }
      
    }
}
