namespace Logger
{
    public class ObjectDumperSettings
    {
        public ObjectDumperSettings()
        {
            WriteCompilerGeneratedTypes = false;
            MaxDepth = 4;
            WriteElementType = false;
        }

        /// <summary>
        /// Write compiler generated elements (e.g. backing fields)
        /// </summary>
        public bool WriteCompilerGeneratedTypes { get; set; }

        /// <summary>
        /// How deep dumper will go into object.
        /// </summary>
        public int MaxDepth { get; set; }

        public bool WriteElementType { get; set; }
    }
}
