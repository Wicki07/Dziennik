using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Data.SqlClient;
using System.Runtime;

namespace Dziennik
{
    public partial class Godziny : Form
    {
		List<Label> labels = new List<Label>();
		List<Uczen> uczniowie = new List<Uczen>();
		List<CheckBox> checkBoxes = new List<CheckBox>();
		List<Nieobecność> nieobecnosci = new List<Nieobecność>();
		string connectionString;
		DateTime time = new DateTime(2020, 1, 1, 8, 0, 0);
		Prowadzacy nProwadzacy;
		public Godziny(List<Prowadzacy> prowadzacy)
        {
            InitializeComponent();
			button1.Click += new System.EventHandler(this.Checklist);
			foreach (Prowadzacy prowadzac in prowadzacy)
            {
				if(prowadzac.IsChecked())
                {
					nProwadzacy = prowadzac;
                }
            }
			ReadDatabaseUczniowie();
			CreateLabels();
		}
		private string SQLRequest()
        {
			string request;

			request = "Select Uczniowie.Id, Imie, Nazwisko,Uczniowie.Poziom, Wiek, ZajeciaID From Uczniowie JOIN Zajecia ON ZajeciaID = Zajecia.Id AND Zajecia.ProwadzacyID = " + nProwadzacy.GetId()
				+ " AND Zajecia.Godzina = '" + time.ToString("T") + "'";
			return request;
        }
		private void ReadDatabaseUczniowie()
		{
			string result;
			connectionString = ConfigurationManager.ConnectionStrings["Dziennik.Properties.Settings.Database1ConnectionString"].ConnectionString;
			using (SqlConnection connection = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\\Database1.mdf;Integrated Security=True"))
			using (SqlDataAdapter adapter = new SqlDataAdapter(SQLRequest(), connection))
			{
				DataTable dataTable = new DataTable();
				adapter.Fill(dataTable);
				result = string.Join(Environment.NewLine, dataTable.Rows.OfType<DataRow>().Select(x => string.Join(";", x.ItemArray)));
				connection.Close();
			}
			using (StringReader reader = new StringReader(result))
			{
				string line = "";
				while ((line = reader.ReadLine()) != null)
				{
					string[] s = line.Split(';');
					uczniowie.Add(new Uczen(int.Parse(s[0]), s[1] + " " + s[2], int.Parse(s[3]), int.Parse(s[4]), int.Parse(s[5])));

				}
			}
		}
		private void CreateLabels()
		{
			int position = 100;
			int counter = 0;
			foreach (Uczen uczen in uczniowie)
			{
				labels.Add(new Label());
			}

			foreach (Label label in labels)
			{
				label.AutoSize = true;
				label.Font = new System.Drawing.Font("Arial", 16.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
				label.Location = new System.Drawing.Point(100, position);
				label.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
				label.Size = new System.Drawing.Size(186, 40);
				label.TabIndex = 5;
				label.Text = uczniowie[counter].ToString();
				this.Controls.Add(label);
				checkBoxes.Add(new CheckBox());
				checkBoxes[counter].AutoSize = true;
				checkBoxes[counter].Location = new System.Drawing.Point(520, position);
				checkBoxes[counter].Size = new System.Drawing.Size(80, 17);
				checkBoxes[counter].TabIndex = 1;
				checkBoxes[counter].UseVisualStyleBackColor = true;
				this.Controls.Add(checkBoxes[counter]);
				counter++;
				position += 50;
			}
		}
		private void Checklist(object sender, EventArgs e)
		{
			int counter = 0;
			foreach (CheckBox checkBox in checkBoxes)
			{
				if (checkBox.Checked)
				{
					nieobecnosci.Add(new Nieobecność(uczniowie[counter].GetID(), DateTime.Now));

				}
				counter++;
			}
			connectionString = ConfigurationManager.ConnectionStrings["Dziennik.Properties.Settings.Database1ConnectionString"].ConnectionString;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				String query = "INSERT INTO Nieobecnosci (UczenId,nData) VALUES (@id,@data)";
				foreach (Nieobecność nieobecnosc in nieobecnosci)
				{
					button1.Text = "zrobionegit";
					using (SqlCommand command = new SqlCommand(query, connection))
					{
						command.Connection.Open();
						command.Parameters.AddWithValue("@id", nieobecnosc.GetID());
						command.Parameters.AddWithValue("@data", nieobecnosc.GetDate());

						Console.WriteLine(nieobecnosc.GetID());
						Console.WriteLine(nieobecnosc.GetDate());
						int result = command.ExecuteNonQuery();
						connection.Close();


					}
				}
			}
			
		}
	}
}
