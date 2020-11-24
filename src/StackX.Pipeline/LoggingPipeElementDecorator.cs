using ServiceStack.Logging;
using ServiceStack.Text;
using System;

namespace StackX.Pipeline
{
    internal class LoggingPipeElementDecorator : PipeElement, ILoggingPipeElementDecorator
    {
        private readonly ILog _logger;
        private readonly PipeElement _element;

        public LoggingPipeElementDecorator(PipeElement element)
        {
            _logger = LogManager.GetLogger(element.GetType());
            _element = element;
        }

        public void SetLogging(bool enable) => IsLoggingEnabled = enable;

        public bool IsLoggingEnabled { get; private set; }


        internal override PipeElementResult ExecuteInternal(object args, PipelineState state)
        {
            if (!IsLoggingEnabled)
                return _element.ExecuteInternal(args, state);
            try
            {
                var res = _element.ExecuteInternal(args, state);
                _logger.Info($"ExecuteAsyncInternal result={res}, args={JsonSerializer.SerializeToString(args)}");
                return res;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error on ExecuteAsyncInternal with args={JsonSerializer.SerializeToString(args)}");
                throw;
            }
        }
    }
}