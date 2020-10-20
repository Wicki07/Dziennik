using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dziennik
{
    class Uczen
    {
        private int ID;
        private string name;
        private int age;
        private int level;
        private int hour;

        public Uczen(int nID, string nName, int nAge, int nLevel, int nHour)
        {
            this.ID = nID;
            this.name = nName;
            this.age = nAge;
            this.level = nLevel;
            this.hour = nHour;
        }

        public override string ToString()
        {
            return this.name;
        }

        public int GetID()
        {
            return ID;
        }

    }
}
