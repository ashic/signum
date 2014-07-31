using System;
using System.Collections.Generic;

namespace signum
{
    public class NaiveLinearContextDefinitionParser : ContextDefinitionParser
    {
        public RoutingGraph Parse(string definition, Dictionary<string, string> serviceMappings)
        {
            var parts = definition.Split(new[] {"=>", " ", Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries);
            var nodes = new GraphNode[parts.Length];
            
            for (int i = 0; i < parts.Length; i++)
            {
                var parents = i == 0 ? new string[0] : new[] {parts[i - 1]};
                var children = i == parts.Length - 1 ? new string[0] : new[] {parts[i + 1]};
                nodes[i] = new GraphNode(parts[i], serviceMappings[parts[i]], parents, children);   
            }
            
            return new RoutingGraph(nodes, parts[0], new CompletedNodes[0], new CompensatedNode[0], false);
        }
    }
}