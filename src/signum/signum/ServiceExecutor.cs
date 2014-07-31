using System.Collections.Generic;

namespace signum
{
    public interface ServiceExecutor
    {
        ServiceExecutorDescriptor Describe();
        void Execute(Context context);
        void Compensate(Context context);
    }
}