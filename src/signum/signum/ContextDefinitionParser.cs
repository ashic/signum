using System.Collections.Generic;

namespace signum
{
    public interface ContextDefinitionParser
    {
        RoutingGraph Parse(string definition, Dictionary<string, string> serviceMappings);
    }
}
