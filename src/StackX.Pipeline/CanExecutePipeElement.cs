namespace StackX.Pipeline
{
    public abstract class CanExecutePipeElement : PipeElement
    {
        internal abstract bool CanExecuteInternal(object args, PipelineState state);
    }
}