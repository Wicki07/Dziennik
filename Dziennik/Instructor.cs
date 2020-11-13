using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dziennik
{
    public class Instructor : Person
    {

        private bool check;
        protected string surname { get; set; }
        public Instructor()
        {

        }
        public Instructor(int id, string name, string surname ) :base(id, name, surname)
        {
            this.check = false;
        }

        public void Checked()
        {
            this.check = true;
        }
        public bool IsChecked()
        {
            return this.check;
        }
    }
}
