namespace signum
{
    public class GraphNode
    {
        public string NodeId { get; private set; }
        public string[] Children { get { return _children; } }
        public string Service { get { return _service; }}
        public string[] Parents { get { return _parents; } }

        private readonly string _service;
        private readonly string[] _parents;
        private readonly string[] _children;

        public GraphNode(string nodeId, string service, string[] parents, string[] children)
        {
            NodeId = nodeId;
            _service = service;
            _parents = parents;
            _children = children;
        }
    }
}