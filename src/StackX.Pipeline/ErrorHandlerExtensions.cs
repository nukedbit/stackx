using System.Diagnostics.CodeAnalysis;

namespace StackX.Pipeline
{
    public static class ErrorHandlerExtensions
    {
        public static PipeElementResult Success<TResult>(this ErrorHandler errorHandler, [NotNull]TResult result)
        {
            return PipeElementResult.Success(result);
        }

        public static PipeElementResult Error<TResult>(this ErrorHandler errorHandler, [NotNull]TResult error)
        {
            return PipeElementResult.Error(error);
        }
        
        public static PipeElementResult Error<TResult, TError>(this ErrorHandler errorHandler, [NotNull]TError error, [NotNull]TResult result)
        {
            return PipeElementResult.Error(error, result);
        }
        
        public static PipeElementResult GoToEnd<TResult>(this ErrorHandler errorHandler, [NotNull]TResult result)
        {
            return PipeElementResult.GoToEnd(result);
        }
        
        public static PipeElementResult Restart<TResult>(this ErrorHandler errorHandler, [NotNull]TResult result)
        {
            return PipeElementResult.Restart(result);
        }
    }
}