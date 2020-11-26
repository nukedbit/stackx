using StackX.Pipeline.Converters;
using System.Threading.Tasks;

namespace StackX.Pipeline
{
    public abstract class PipeElement : IPipeElement
    {
        protected virtual Converter[] Converters => new Converter[0];

        internal abstract Task<PipeElementResult> ExecuteInternalAsync(object args, PipelineState state);       
    }
}