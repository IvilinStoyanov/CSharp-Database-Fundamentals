using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Fetching_Resultsets_ADO.NET
{
    public class Problems
    {
        public void InitialSetup(string connectionString)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                // Creating DATABASE
                string databaseSql =
                    "CREATE DATABASE MinionsDB";
                ExecNonQuery(connection, databaseSql);

                connection.ChangeDatabase("MinionsDB");
                // CREATING TABLES FOR MinionsDB
                string tableCountries =
                    "CREATE TABLE Countries (Id INT PRIMARY KEY IDENTITY,Name VARCHAR(50))";

                string tableTowns =
                    "CREATE TABLE Towns(Id INT PRIMARY KEY IDENTITY, Name VARCHAR(50), CountryCode INT FOREIGN KEY REFERENCES Countries(Id))";

                string tableMinions =
                    "CREATE TABLE Minions(Id INT PRIMARY KEY IDENTITY,Name VARCHAR(30), Age INT, TownId INT FOREIGN KEY REFERENCES Towns(Id))";

                string tableEvilnessFactors =
                    "CREATE TABLE EvilnessFactors(Id INT PRIMARY KEY IDENTITY, Name VARCHAR(50))";

                string tableVillains =
                    "CREATE TABLE Villains (Id INT PRIMARY KEY IDENTITY, Name VARCHAR(50), EvilnessFactorId INT FOREIGN KEY REFERENCES EvilnessFactors(Id))";

                string tableMinionsVillains =
                "CREATE TABLE MinionsVillains (MinionId INT FOREIGN KEY REFERENCES Minions(Id),VillainId INT FOREIGN KEY REFERENCES Villains(Id),CONSTRAINT PK_MinionsVillains PRIMARY KEY (MinionId, VillainId))";

                ExecNonQuery(connection, tableCountries);
                ExecNonQuery(connection, tableTowns);
                ExecNonQuery(connection, tableMinions);
                ExecNonQuery(connection, tableEvilnessFactors);
                ExecNonQuery(connection, tableVillains);
                ExecNonQuery(connection, tableMinionsVillains);

                // INSERTIGN INFORMATION INTO THE MinionsDB

                string insertIntoTableCountries =
                    "INSERT INTO Countries ([Name]) VALUES ('Bulgaria'),('England'),('Cyprus'),('Germany'),('Norway')";

                string insertIntoTableTowns =
                    "INSERT INTO Towns ([Name], CountryCode) VALUES ('Plovdiv', 1),('Varna', 1),('Burgas', 1),('Sofia', 1),('London', 2),('Southampton', 2),('Bath', 2),('Liverpool', 2),('Berlin', 3),('Frankfurt', 3),('Oslo', 4)";

                string insertIntoTableMinions =
                    "INSERT INTO Minions (Name,Age, TownId) VALUES('Bob', 42, 3),('Kevin', 1, 1),('Bob ', 32, 6),('Simon', 45, 3),('Cathleen', 11, 2),('Carry ', 50, 10),('Becky', 125, 5),('Mars', 21, 1),('Misho', 5, 10),('Zoe', 125, 5),('Json', 21, 1)";

                string insertIntoTableEvilnessFactors =
                    "INSERT INTO EvilnessFactors (Name) VALUES ('Super good'),('Good'),('Bad'), ('Evil'),('Super evil')";

                string insertIntoTableVillains =
                    "INSERT INTO Villains (Name, EvilnessFactorId) VALUES ('Gru',2),('Victor',1),('Jilly',3),('Miro',4),('Rosen',5),('Dimityr',1),('Dobromir',2)";

                string insertIntoTableMinionsVillains =
                    "INSERT INTO MinionsVillains (MinionId, VillainId) VALUES (4,2),(1,1),(5,7),(3,5),(2,6),(11,5),(8,4),(9,7),(7,1),(1,3),(7,3),(5,3),(4,3),(1,2),(2,1),(2,7)";

                ExecNonQuery(connection, insertIntoTableCountries);
                ExecNonQuery(connection, insertIntoTableTowns);
                ExecNonQuery(connection, insertIntoTableMinions);
                ExecNonQuery(connection, insertIntoTableEvilnessFactors);
                ExecNonQuery(connection, insertIntoTableVillains);
                ExecNonQuery(connection, insertIntoTableMinionsVillains);

                connection.Close();
            }
        }

        public void VillainNames(string connectionString)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string minionsInfo = "SELECT v.Name, COUNT(mv.MinionId) AS MinionsCount FROM Villains AS V JOIN MinionsVillains AS mv ON mv.VillainId = v.Id GROUP BY v.Name HAVING COUNT(mv.MinionId) >= 3 ORDER BY MinionsCount DESC";

                using (SqlCommand command = new SqlCommand(minionsInfo, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                      while(reader.Read())
                        {
                            Console.WriteLine($"{reader["Name"]} - {reader["MinionsCount"]}");
                        }               
                    }
                }

                connection.Close();
            }
        }

        public void MinionNames(string connectionString)
        {
            var villainId = int.Parse(Console.ReadLine());

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string villianName = GetVillianName(villainId, connection);

                string minionsInfo = $"SELECT v.Name, m.Name AS [Minions Names] FROM Villains AS v JOIN MinionsVillains AS mv ON mv.VillainId = v.Id JOIN Minions AS m ON m.Id = mv.MinionId WHERE V.Id = {villainId} ORDER BY m.Name";

                using (SqlCommand command = new SqlCommand(minionsInfo, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        reader.Read();
                       Console.WriteLine($"Villian: {reader["Name"]}");
                        
                        while (reader.Read())
                        {
                            int count = 0;
                            Console.WriteLine($"{++count}. {reader["Minions Names"]}");
                        }
                    }
                }

                connection.Close();
            }
        }

        private string GetVillianName(int villainId, SqlConnection connection)
        {
            string nameSql = "SELECT Name FROM Villians WHERE Id = @id";

            using (SqlCommand command = new SqlCommand(nameSql, connection))
            {
                command.Parameters.AddWithValue("@id", villainId);
                return (string)command.ExecuteScalar();
            }
        }

        private void ExecNonQuery(SqlConnection connection, string sqlCommand)
        {
            using (SqlCommand command = new SqlCommand(sqlCommand, connection))
            {
                command.ExecuteNonQuery();
            }
        }
    }
}

