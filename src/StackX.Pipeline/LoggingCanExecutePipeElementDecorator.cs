using ServiceStack.Logging;
using ServiceStack.Text;
using System;

namespace StackX.Pipeline
{
    internal class LoggingCanExecutePipeElementDecorator : CanExecutePipeElement, ILoggingPipeElementDecorator
    {
        private readonly ILog _logger;
        private readonly CanExecutePipeElement _element;

        public LoggingCanExecutePipeElementDecorator(CanExecutePipeElement element)
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
                if(_logger.IsDebugEnabled)
                    _logger.Debug($"ExecuteAsyncInternal result={res}, args={JsonSerializer.SerializeToString(args)}");
                
                return res;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error on ExecuteAsyncInternal with args={JsonSerializer.SerializeToString(args)}");
                throw;
            }
        }

        internal override bool CanExecuteInternal(object args, PipelineState state)
        {
            if (!IsLoggingEnabled)
                return _element.CanExecuteInternal(args, state);
            try
            {
                var res = _element.CanExecuteInternal(args, state);
                if (_logger.IsDebugEnabled)
                {
                    _logger.Debug($"CanExecuteAsyncInternal result={res}, args={JsonSerializer.SerializeToString(args)}");
                }
                return res;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error on CanExecuteAsyncInternal with args={JsonSerializer.SerializeToString(args)}");
                throw;
            }
        }
    }
}