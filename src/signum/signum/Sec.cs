using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace signum
{
    public class Sec
    {
        readonly Dictionary<string, ContextFactory> _contextFactories;
        private readonly Dictionary<string, ServiceExecutor> _executors;

        internal Sec(Dictionary<string, ContextFactory> contextFactories, Dictionary<string, ServiceExecutor> executors)
        {
            _contextFactories = contextFactories;
            _executors = executors;
        }

        public static SecBuilder Initialise()
        {
            var parser = new NaiveLinearContextDefinitionParser();
            return new SecBuilder(parser);
        }

        public Context CreateContext(Action<ContextParameters> paramsAdjuster)
        {
            var ps = new ContextParameters();
            paramsAdjuster(ps);

            var factory = _contextFactories[ps.Name];
            return factory.Create(ps, this);
        }

        public void Execute(Context context)
        {
            var serviceExecutor = _executors[context.CurrentService];
            serviceExecutor.Execute(context);
        }

        public void Compensate(Context context)
        {
            var serviceExecutor = _executors[context.CurrentService];
            serviceExecutor.Compensate(context);
        }
    }
}
