namespace StackX.Pipeline
{
    public class DefaultErrorHandler : ErrorHandler
    {
        protected override PipeElementResult Execute(PipeErrorResult error)
        {
            return error;
        }
    }
}