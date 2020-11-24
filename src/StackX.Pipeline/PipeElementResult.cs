namespace StackX.Pipeline
{
    public abstract class PipeElementResult
    {
        public object Result { get; private set; }

        protected PipeElementResult(object result)
        {
            Result = result;
        }

        public static PipeElementResult Success(object @object)
        {
            return new PipeSuccessResult(@object);
        }

        public static PipeElementResult Error(object @error, object @object = null)
        {
            return new PipeErrorResult(@object, @error);
        }

        public static PipeElementResult GoToEnd(object result)
        {
            return new PipeGoToEndResult(result);
        }

        public static PipeElementResult Restart(object result)
        {
            return new PipeRestartResult(result);
        }
    }

    public class PipeErrorResult : PipeElementResult
    {
        public object ErrorObject { get; private set; }

        public PipeErrorResult(object result, object errorObject) : base(result)
        {
            ErrorObject = errorObject;
        }
    }

    public class PipeSuccessResult : PipeElementResult
    {
        public PipeSuccessResult(object result) : base(result)
        {

        }
    }


    public class PipeGoToEndResult : PipeSuccessResult
    {
        public PipeGoToEndResult(object result) : base(result)
        {
        }
    }

    public class PipeRestartResult : PipeElementResult
    {
        public PipeRestartResult(object result) : base(result)
        {

        }
    }

    public class PipeRestartLimitReachedResult : PipeElementResult
    {
        internal PipeRestartLimitReachedResult(PipeElementResult lastResult)
            : base(lastResult)
        {

        }
    }
}
