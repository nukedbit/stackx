namespace StackX.Pipeline
{
    public abstract record PipeElementResult
    {
        public virtual object Result { get; init; }

        public static PipeElementResult Success(object @object)
        {
            return new PipeSuccessResult { Result = @object};
        }

        public static PipeElementResult Error(object @error, object @object = null)
        {
            return new PipeErrorResult{ Result = @object, ErrorObject = error};
        }

        public static PipeElementResult GoToEnd(object result)
        {
            return new PipeGoToEndResult { Result = result};
        }

        public static PipeElementResult Restart(object result)
        {
            return new PipeRestartResult {Result = result};
        }
    }

    public record PipeErrorResult : PipeElementResult
    {
        public object ErrorObject { get; init; }
    }

    public record PipeSuccessResult : PipeElementResult;

    public record PipeGoToEndResult : PipeSuccessResult;

    public record PipeRestartResult : PipeElementResult;

    public record PipeRestartLimitReachedResult : PipeElementResult;
}
