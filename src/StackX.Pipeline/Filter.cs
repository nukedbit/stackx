using System;
using System.Linq;
using System.Threading.Tasks;

namespace StackX.Pipeline
{
    public interface IFilter
    {

    }

    public abstract class Filter<TInput> : PipeElement, IFilter
    {

        protected virtual Task<PipeElementResult> ExecuteAsync(TInput input, PipelineState state)
        {
            throw new NotImplementedException(nameof(ExecuteAsync));
        }

        internal override Task<PipeElementResult> ExecuteInternalAsync(object args, PipelineState state)
        {
            var converter = Converters.SingleOrDefault(t => t.CanConvert(args.GetType()));
            var input = converter == null ? args : converter.Convert(args);
            return ExecuteAsync((TInput)input, state);
        }
    }
}