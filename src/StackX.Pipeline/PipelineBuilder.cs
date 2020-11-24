using ServiceStack.Logging;
using System;
using System.Collections.Generic;

namespace StackX.Pipeline
{
    public sealed class PipelineBuilder
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof(PipelineBuilder));
        readonly List<PipeElement> _pipelineElements = new();
        private ErrorHandler _errorHandler;
        private RestartFilter? _restartFilter;
        private int? _restartCountLimit;
        private DefaultStatusManager _defaultStatusManager;


        public PipelineBuilder() : this(false)
        {
            
        }
        public PipelineBuilder(bool enableLogging)
        {
            IsLoggingEnabled = enableLogging;
            _errorHandler = new DefaultErrorHandler();
            _defaultStatusManager = new DefaultStatusManager();
        }

        public bool IsLoggingEnabled { get; private set; }

        public void SetLogging(bool enable)
        {
            IsLoggingEnabled = enable;
            foreach (var el in _pipelineElements)
            {
                if (el is ILoggingPipeElementDecorator decorator)
                {
                    decorator.SetLogging(enable);
                }
            }
        }

        private void AddDecorated(PipeElement element)
        {
            if (_logger is null)
            {
                _pipelineElements.Add(element);
                return;
            }
            if (element is CanExecutePipeElement canExecutePipeElement)
            {
                var decorator = new LoggingCanExecutePipeElementDecorator(canExecutePipeElement);
                decorator.SetLogging(IsLoggingEnabled);
                _pipelineElements.Add(decorator);
            }
            else
            {
                var decorator = new LoggingPipeElementDecorator(element);
                decorator.SetLogging(IsLoggingEnabled);
                _pipelineElements.Add(decorator);
            }
        }

        public PipelineBuilder Add(PipeElement element)
        {
            AddDecorated(element);
            return this;
        }
        public PipelineBuilder Add<TElement>()
            where TElement : PipeElement
        {
            var newElement = Activator.CreateInstance<TElement>();
            AddDecorated(newElement);
            return this;
        }

        public PipelineBuilder OnError(ErrorHandler errorHandler)
        {
            _errorHandler = errorHandler;
            return this;
        }

        public PipelineBuilder SetRestartLimit(int restartLimit)
        {
            _restartCountLimit = restartLimit;
            return this;
        }

        public PipelineBuilder OnRestart(RestartFilter restartFilter)
        {
            _restartFilter = restartFilter;
            return this;
        }

        public PipelineBuilder SetStatusManager(DefaultStatusManager defaultStatusManager)
        {
            _defaultStatusManager = defaultStatusManager;
            return this;
        }

        public IPipeline<TInput> Build<TInput>()
        {
            if (_errorHandler is null) throw new NullReferenceException("Error handler should not be null");
            return new Pipeline<TInput>(_pipelineElements, _errorHandler, 
                _restartFilter, _defaultStatusManager, _restartCountLimit);
        }
    }
}
