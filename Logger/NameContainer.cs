namespace Logger
{
    public class NameContainer
    {
        public NameContainer(string name, string fullName)
        {
            Name = name;
            FullName = fullName;
        }

        public string Name { get; set; }
        public string FullName { get; set; }

        internal object GetName(bool fullName)
        {
            if (fullName)
                return FullName;

            return Name;
        }
    }
}
