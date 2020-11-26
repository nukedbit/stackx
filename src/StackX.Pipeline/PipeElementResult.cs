namespace StackX.Pipeline
{
    public abstract record PipeElementResult
    {
        public virtual object Result { get; init; }
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
