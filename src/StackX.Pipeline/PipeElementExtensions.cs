using System.Diagnostics.CodeAnalysis;

namespace StackX.Pipeline
{
    public static class PipeElementExtensions
    {
        public static PipeElementResult Success<TResult>(this IPipeElement element, [NotNull] TResult result)
        {
            return new PipeSuccessResult {Result = result};
        }

        public static PipeElementResult Error<TResult>(this IPipeElement element, [NotNull] TResult error)
        {
            return new PipeErrorResult {ErrorObject = error};
        }

        public static PipeElementResult Error<TResult, TError>(this IPipeElement element, [NotNull] TError error,
            [NotNull] TResult result)
        {
            return new PipeErrorResult {ErrorObject = error, Result = result};
        }

        public static PipeElementResult GoToEnd<TResult>(this IPipeElement element, [NotNull] TResult result)
        {
            return new PipeGoToEndResult {Result = result};
        }

        public static PipeElementResult Restart<TResult>(this IPipeElement element, [NotNull] TResult result)
        {
            return new PipeRestartResult {Result = result};
        }
    }
}
