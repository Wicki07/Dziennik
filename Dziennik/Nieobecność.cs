using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dziennik
{
    class Nieobecność
    {
        private int ID;
        private DateTime date;

        public Nieobecność(int nID, DateTime nDate)
        {
            this.ID = nID;
            this.date = nDate;
        }

        public int GetID()
        {
            return ID;
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
