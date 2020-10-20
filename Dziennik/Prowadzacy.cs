using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dziennik
{
    public class Prowadzacy
    {
        private string name;
        private int id;
        private bool check;
        public Prowadzacy (int nId, string nName)
        {
            this.id = nId;
            this.name = nName;
            this.check = false;
        }
        public string ToSave()
        {
            return this.name;
        }
        public int GetId()
        {
            return this.id;
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
