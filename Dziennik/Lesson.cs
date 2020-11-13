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
    public partial class Lesson : Form
    {
		List<Label> labels = new List<Label>();
		List<Person> people = new List<Person>();
		List<CheckBox> checkBoxes = new List<CheckBox>();
		List<Absence> absences = new List<Absence>();
		Connection connection = new Connection();

		public Lesson(Instructor instructor)
        {
            InitializeComponent();
			button1.Click += new EventHandler(this.Checklist);
			var dataTable = connection.ReadDatabase(Program.Person.Student, instructor);
			people = connection.MakingList(Program.Person.Student, dataTable);
			CreateLabels();
		}
		private void CreateLabels()
		{
			int position = 100;
			int counter = 0;
			foreach (Student student in people)
			{
				labels.Add(new Label());
			}

			foreach (Label label in labels)
			{
				label.AutoSize = true;
				label.Font = new Font("Arial", 16.25F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(238)));
				label.Location = new Point(100, position);
				label.Margin = new Padding(2, 0, 2, 0);
				label.Size = new Size(186, 40);
				label.TabIndex = 5;
				label.Text = people[counter].ToString();
				this.Controls.Add(label);
				checkBoxes.Add(new CheckBox());
				checkBoxes[counter].AutoSize = true;
				checkBoxes[counter].Location = new Point(520, position);
				checkBoxes[counter].Size = new Size(80, 17);
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
					absences.Add(new Absence(people[counter].Id, DateTime.Now));

				}
				counter++;
			}
			connection.WriteToDatabase(absences);
		}
	}
}
