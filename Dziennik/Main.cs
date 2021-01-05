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
				label.Click += new EventHandler(this.InstructorChoice);
				counter++;
				position += 60;
				this.Controls.Add(label);
			}
		}

		public void InstructorChoice(object sender, EventArgs e)
		{
			Label clickedLabel = sender as Label;

			var instructor = (Instructor)people[labels.IndexOf(clickedLabel)];

			this.Hide();
			var lessons = new Lesson(instructor);
			lessons.Closed += (s, args) => this.Close();
			lessons.Show();
			lessons.Text = clickedLabel.Text;

		}

    }
}
