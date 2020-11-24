using System;
using System.Linq;

namespace StackX.Pipeline
{
    public interface IFilter
    {

    }

    public abstract class Filter<TInput> : PipeElement, IFilter
    {

        protected virtual PipeElementResult Execute(TInput input, PipelineState state)
        {
            throw new NotImplementedException(nameof(Execute));
        }

        internal override PipeElementResult ExecuteInternal(object args, PipelineState state)
        {
            var converter = Converters.SingleOrDefault(t => t.CanConvert(args.GetType()));
            var input = converter == null ? args : converter.Convert(args);
            return Execute((TInput)input, state);
        }
    }
}