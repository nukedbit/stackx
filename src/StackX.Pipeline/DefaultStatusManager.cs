using System;

namespace StackX.Pipeline
{
    public class DefaultStatusManager
    {
        public int RestartCount { get; protected set; }
        protected object InitialInput { get; set; }

        internal void IncRestartCount()
        {
            RestartCount += 1;
        }

        internal void SetInitialInput(object initialInput)
        {
            InitialInput = initialInput ?? throw new NullReferenceException("Initial input can't be null");
        }

        protected virtual PipelineState OnBuildPipelineState(PipeElementResult result)
        {
            return new PipelineState(RestartCount, InitialInput);
        }

        internal PipelineState BuildPipelineState(PipeElementResult result)
        {
            return OnBuildPipelineState(result);
        }

        public virtual void Reset()
        {
            RestartCount = 0;
            InitialInput = null;
        }
    }
}