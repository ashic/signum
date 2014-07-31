using System;
using System.Collections.Generic;
using System.Linq;

namespace signum
{
    public class RoutingGraph
    {
        private readonly string _currentNode;
        public string CurrentNode { get { return _currentNode; } }
        public string CurrentService { get { return _nodes[_currentNode].Service; } }

        private readonly CompletedNodes[] _completedNodes;
        private readonly CompensatedNode[] _compensatedNodes;
        private readonly bool _failed;
        private readonly Dictionary<string, GraphNode> _nodes;

        public RoutingGraph(GraphNode[] nodes, string currentNode, CompletedNodes[] completedNodes,
            CompensatedNode[] compensatedNodes, bool failed)
        {
            _currentNode = currentNode;
            _completedNodes = completedNodes;
            _compensatedNodes = compensatedNodes;
            _failed = failed;
            _nodes = nodes.ToDictionary(x => x.NodeId, x => x);
        }

        public RoutingGraph[] Next()
        {
            var current = _nodes[_currentNode];
            var children = current.Children;

            var visited = new CompletedNodes[_completedNodes.Length + 1];
            Array.Copy(_completedNodes, visited, _completedNodes.Length);
            visited[visited.Length - 1] = new CompletedNodes(_currentNode, DateTime.UtcNow);
            var graphNodes = _nodes.Values.ToArray();

            return children.Select(x => new RoutingGraph(graphNodes, x, visited, _compensatedNodes, _failed)).ToArray();
        }

        public RoutingGraph[] Fail(Exception executionException, Exception compensationException)
        {
            var current = _nodes[_currentNode];
            var parents = current.Parents;

            var compensatedNode = new CompensatedNode(_currentNode, DateTime.UtcNow, executionException,
                compensationException);
            var newCompensated = new CompensatedNode[_compensatedNodes.Length + 1];
            Array.Copy(_compensatedNodes, newCompensated, _compensatedNodes.Length);
            newCompensated[newCompensated.Length - 1] = compensatedNode;

            var graphNodes = _nodes.Values.ToArray();
            return parents.Select(x => new RoutingGraph(graphNodes, x, _completedNodes, newCompensated, true))
                .ToArray();
        }

        public RoutingGraph[] GetGraphsForCompensation()
        {
            var current = _nodes[_currentNode];
            var parents = current.Parents;

            var compensatedNode = new CompensatedNode(_currentNode, DateTime.UtcNow);
            var newCompensated = new CompensatedNode[_compensatedNodes.Length + 1];
            Array.Copy(_compensatedNodes, newCompensated, _compensatedNodes.Length);
            newCompensated[newCompensated.Length - 1] = compensatedNode;

            var graphNodes = _nodes.Values.ToArray();
            return parents.Select(x => new RoutingGraph(graphNodes, x, _completedNodes, newCompensated, true))
                .ToArray();
        }
    }
}