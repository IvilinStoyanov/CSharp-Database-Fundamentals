using System;
using System.Data.SqlClient;

namespace Databases_Advanced_EntityFramework
{
    class StartUp
    {
        static void Main(string[] args)
        {
            using (SqlConnection connection = new SqlConnection())
            {
                connection.Open();

                string datebaseSql = ""

                connection.Close();
            }
        }
    }
}
