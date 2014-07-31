using System;

namespace signum
{
    public class LocalServiceExecutor : ServiceExecutor
    {
        private readonly Action<Context> _handler;
        private readonly Action<Context> _compensator;

        public LocalServiceExecutor(Action<Context> handler, Action<Context> compensator)
        {
            _handler = handler;
            _compensator = compensator;
        }

        public ServiceExecutorDescriptor Describe()
        {
            return new LocalServiceExecutorDescriptor();
        }

        public void Execute(Context context)
        {
            try
            {
                _handler(context);
            }
            catch (Exception e)
            {
                Exception exec = e;
                Exception compensate = null;
            
                try
                {
                    _compensator(context);
                }
                catch (Exception e2)
                {
                    compensate = e2;
                    
                }
                finally
                {
                    var nodesToNotify = context.Fail(exec, compensate);
                    foreach (var prev in nodesToNotify)
                    {
                        prev.Compensate();
                    }
                }
                return;
            }

            var childSteps = context.Next();
            foreach (var childStep in childSteps)
                childStep.Execute();

        }

        public void Compensate(Context context)
        {
            try
            {
                _compensator(context);
            }
            finally
            {
                var nodesToNotify = context.GetNextNodesForCompensation();
                foreach (var prev in nodesToNotify)
                {
                    prev.Compensate();
                }

            }
        }
    }

    public class LocalServiceExecutorDescriptor : ServiceExecutorDescriptor
    {
    }

}