using System.Diagnostics.CodeAnalysis;

namespace StackX.Pipeline
{
  
    public static class PipeElementExtensions
    {
        
        public static PipeElementResult Success<TResult>(this IPipeElement element, [NotNull]TResult result)
        {
            return PipeElementResult.Success(result);
        }

        public static PipeElementResult Error<TResult>(this IPipeElement element, [NotNull]TResult error)
        {
            return PipeElementResult.Error(error);
        }

        
        public static PipeElementResult Error<TResult, TError>(this IPipeElement element, [NotNull]TError error, [NotNull]TResult result)
        {
            return PipeElementResult.Error(error, result);
        }

        
        public static PipeElementResult GoToEnd<TResult>(this IPipeElement element, [NotNull]TResult result)
        {
            return PipeElementResult.GoToEnd(result);
        }

        
        public static PipeElementResult Restart<TResult>(this IPipeElement element, [NotNull]TResult result)
        {
            return PipeElementResult.Restart(result);
        }
    }
}
