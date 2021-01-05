using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Dziennik
{
    class Creator
    {

		public static List<Label> CreateLabels(List<Person> _people)
        {
			var form1 = new Main();
			var labels = new List<Label>();
			var position = 100;
			var counter = 0;
			foreach (var people in _people)
			{
				labels.Add(new Label());
			}
			foreach (Label label in labels)
			{
				label.AutoSize = true;
				label.Font = new Font("Arial", 26.25F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(238)));
				label.Location = new Point(100, position);
				label.Margin = new Padding(2, 0, 2, 0);
				label.Size = new Size(186, 50);
				label.TabIndex = 5;
				label.Text = _people[counter].ToString();
				label.Click += new EventHandler(form1.InstructorChoice);
				counter++;
				position += 60;
                //Form1.Controls.Add(label);
			}
			return labels;
		}
    }
}
