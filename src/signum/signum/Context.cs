using System;
using System.Collections;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace signum
{
    public class Context
    {
        private readonly ContextState _state;
        private readonly Sec _sec;
        public JObject Data { get { return _state.Data; } }

        public Context(ContextState state, Sec sec)
        {
            _state = state;
            _sec = sec;
        }

        public string CurrentService { get { return _state.CurrentService; }}

        public void Execute()
        {
            _sec.Execute(this);
        }

        public void Compensate()
        {
            _sec.Compensate(this);
        }

        public Context[] Next()
        {
            var nextStates = _state.NextStates();
            return nextStates.Select(x => new Context(x, _sec)).ToArray();
        }

        public Context[] Fail(Exception executionException, Exception compensationException)
        {
            var statesForCompensation = _state.Fail(executionException, compensationException);
            return statesForCompensation.Select(x => new Context(x, _sec)).ToArray();
        }

        public Context[] GetNextNodesForCompensation()
        {
            var statesForCompensation = _state.GetNextStatesForCompensation();
            return statesForCompensation.Select(x => new Context(x, _sec)).ToArray();
        }
    }
}