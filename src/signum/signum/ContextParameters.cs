using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace signum
{
    public class ContextParameters
    {
        public string Name { get; set; }
        public JObject Data { get; private set; }

        public ContextParameters()
        {
            Data = new JObject();
        }
    }
}