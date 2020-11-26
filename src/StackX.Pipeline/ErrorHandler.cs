using System;
using System.Threading.Tasks;

namespace StackX.Pipeline
{
    public abstract class ErrorHandler
    {
        protected virtual Task<PipeElementResult> OnExecuteAsync(PipeErrorResult error)
        {
            throw new NotImplementedException();
        }

        internal Task<PipeElementResult> ExecuteInternalAsync(PipeErrorResult error) =>
            OnExecuteAsync(error);
    }
}