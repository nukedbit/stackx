using System;

namespace StackX.Pipeline
{
    public abstract class ErrorHandler
    {
        protected virtual PipeElementResult Execute(PipeErrorResult error)
        {
            throw new NotImplementedException();
        }

        internal PipeElementResult ExecuteInternal(PipeErrorResult error) =>
            Execute(error);
    }
}