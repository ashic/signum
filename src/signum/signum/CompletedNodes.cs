using System;

namespace signum
{
    public class CompletedNodes
    {
        private readonly string _node;
        private readonly DateTime _timestamp;

        public CompletedNodes(string node, DateTime timestamp)
        {
            _node = node;
            _timestamp = timestamp;
        }
    }
}