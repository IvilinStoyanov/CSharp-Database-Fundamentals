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

        // problems.InitialSetup(Configuration.ConnectionString);           //  P01.Initial Setup
        // problems.VillainNames(Configuration.ConnectionString);           //  P02.Villain Names
        // problems.MinionNames(Configuration.ConnectionString);            //  P03.Minion Names
        // problems.AddMinion(Configuration.ConnectionString);              //  P04.Add Minion
        // problems.ChangeTownNamesCasing(Configuration.ConnectionString);  //  P05.ChangeTownNamesCasing
        // problems.PrintAllMinionsNames(Configuration.ConnectionString);   //  P07.PrintAllMinionsNames
        // problems.IncreaseMinionAge(Configuration.ConnectionString);      //  P08.IncreaseMinionAge 
        problems.IncreaseAgeStoredProcedure(Configuration.ConnectionString);// P09.IncreaseMinionAge



        }
    }
}
