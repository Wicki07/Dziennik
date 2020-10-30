using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dziennik
{
    class Student : Person
    {
        private int age { get; set; }
        private int level { get; set; }
        private int hour { get; set; }

        public Student(int id, string name, int age, int level, int hour) :base(id, name)
        {
            this.age = age;
            this.level = level;
            this.hour = hour;
        }


    }
}
