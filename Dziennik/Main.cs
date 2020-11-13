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
    public partial class Main : Form
    {
		List<Label> labels = new List<Label>();
		List<Person> people = new List<Person>();
		Connection connection = new Connection();
        public Main()
        {
            InitializeComponent();
			var dataTable = connection.ReadDatabase1(Program.Person.Instructor, new Instructor());
			//people = connection.MakingList(Program.Person.Instructor, dataTable);
			people = connection.ReadDatabase();
			CreateLabels();
		}

		private void CreateLabels()
        {
			int position = 300;
			int counter = 0;
			foreach (var people in people)
            {
				labels.Add(new Label());
            }
			foreach(Label label in labels)
            {
				label.AutoSize = true;
				label.Font = new Font("Arial", 26.25F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(238)));
				label.Location = new Point(100, position);
				label.Margin = new Padding(2, 0, 2, 0);
				label.Size = new Size(186, 50);
				label.TabIndex = 5;
				label.Text = people[counter].ToString();
				label.Click += new EventHandler(this.ProwadzacyChoice);
				counter++;
				position += 60;
				this.Controls.Add(label);
			}
		}

		public void ProwadzacyChoice(object sender, EventArgs e)
		{
			Label clickedLabel = sender as Label;
			var instructor = (Instructor)people[labels.IndexOf(clickedLabel)];
			this.Hide();
			var godziny = new Lesson(instructor);
			godziny.Closed += (s, args) => this.Close();
			godziny.Show();
			godziny.Text = clickedLabel.Text;
		}

    }
}
