using System;
using Newtonsoft.Json;

namespace signum
{
    public class CompensatedNode
    {
        private readonly string _node;
        private readonly DateTime _timestamp;
        private readonly string _executionException;
        private readonly string _compensatorException;

        public CompensatedNode(string node, DateTime timestamp)
        {
            _node = node;
            _timestamp = timestamp;
        }

        public CompensatedNode(string node, DateTime timestamp, Exception executionException, Exception compensatorException)
        {
            _node = node;
            _timestamp = timestamp;
            _executionException = executionException.ToString();
            _compensatorException = compensatorException.ToString();
        }
    }
}