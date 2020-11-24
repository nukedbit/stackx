using System.Collections.Generic;
using System.Reflection;

namespace StackX.Pipeline
{
    class Pipeline<TInput> : IPipeline<TInput>
    {
        private readonly List<PipeElement> _elements;
        private readonly ErrorHandler _errorHandler;
        private readonly RestartFilter? _restartFilter;
        private readonly DefaultStatusManager _defaultStatusManager;
        private readonly int? _restartCountLimit;

        internal Pipeline(List<PipeElement> elements, ErrorHandler errorHandler,
            RestartFilter? restartFilter, DefaultStatusManager defaultStatusManager, int? restartCountLimit)
        {
            _elements = elements;
            _errorHandler = errorHandler;
            _restartFilter = restartFilter;
            _defaultStatusManager = defaultStatusManager;
            _restartCountLimit = restartCountLimit;
        }

        public PipeElementResult Run(TInput input)
        {
            _defaultStatusManager.Reset();
            _defaultStatusManager.SetInitialInput(input);
            return RunInternal(input);
        }

        private PipeElementResult OnRestart(PipeRestartResult result, PipelineState state)
        {
            if (_restartCountLimit.HasValue && _defaultStatusManager.RestartCount > _restartCountLimit.Value - 1)
                return new PipeRestartLimitReachedResult { Result = result };
            var value = TryExecuteRestartFilter(result, state);
            _defaultStatusManager.IncRestartCount();
            return RunInternal(@value);
        }

        private object TryExecuteRestartFilter(PipeRestartResult result, PipelineState state)
        {
            var @value = result.Result;
            if (_restartFilter != null)
            {
                @value = _restartFilter.ExecuteInternal(result,state).Result;
            }
            return value;
        }

        private PipeElementResult RunInternal(object input)
        {
            var result = PipeElementResult.Success(input);
            var pipeState = _defaultStatusManager.BuildPipelineState(result);
            foreach (var element in _elements)
            {

                bool canExecute = CheckCanExecute(element, result.Result, pipeState);
                if (!canExecute)
                    continue;
                result = element.ExecuteInternal(result.Result, pipeState);

                if (result is PipeErrorResult)
                {
                    result = _errorHandler.ExecuteInternal((PipeErrorResult)result);
                    if (result is PipeErrorResult)
                    {
                        break;
                    }
                }

                if (IsExitResult(result))
                {
                    break;
                }
                pipeState = _defaultStatusManager.BuildPipelineState(result);
            }

            if (result is PipeRestartResult)
                result = OnRestart((PipeRestartResult)result, pipeState);
            return result;
        }

        private static bool IsExitResult(PipeElementResult result)
        {
            return result is PipeGoToEndResult || result is PipeRestartResult || result is PipeRestartLimitReachedResult;
        }

        private static bool CheckCanExecute(PipeElement element, object currentInput, PipelineState state)
        {
            try
            {
                if (element is CanExecutePipeElement canExecute)
                {
                    return canExecute.CanExecuteInternal(currentInput, state);
                }
                return true;
            }
            catch (TargetInvocationException ex)
            {
                if (ex.InnerException != null)
                    throw ex.InnerException;
                throw;
            }
        }
    }
}