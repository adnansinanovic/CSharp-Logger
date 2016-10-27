using System.Collections.Generic;

namespace Logger.Example
{
    public class Person
    {

        public Person()
        {
            Animals = new List<Animal>();
            Animals.Add(new Animal() { Nickname = "Dog", Weight = 5 });
            Animals.Add(new Animal() { Nickname = "Cat", Weight = 3 });
            Animals.Add(new Animal() { Nickname = "Cow", Weight = 4.2 });
        
    }
        public List<Animal> Animals { get; set; }

        public List<Animal> Lions { get; set; }

        public Car car;

        public int Age;

        public string Name { get; set; }

        public string LastName
        {
            get
            {
                return _lastName;
            }

            set
            {
                _lastName = value;
            }
        }

        private string _lastName;

        private int PrivateProperty { get; set; }
        protected int ProtectedProperty { get; set; }
        internal int InternalProperty { get; set; }
        protected internal int ProtectedInternalProperty { get; set; }
        public int PublicProperty { get; set; }


        private int PrivateField;
        protected int ProtectedField;
        internal int InternalField;
        protected internal int ProtectedInternalField;
        public int PublicField;

        public int? kiotnja = null;

    }
}
