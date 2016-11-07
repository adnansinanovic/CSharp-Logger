namespace Logger
{
    public class P1
    {
        public string Name = "P1";
        public C1 Child1 = new C1("C1A");
        public C1 Child2 = new C1("C1B");
    }

    public class C1
    {        
        public C1(string name)
        {
            Name = name;
        }

        public string Name = "C1";
        public C2 Child2a = new C2("C2A", 39);
        public C2 Child2b = new C2("C2B", 17);
    }

    public class C2
    {
        
        public C2(string description, int age)
        {
            Description = description;
            Age = age;
        }

        public int Age { get; set; }
        public string Description = "C2";
    }
}
