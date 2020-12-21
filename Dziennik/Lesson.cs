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
		List<Student> people = new List<Student>();
		List<CheckBox> checkBoxes = new List<CheckBox>();
		List<Absence> absences = new List<Absence>();
		Connection connection = new Connection();
		List<Student> s = new List<Student>();
		Instructor _instructor = new Instructor();
		int lessonId;
		int begginingId;

		public Lesson(Instructor instructor)
        {
            InitializeComponent();

			button1.Click += new EventHandler(this.Checklist);
			_instructor = instructor;
			s = connection.ReadDatabaseAbsent();

			foreach (var day in connection.SetComboBoxItems(instructor.Id))
			{
				comboBox1.Items.Add(day);
			}
			foreach(var studnets in s)
            {
				absent.Items.Add(studnets);
            }
		}
		private void CreateLabels()
		{
			int position = 100;
			int counter = 0;
			int counterLabels = 0;
			bool check = true;

			labels.Clear();

			foreach (Student student in people)
			{

				if (lessonId == student.Hour)
				{
					labels.Add(new Label());
                    if (check)
                    {
						begginingId = counter;
						check = false;
					}
                    try
                    {
						if (!(((Student)people[counter + 1]).Hour == student.Hour))
						{
							lessonId = ((Student)people[counter + 1]).Hour;
							check = true;
							goto Test;
						}
					}
					catch(ArgumentOutOfRangeException e)
                    {
						button2.Enabled = false;
						goto Test;
					}
				}
				counter++;

			}

		Test:
			counter = 0;
			label3.Text = comboBox1.SelectedItem.ToString() + " " + connection.GetHour(people[begginingId].Id).ToString(@"h\:mm");
			counterLabels = begginingId;
			foreach (Label label in labels)
			{
				label.AutoSize = true;
				label.Font = new Font("Arial", 16.25F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(238)));
				label.Location = new Point(100, position);
				label.Margin = new Padding(2, 0, 2, 0);
				label.Size = new Size(186, 40);
				label.TabIndex = 5;
				label.Text = people[counterLabels].Name + " " + people[counterLabels].Surname;
				this.Controls.Add(label);
				checkBoxes.Add(new CheckBox());
				checkBoxes[counter].AutoSize = true;
				checkBoxes[counter].Location = new Point(520, position);
				checkBoxes[counter].Size = new Size(80, 17);
				checkBoxes[counter].TabIndex = 1;
				checkBoxes[counter].UseVisualStyleBackColor = true;
				checkBoxes[counter].CheckedChanged += new EventHandler(CheckIfchanged);
				this.Controls.Add(checkBoxes[counter]);

				counter++;
				position += 50;
				counterLabels++;
			}
		}
		private void Checklist(object sender, EventArgs e)
		{
			var counter = begginingId;

			absences.Clear();

			foreach (CheckBox checkBox in checkBoxes)
			{
				if (checkBox.Checked)
				{
					absences.Add(new Absence(people[counter].Id, DateTime.Now));
				}
				counter++;
			}
			foreach (Student itemChecked in absent.CheckedItems)
			{
				connection.RemoveAbsent(itemChecked.Id);
			}
			button1.Enabled = false;
			connection.WriteToDatabase(absences);
			button2.Enabled = true;
		}
		private void CheckIfchanged(object sender, EventArgs e)
        {
			bool check = true;
			button1.Enabled = check;
			button2.Enabled = false;
		}
		
		private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
		{
            //var dataTable = connection.ReadDatabase(Program.Person.Student, _instructor, comboBox1.SelectedItem.ToString());
            //people = connection.MakingList(Program.Person.Student, dataTable);
            people = connection.ReadDatabase(_instructor, comboBox1.SelectedItem.ToString());
            lessonId = ((Student)people[0]).Hour;

			foreach (Label label in labels)
			{
				label.Text = "";
				label.Refresh();
			}
			foreach (CheckBox checkBox in checkBoxes)
			{
				checkBox.Visible = false;
				checkBox.Refresh();
			}

			checkBoxes.Clear();
			labels.Clear();
			label3.Text = comboBox1.SelectedItem.ToString() + " " + connection.GetHour(people[0].Id).ToString(@"h\:mm");
			label3.Visible = true;
			label1.Visible = true;
			label2.Visible = true;
			absent.Visible = true;
			CreateLabels();
		}

        private void button2_Click(object sender, EventArgs e)
        {
            foreach (Label label in labels)
            {
                label.Text = "";
                label.Refresh();
            }
            foreach (CheckBox checkBox in checkBoxes)
            {
                checkBox.Visible = false;
                checkBox.Refresh();
            }
			checkBoxes.Clear();
			CreateLabels();
		}
    }
}
