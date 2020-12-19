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
        private int _hour { get; set; }
        private int numbersAbsences { get; set; }

        public Student(int id, string name, int age, int level, int hour) :base(id, name)
        {
            this.age = age;
            this.level = level;
            this._hour = hour;
        }
        public Student(int id, string name, string surname, int numbersAbsences) : base(id, name, surname)
        {
            this.numbersAbsences = numbersAbsences;
        }
        public override string ToString()
        {
            return Name + " " + Surname + " Liczba nieobecności: " + numbersAbsences;
        }
        public int Hour
        {
            get => _hour;
            set => _hour = value;
        }

    }
}
