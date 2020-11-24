namespace StackX.Pipeline
{
    internal interface ILoggingPipeElementDecorator
    {
        void SetLogging(bool enable);
        bool IsLoggingEnabled { get; }
    }
}