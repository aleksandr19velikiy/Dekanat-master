using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Dekanat.SqlServer
{
    class SqlServer
    {
        public static int GetServerPermission(string connectionString, SqlCredential credential)
        {
            using (var connection = new SqlConnection(connectionString, credential))
            {
                connection.Open();

                var command = new SqlCommand("SELECT IS_SRVROLEMEMBER('sysadmin')", connection);
                var permission = command.ExecuteScalar();

                return (int) permission;
            }
        }

        public static DataSet ExecuteStoredProcedure(string connectionString, SqlCredential credential,
            string storedProcedureName, string srcTable, SqlParameter[] sqlParameters = null)
        {
            using (var connection = new SqlConnection(connectionString, credential))
            {
                var command = new SqlCommand(storedProcedureName, connection) { CommandType = CommandType.StoredProcedure };

                if (sqlParameters != null)
                {
                    command.Parameters.AddRange(sqlParameters);
                }
                var dataSet = new DataSet();
                var dataAdapter = new SqlDataAdapter(command);
                dataAdapter.Fill(dataSet, srcTable);

                return dataSet;
            }
        }
    }
}
