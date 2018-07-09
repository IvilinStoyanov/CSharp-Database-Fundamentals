using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Fetching_Resultsets_ADO.NET
{
    public class Problems
    {
        // P01
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

        // P02
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
                        while (reader.Read())
                        {
                            Console.WriteLine($"{reader["Name"]} - {reader["MinionsCount"]}");
                        }
                    }
                }

                connection.Close();
            }
        }

        // P03
        public void MinionNames(string connectionString)
        {
            var villainId = int.Parse(Console.ReadLine());

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string villianName = GetVillainNames(villainId, connection);

                if (villianName == null)
                {
                    Console.WriteLine($"No villain with ID {villainId} exists in the database.");
                }
                else
                {
                    Console.WriteLine($"Villian: {villianName}");
                    PrintNames(villainId, connection);

                }

                //string minionsInfo = $"SELECT v.Name, m.Name AS [Minions Names] FROM Villains AS v JOIN MinionsVillains AS mv ON mv.VillainId = v.Id JOIN Minions AS m ON m.Id = mv.MinionId WHERE V.Id = {villainId} ORDER BY m.Name";

                //using (SqlCommand command = new SqlCommand(minionsInfo, connection))
                //{
                //    using (SqlDataReader reader = command.ExecuteReader())
                //    {
                //        reader.Read();
                //       Console.WriteLine($"Villian: {reader["Name"]}");

                //        while (reader.Read())
                //        {
                //            int count = 0;
                //            Console.WriteLine($"{++count}. {reader["Minions Names"]}");
                //        }
                //    }
                //}

                connection.Close();
            }
        }

        // P04
        public void AddMinion(string connectionString)
        {
            string[] minionsInfo = Console.ReadLine().Split();
            string[] villainInfo = Console.ReadLine().Split();

            string minionName = minionsInfo[1];
            int age = int.Parse(minionsInfo[2]);
            string townName = minionsInfo[3];

            string villainName = villainInfo[1];

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                int townId = GetTownId(townName, connection);
                int villainId = GetvillainId(villainName, connection);
                int minionId = InsertMinionAndGetId(minionName, age, townId, connection);

                AssingMinionToVillain(villainId, minionId, connection);
                Console.WriteLine($"Successfully added {minionName} to be minion of {villainName}.");
                connection.Close();
            }
        }

        // P05
        public void ChangeTownNamesCasing(string conneciotnString)
        {
            string countryName = Console.ReadLine();

            using (SqlConnection connection = new SqlConnection(conneciotnString))
            {
                connection.Open();

                int countryId = GetCountryID(countryName, connection);

                if (countryId == 0)
                {
                    Console.WriteLine("No town names were affected.");
                }
                else
                {
                    int affectedRows = UpdateNames(countryId, connection);
                    List<string> names = GetNames(countryId, connection);
                    Console.WriteLine($"{affectedRows} town names were affected");
                    Console.WriteLine($"[{string.Join(", ", names)}]");
                }

                connection.Close();
            }
        }

        //P07
        public void PrintAllMinionsNames(string connectionString)
        {
            List<string> minionsInitial = new List<string>();
            List<string> minionsArranged = new List<string>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string allMinionNamesSql = "SELECT Name FROM Minions";

                using (SqlCommand command = new SqlCommand(allMinionNamesSql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            minionsInitial.Add((string)reader["Name"]);
                        }
                    }

                    while (minionsInitial.Count > 0)
                    {
                        minionsArranged.Add(minionsInitial[0]);
                        minionsInitial.RemoveAt(0);

                        if (minionsInitial.Count > 0)
                        {
                            minionsArranged.Add(minionsInitial[minionsInitial.Count - 1]);
                            minionsInitial.RemoveAt(minionsInitial.Count - 1);
                        }
                    }

                    minionsArranged.ForEach(m => Console.WriteLine(m));
                }
                connection.Close();
            }
        }

        //P08 
        public void IncreaseMinionAge(string connectionString)
        {
            int[] selectedIds = Console.ReadLine().Split().Select(int.Parse).ToArray();

            SqlConnection dbCon = new SqlConnection(connectionString);
            dbCon.Open();

            List<int> minionIds = new List<int>();
            List<string> minionaNames = new List<string>();
            List<int> minionAges = new List<int>();

            using (dbCon)
            {
                SqlCommand command = new SqlCommand($"SELECT * FROM Minions WHERE Id IN ({String.Join(", ", selectedIds)})", dbCon);
                SqlDataReader reader = command.ExecuteReader();

                using (reader)
                {
                    if (!reader.HasRows)
                    {
                        reader.Close();
                        dbCon.Close();
                        return;
                    }

                    while (reader.Read())
                    {
                        minionIds.Add((int)reader["Id"]);
                        minionaNames.Add((string)reader["Name"]);
                        minionAges.Add((int)reader["Age"]);
                    }
                }

                for (int i = 0; i < minionIds.Count; i++)
                {
                    int id = minionIds[i];
                    string name = String.Join(" ", minionaNames[i].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList().Select(n => n = char.ToUpper(n.First()) + n.Substring(1).ToLower()).ToArray());
                    int age = minionAges[i] + 1;

                    command = new SqlCommand("UPDATE Minions SET Name = @name, Age = @age WHERE Id = @Id", dbCon);
                    command.Parameters.AddWithValue("@name", name);
                    command.Parameters.AddWithValue("@age", age);
                    command.Parameters.AddWithValue("@id", id);

                    command.ExecuteNonQuery();
                }

                command = new SqlCommand($"SELECT * FROM Minions", dbCon);
                reader = command.ExecuteReader();

                using (reader)
                {
                    if (!reader.HasRows)
                    {
                        reader.Close();
                        dbCon.Close();
                        return;
                    }

                    while (reader.Read())
                    {
                        Console.WriteLine($"{(int)reader["Id"]} {(string)reader["Name"]} {(int)reader["Age"]}");
                    }
                }
            }
        }

        //P09 
        public void IncreaseAgeStoredProcedure(string connectionString)
        {
            SqlConnection dbCon = new SqlConnection(connectionString);
            dbCon.Open();

            int id = int.Parse(Console.ReadLine());

            using (dbCon)
            {
                var command = new SqlCommand("EXEC usp_GetOlder @Id", dbCon);
                command.Parameters.AddWithValue("@Id", id);

                command.ExecuteNonQuery();

                command = new SqlCommand("SELECT * FROM Minions WHERE Id = @Id", dbCon);
                command.Parameters.AddWithValue("@Id", id);

                var reader = command.ExecuteReader();

                using (reader)
                {
                    reader.Read();

                    Console.WriteLine($"{(string)reader["Name"]} - {(int)reader["Age"]} years old");
                }
            }
        }

        private List<string> GetNames(int countryId, SqlConnection connection)
        {
            List<string> names = new List<string>();

            string namesSql = "SELECT Name FROM Towns WHERE CountryCode = @countryId";

            using (SqlCommand command = new SqlCommand(namesSql, connection))
            {
                command.Parameters.AddWithValue("@countryId", countryId);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        names.Add((string)reader[0]);
                    }
                }
            }
            return names;
        }

        private static int UpdateNames(int countryId, SqlConnection connection)
        {
            string updateNames = "UPDATE Towns SET Name = UPPER(Name) WHERE CountryCode = @CountryId";

            using (SqlCommand command = new SqlCommand(updateNames, connection))
            {
                command.Parameters.AddWithValue("@CountryId", countryId);
                return command.ExecuteNonQuery();
            }
        }

        private int GetCountryID(string countryName, SqlConnection connection)
        {
            string countryInfo = "SELECT TOP(1) c.Id FROM Towns AS t JOIN Countries AS c ON c.Id = t.CountryCode WHERE c.Name = @name";

            using (SqlCommand command = new SqlCommand(countryInfo, connection))
            {
                command.Parameters.AddWithValue("@name", countryName);
                if (command.ExecuteScalar() == null)
                {
                    return 0;
                }
                return (int)command.ExecuteScalar();
            }
        }

        private void AssingMinionToVillain(int villainId, int minionId, SqlConnection connection)
        {
            string insertMinionToVillain = "INSERT INTO MinionsVillains(MinionId, VillainId) VALUES (@minionId, @villainId)";

            using (SqlCommand command = new SqlCommand(insertMinionToVillain, connection))
            {
                command.Parameters.AddWithValue("@minionId", minionId);
                command.Parameters.AddWithValue("@villainId", villainId);
                command.ExecuteNonQuery();
            }
        }

        private int InsertMinionAndGetId(string minionName, int age, int townId, SqlConnection connection)
        {
            string insertMinion = "INSERT INTO Minions (Name, Age, TownId) VALUES (@name, @age, @townId)";

            using (SqlCommand command = new SqlCommand(insertMinion, connection))
            {
                command.Parameters.AddWithValue("@name", minionName);
                command.Parameters.AddWithValue("@age", age);
                command.Parameters.AddWithValue("@townId", townId);
                command.ExecuteNonQuery();
            }

            string minionsSql = "SELECT Id FROM Minions WHERE Name = @name";

            using (SqlCommand command = new SqlCommand(minionsSql, connection))
            {
                command.Parameters.AddWithValue("@name", minionName);
                return (int)command.ExecuteScalar();
            }
        }

        private int GetvillainId(string villainName, SqlConnection connection)
        {
            string townSql = "SELECT Id FROM Villains WHERE Name = @Name";

            using (SqlCommand command = new SqlCommand(townSql, connection))
            {
                command.Parameters.AddWithValue("@Name", villainName);

                if (command.ExecuteScalar() == null)
                {
                    InsertIntoVillian(villainName, connection);
                    Console.WriteLine($"Villain {villainName} was added to the database");
                }
                return (int)command.ExecuteScalar();
            }
        }

        private void InsertIntoVillian(string villainName, SqlConnection connection)
        {
            string insertTowns = "INSERT INTO Villains(Name) VALUES (@villianName)";

            using (SqlCommand command = new SqlCommand(insertTowns, connection))
            {
                command.Parameters.AddWithValue("@villianName", villainName);

                command.ExecuteNonQuery();
            }
        }

        private int GetTownId(string townName, SqlConnection connection)
        {
            string townSql = "SELECT Id FROM Towns WHERE Name = @Name";

            using (SqlCommand command = new SqlCommand(townSql, connection))
            {
                command.Parameters.AddWithValue("@Name", townName);

                if (command.ExecuteScalar() == null)
                {
                    InsertIntoTowns(townName, connection);
                    Console.WriteLine($"Town {townName} was added to the database");
                }
                return (int)command.ExecuteScalar();
            }
        }

        private void InsertIntoTowns(string townName, SqlConnection connection)
        {
            string insertTowns = "INSERT INTO Towns(Name) VALUES (@townName)";

            using (SqlCommand command = new SqlCommand(insertTowns, connection))
            {
                command.Parameters.AddWithValue("@townName", townName);

                command.ExecuteNonQuery();
            }
        }

        private string GetVillainNames(int villainId, SqlConnection connection)
        {
            string nameSql = "SELECT Name FROM Villains WHERE Id = @id";

            using (SqlCommand command = new SqlCommand(nameSql, connection))
            {
                command.Parameters.AddWithValue("@id", villainId);
                return (string)command.ExecuteScalar();
            }
        }

        private void PrintNames(int villainId, SqlConnection connection)
        {
            string minionsNames = "SELECT Name, Age FROM Minions AS m JOIN MinionsVillains AS mv ON mv.MinionId = m.Id WHERE mv.VillainId = @Id";

            using (SqlCommand command = new SqlCommand(minionsNames, connection))
            {
                command.Parameters.AddWithValue("Id", villainId);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (!reader.HasRows)
                    {
                        Console.WriteLine("(no minions)");
                    }
                    else
                    {
                        int counter = 0;
                        while (reader.Read())
                        {
                            Console.WriteLine($"{++counter}. {reader[0]} {reader[1]} ");
                        }
                    }
                }
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

