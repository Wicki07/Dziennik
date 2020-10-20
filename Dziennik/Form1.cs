using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Text;
using System.Configuration;
using System.Data.SqlClient;

namespace Dziennik
{
    public partial class Form1 : Form
    {
		List<Label> labels = new List<Label>();
		List<Prowadzacy> prowadzacy = new List<Prowadzacy>();
		string connectionString;

		public Form1()
        {
            InitializeComponent();
			ReadDatabaseProwadzacy();
			CreateLabels();

		}

		private void CreateLabels()
        {
			int position = 300;
			int counter = 0;
			foreach (Prowadzacy prowadzacy in prowadzacy)
            {
				labels.Add(new Label());
            }
			foreach(Label label in labels)
            {
				label.AutoSize = true;
				label.Font = new System.Drawing.Font("Arial", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
				label.Location = new System.Drawing.Point(100, position);
				label.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
				label.Size = new System.Drawing.Size(186, 50);
				label.TabIndex = 5;
				label.Text = prowadzacy[counter].ToSave();
				label.Click += new System.EventHandler(this.ProwadzacyChoice);
				counter++;
				position += 60;
				this.Controls.Add(label);
			}
		}
		private void ReadDatabaseProwadzacy()
        {
			string result;
			connectionString = ConfigurationManager.ConnectionStrings["Dziennik.Properties.Settings.Database1ConnectionString"].ConnectionString;
			using (SqlConnection connection = new SqlConnection(connectionString))
			using (SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM Nauczyciele", connection))
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
					prowadzacy.Add(new Prowadzacy(int.Parse(s[0]), s[1] + " " + s[2]));

				}
			}
		}

		private void ProwadzacyChoice(object sender, EventArgs e)
		{
			Label clickedLabel = sender as Label;
			prowadzacy[labels.IndexOf(clickedLabel)].Checked();
			this.Hide();
			var godziny = new Godziny(prowadzacy);
			godziny.Closed += (s, args) => this.Close();
			godziny.Show();
			godziny.Text = clickedLabel.Text;
		}

    }
}
