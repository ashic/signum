using System.Collections.Generic;

namespace signum
{
    public class ContextFactoryParameters
    {
        public string Name { get; set; }
        public string Definition { get; set; }
        public Dictionary<string, string> ServiceMappings { get; private set; }
        public string[] ServicesInToken { get; set; }

        public ContextFactoryParameters()
        {
            ServiceMappings = new Dictionary<string, string>();
            ServicesInToken = new string[0];
        }
    }
}