using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dziennik
{
    class Absence
    {
        private int _id;
        private DateTime date;



        public Absence(int _id, DateTime date)
        {
            this._id = _id;
            this.date = date;
        }
        public int Id 
        { 
            get => _id; 
            set => _id = value; 
        }
        public string GetDate()
        {
            return date.ToString("d");
        }
        public DateTime GetDateTime()
        {
            return date;
        }
    }
}
