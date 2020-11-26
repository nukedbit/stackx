using System.Diagnostics.CodeAnalysis;

namespace StackX.Pipeline
{
    public static class ErrorHandlerExtensions
    {
        public static PipeElementResult Success<TResult>(this ErrorHandler errorHandler, [NotNull]TResult result)
        {
            return new PipeSuccessResult() { Result = result };
        }

        public static PipeElementResult Error<TResult>(this ErrorHandler errorHandler, [NotNull]TResult error)
        {
            return new PipeErrorResult() {ErrorObject = error};
        }
        
        public static PipeElementResult Error<TResult, TError>(this ErrorHandler errorHandler, [NotNull]TError error, [NotNull]TResult result)
        {
            return new PipeErrorResult() {ErrorObject = error, Result = result};
        }
        
        public static PipeElementResult GoToEnd<TResult>(this ErrorHandler errorHandler, [NotNull]TResult result)
        {
            return new PipeGoToEndResult() {Result = result};
        }
        
        public static PipeElementResult Restart<TResult>(this ErrorHandler errorHandler, [NotNull]TResult result)
        {
            return new PipeRestartResult() {Result = result};
        }
    }
}