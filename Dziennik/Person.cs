using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dziennik
{
    public class Person
    {
        protected int _id;
        protected string _name;

        public Person()
        {

        }
        public Person(int id, string name)
        {
            _id = id;
            _name = name;
        }

        public int Id 
        { 
            get => _id; 
            set => _id = value;
        }
        public override string ToString()
        {
            return this._name;
        }
    }
}
