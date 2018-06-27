using System;
using System.Data.SqlClient;

namespace Fetching_Resultsets_ADO.NET
{
    class StartUp
    {
        static void Main(string[] args)
        {
            SqlConnection connection = new SqlConnection(Configuration.ConnectionString);

            Problems problems = new Problems();

            // problems.InitialSetup(Configuration.ConnectionString); // P01.Initial Setup
            // problems.VillainNames(Configuration.ConnectionString); // P02.Villain Names
            problems.MinionNames(Configuration.ConnectionString);




        }
    }
}
