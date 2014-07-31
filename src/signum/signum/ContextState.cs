using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace signum
{
    public class ContextState
    {
        public JObject Data { get { return _data; } }

        private readonly RoutingGraph _graph;
        private readonly Dictionary<string, ServiceExecutorDescriptor> _services;
        private readonly JObject _data;

        public ContextState(RoutingGraph graph, Dictionary<string, ServiceExecutorDescriptor> services, JObject data)
        {
            _graph = graph;
            _services = services;
            _data = data;
        }

        public string CurrentService { get { return _graph.CurrentService; }}

        public ContextState[] NextStates()
        {
            var nextGraphs = _graph.Next();
            return nextGraphs.Select(x => new ContextState(x, _services, _data)).ToArray();
        }

        public ContextState[] Fail(Exception executionException, Exception compensationException)
        {
            var contextsForCompensating = _graph.Fail(executionException, compensationException);
            return contextsForCompensating.Select(x => new ContextState(x, _services, _data))
                .ToArray();
        }

        public ContextState[] GetNextStatesForCompensation()
        {
            var contextsForCompensating = _graph.GetGraphsForCompensation();
            return contextsForCompensating.Select(x => new ContextState(x, _services, _data))
                .ToArray();
        }
    }
}