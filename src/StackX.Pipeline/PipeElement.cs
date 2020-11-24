using StackX.Pipeline.Converters;

namespace StackX.Pipeline
{
    public abstract class PipeElement : IPipeElement
    {
        protected virtual Converter[] Converters => new Converter[0];

        internal abstract PipeElementResult ExecuteInternal(object args, PipelineState state);       
    }
}