using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Data;

namespace Dziennik
{
    class Connection
    {
		private DateTime time = new DateTime(2020, 1, 1, 8, 0, 0);
		private string SQLRequest(Program.Person person, Instructor instructor)
		{
			switch (person)
			{
				case Program.Person.Instructor:
					return "SELECT * FROM Nauczyciele";
					break;
				case Program.Person.Student:
					{
						return $"Select Uczniowie.Id, Imie, Nazwisko,Uczniowie.Poziom, Wiek, ZajeciaID From Uczniowie " +
								$"JOIN Zajecia ON ZajeciaID = Zajecia.Id AND Zajecia.ProwadzacyID = {instructor.Id} " +
								$"AND Zajecia.Godzina = '{ time.ToString("T")}'";

					}
					break;
			}
			return "";
		}
		public static List<Person> ReadDatabase(Program.Person person, Instructor instructor)
        {

			var people = new List<Person>();
			var con = new Connection();
			var request = con.SQLRequest(person, instructor);
			string result;
			using (var connection = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;Database=dziennik;Integrated Security=True"))
			using (var adapter = new SqlDataAdapter(request, connection))
			{
				DataTable dataTable = new DataTable();
				adapter.Fill(dataTable);
				result = string.Join(Environment.NewLine, dataTable.Rows.OfType<DataRow>().Select(x => string.Join(";", x.ItemArray)));
				connection.Close();
			}
			Console.WriteLine(request);
            switch (person)
            {
				case Program.Person.Instructor:
					using (var reader = new StringReader(result))
					{
						string line = "";
						while ((line = reader.ReadLine()) != null)
						{
							string[] s = line.Split(';');
							people.Add(new Instructor(int.Parse(s[0]), s[1] + " " + s[2]));
						}
					}
					break;
				case Program.Person.Student:
					using (var reader = new StringReader(result))
					{
						string line = "";
						while ((line = reader.ReadLine()) != null)
						{
							string[] s = line.Split(';');
							people.Add(new Student(int.Parse(s[0]), s[1] + " " + s[2], int.Parse(s[3]), int.Parse(s[4]), int.Parse(s[5])));

						}
					}
					break;
				default:
                    break;
            }
            return people;
        }
		public static void WriteToDatabase(List<Absence> absences)
        {
			using (SqlConnection connection = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;Database=dziennik;Integrated Security=True"))
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
    }
}
