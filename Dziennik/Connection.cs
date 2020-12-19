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
        private string SQLRequest(Program.Person person, Instructor instructor, string day)
        {
            switch (person)
            {
                case Program.Person.Instructor:
                    return "SELECT * FROM Instructors";
                case Program.Person.Student:
                    {
                        return $"Select Students.Id, Name, Surname, Students.Level, Age, LessonsID From Students " +
                                $"JOIN Lessons ON LessonsID = Lessons.Id AND Lessons.InstructorID = {instructor.Id} " +
                                $"AND Lessons.Day = '{ day}' order by Lessons.Hour";

                    }
            }
            return "";
        }
        public DataTable ReadDatabase(Program.Person person, Instructor instructor, string day)
        {
            var con = new Connection();
            var request = con.SQLRequest(person, instructor, day);
            var dataTable = new DataTable();
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString))
            using (var adapter = new SqlDataAdapter(request, connection))
            {
                adapter.Fill(dataTable);
                connection.Close();
            }
            return dataTable;
        }
        public void WriteToDatabase(List<Absence> absences)
        {
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString))
            {
                String query = "INSERT INTO Absences (StudentId,nData) VALUES (@id,@data)";
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
        public List<Person> MakingList(Program.Person person, DataTable dataTable)
        {
            var people = new List<Person>();
            switch (person)
            {
                case Program.Person.Instructor:
                    foreach (DataRow row in dataTable.Rows)
                    {
                        people.Add(new Instructor((int)row["id"], row["name"].ToString(), row["surname"].ToString()));
                    }
                    break;
                case Program.Person.Student:
                    foreach (DataRow row in dataTable.Rows)
                    {
                        people.Add(new Student((int)row["id"], row["name"].ToString() + " " + row["surname"].ToString(), (int)row["age"], (int)row["level"], (int)row["lessonsid"]));
                    }
                    break;
                default:
                    break;
            }
            return people;
        }
        public List<Person> ReadDatabase()
        {
            var people = new List<Person>();
            var request = "SELECT * FROM Instructors";
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString))
            {
                var instructors = connection.Query<Instructor>(request).ToList();

                connection.Close();
                foreach (var instructor in instructors)
                {
                    people.Add(instructor);
                }
            }

            return people;
        }
        public List<Student> ReadDatabase(Instructor instructor)
        {
            var students = new List<Student>();
            var request = $"Select Students.Id, Name, Surname, Students.Level, Age, LessonsID From Students " +
                                $"JOIN Lessons ON LessonsID = Lessons.Id AND Lessons.InstructorID = {instructor.Id} " +
                                $"AND Lessons.Hour = '{ time.ToString("T")}'";
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString))
            {
                students = connection.Query<Student>(request).ToList();
                connection.Close();
            }
            return students;
        }
        public DataTable ReadDatabase1(Program.Person person, Instructor instructor)
        {
            var people = new List<Person>();
            var con = new Connection();
            var dataTable = new DataTable();
            var request = "SELECT * FROM Instructors";
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString))
            {
                var instructors = connection.Query<Instructor>(request).ToList();
                connection.Close();
            }
            return dataTable;
        }
        public List<Student> ReadDatabaseAbsent()
        {
            var students = new List<Student>();
            var request = "Select Students.Id, Name, Surname, COUNT(Name) AS numbersAbsences From Students JOIN Absences " +
                            "ON Absences.StudentId = Students.Id " +
                            "GROUP BY Students.Surname, Students.Name, Students.Id";
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString))
            {
                students = connection.Query<Student>(request).ToList();
                connection.Close();
            }
            return students;
        }
        public void RemoveAbsent(int id)
        {
            string queryString = $"DELETE TOP (1) FROM Absences Where Absences.StudentId = {id}";

            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }
        public TimeSpan GetHour(int id)
        {
            var request = $"SELECT Hour From Lessons where Lessons.Id = (SELECT LessonsID From Students where Students.Id = {id})";
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString))
            {
                return connection.ExecuteScalar<TimeSpan>(request);
            }
        }

        public List<String> SetComboBoxItems(int id)
        {
            
            var request = $"SELECT Day From Lessons where Lessons.InstructorID = {id} Group by Day";
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString))
            {
                 return connection.Query<String>(request).ToList();
            }
        }
    }
}
