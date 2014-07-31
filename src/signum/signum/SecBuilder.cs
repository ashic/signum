using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace signum
{
    public class SecBuilder
    {
        private readonly ContextDefinitionParser _parser;
        readonly List<Action<Dictionary<string, ServiceExecutor>>> _serviceBuilders = new List<Action<Dictionary<string, ServiceExecutor>>>();
        readonly List<Action<ContextFactoryParameters>> _contextFactoryBuilders = new List<Action<ContextFactoryParameters>>(); 
     
        public SecBuilder(ContextDefinitionParser parser)
        {
            _parser = parser;
        }

        public SecBuilder RegisterServices(Action<Dictionary<string, ServiceExecutor>> builder)
        {
            _serviceBuilders.Add(builder);
            return this;
        }

        public SecBuilder RegisterContextFactory(Action<ContextFactoryParameters> builder)
        {
            _contextFactoryBuilders.Add(builder);
            return this;
        }

        public Sec Build()
        {
            var services = new Dictionary<string, ServiceExecutor>();
            foreach (var serviceBuilder in _serviceBuilders)
                serviceBuilder(services);

            var descriptors = services.ToDictionary(x => x.Key, v => v.Value.Describe());

            var contextFactories = new Dictionary<string, ContextFactory>();

            foreach (var builder in _contextFactoryBuilders)
            {
                var cfps = new ContextFactoryParameters();
                builder(cfps);

                var graph = _parser.Parse(cfps.Definition, cfps.ServiceMappings);
                var descriptorsInToken = descriptors.Where(x => cfps.ServicesInToken.Contains(x.Key))
                    .ToDictionary(x => x.Key, v => v.Value);
                var contextFactory = new ContextFactory(graph, descriptorsInToken);
                contextFactories[cfps.Name] = contextFactory;
            }

            return new Sec(contextFactories, services);
        }
    }
}