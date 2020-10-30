using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Data;
using Dapper;

namespace Dziennik
{
    interface test
    {

    }
    class Connection
    {
		private DateTime time = new DateTime(2020, 1, 1, 8, 0, 0);
		private string SQLRequest(Program.Person person, Instructor instructor)
		{
			switch (person)
			{
				case Program.Person.Instructor:
					return "SELECT * FROM Nauczyciele";
				case Program.Person.Student:
					{
						return $"Select Uczniowie.Id, Imie, Nazwisko,Uczniowie.Poziom, Wiek, ZajeciaID From Uczniowie " +
								$"JOIN Zajecia ON ZajeciaID = Zajecia.Id AND Zajecia.ProwadzacyID = {instructor.Id} " +
								$"AND Zajecia.Godzina = '{ time.ToString("T")}'";

					}
			}
			return "";
		}
        public DataTable ReadDatabase(Program.Person person, Instructor instructor)
        {
            var people = new List<Person>();
            var con = new Connection();
            var request = con.SQLRequest(person, instructor);
			var dataTable = new DataTable();
			using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString))
            using (var adapter = new SqlDataAdapter(request, connection))
            {
                adapter.Fill(dataTable);
                Console.WriteLine(dataTable.Rows);
				connection.Close();
            }
			return dataTable;
        }
        public void WriteToDatabase(List<Absence> absences)
        {
			using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString))
			{
				String query = "INSERT INTO Nieobecnosci (UczenId,nData) VALUES (@id,@data)";
				foreach (var absence in absences)
				{
					using (SqlCommand command = new SqlCommand(query, connection))
					{
						command.Connection.Open();
						command.Parameters.AddWithValue("@id", absence.Id);
						command.Parameters.AddWithValue("@data", absence.GetDate());
						int result = command.ExecuteNonQuery();
						connection.Close();

					}
				}
			}
		}
		public List<Person> MakingList(Program.Person person, Instructor instructor)
        {
            var people = new List<Person>();
            DataTable dataTable;
            switch (person)
            {
                case Program.Person.Instructor:
                    dataTable = ReadDatabase(Program.Person.Instructor, instructor);
                    foreach (DataRow row in dataTable.Rows)
                    {
                        people.Add(new Instructor((int)row["id"], row["imie"].ToString() + " " + row["nazwisko"].ToString()));
                    }
                    break;
                case Program.Person.Student:
                    dataTable = ReadDatabase(Program.Person.Student, instructor);
                    foreach (DataRow row in dataTable.Rows)
                    {
                        people.Add(new Student((int)row["id"], row["imie"].ToString() + " " + row["nazwisko"].ToString(), (int)row["wiek"], (int)row["poziom"], (int)row["zajeciaid"]));
                    }
                    break;
                default:
                    break;
            }
            return people;
		}
        public DataTable ReadDatabase1(Program.Person person, Instructor instructor)
        {
            var people = new List<Person>();
            var con = new Connection();
            var dataTable = new DataTable();
            var request = "SELECT Id, Imie FROM Nauczyciele";
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString))
            {
                var instructors = connection.Query<Test>(request).ToList();
                Console.WriteLine(connection.Query<Test>(request).Count());
                Console.WriteLine(connection.Query<Test>(request).ToString());
                connection.Close();
            }
            return dataTable;
        }
    }
    class Test
    {
        public int Id;
        public string Imie;
    }
}
