using System.Threading.Tasks;

namespace StackX.Pipeline
{
    public interface IPipeline<in TInput>
    {
        Task<PipeElementResult> RunAsync(TInput input);
    }
}