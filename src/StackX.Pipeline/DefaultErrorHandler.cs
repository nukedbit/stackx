using System.Threading.Tasks;

namespace StackX.Pipeline
{
    public class DefaultErrorHandler : ErrorHandler
    {
        protected override Task<PipeElementResult> OnExecuteAsync(PipeErrorResult error)
        {
            return Task.FromResult<PipeElementResult>(error);
        }
    }
}