using System.Collections.Generic;
using System.Linq;

namespace signum
{
    public class ContextFactory
    {
        private readonly RoutingGraph _graph;
        private readonly Dictionary<string, ServiceExecutorDescriptor> _serviceExecutorDescriptors;

        public ContextFactory(RoutingGraph graph, 
            Dictionary<string, ServiceExecutorDescriptor> serviceExecutorDescriptors)
        {
            _graph = graph;
            _serviceExecutorDescriptors = serviceExecutorDescriptors;
        }

        public Context Create(ContextParameters ps, Sec sec)
        {
            return new Context(new ContextState(_graph, _serviceExecutorDescriptors, ps.Data), sec);
        }
    }
}