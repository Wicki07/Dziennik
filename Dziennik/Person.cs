using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dziennik
{
    public class Person
    {
        protected int _id { get; set; }
        protected string _name { get; set; }
        protected string _surname { get; set; }

        public Person()
        {

        }
        public Person(int id, string name)
        {
            _id = id;
            _name = name;
        }
        public Person(int id, string name, string surname)
        {
            _id = id;
            _name = name;
            _surname = surname;
        }

        public string Name
        { 
            get => _name; 
            set => _name = value; 
        }
        public string Surname
        {
            get => _surname;
            set => _surname = value;
        }
        public int Id 
        { 
            get => _id; 
            set => _id = value;
        }
        public override string ToString()
        {
            return this._name + " " +  this._surname;
        }
    }
}
