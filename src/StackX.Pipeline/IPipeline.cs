namespace StackX.Pipeline
{
    public interface IPipeline<in TInput>
    {
        PipeElementResult Run(TInput input);
    }
}