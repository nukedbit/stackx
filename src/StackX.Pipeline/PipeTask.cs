﻿using System;
using System.Linq;

namespace StackX.Pipeline
{
    /// <summary>
    /// This is the base class for all pipe tasks, each one can override CanExecute, to indicated if
    /// based on the received parameters if it can execute this task.
    /// In the Execute override you can write your own task logic
    /// </summary>
    /// <typeparam name="TSArgs"></typeparam>
    public abstract class PipeTask<TSArgs> : CanExecutePipeElement
    {
        /// <summary>
        /// Determine if the task can be executed by default it's always true
        /// </summary>
        /// <param name="args">Task Argument Object</param>
        /// <param name="state">Pipe Status Object</param>
        /// <returns></returns>
        protected virtual bool CanExecute(TSArgs args, PipelineState state)
        {
            return true;
        }
        
        internal override bool CanExecuteInternal(object args, PipelineState state)
        {
            if (args is TSArgs tsArgs)
                return CanExecute(tsArgs, state);
            if (Converters.Length == 0)
                return CanExecute((TSArgs)args, state);
            var converter = Converters.SingleOrDefault(t => t.CanConvert(args.GetType()));
            var input = converter == null ? args : converter.Convert(args);
            return CanExecute((TSArgs)input, state);
        }

        /// <summary>
        /// Define here your own task logic 
        /// </summary>
        /// <param name="args">Task Argument</param>
        /// <param name="state">Pipe state</param>
        /// <returns></returns>
        protected abstract PipeElementResult Execute(TSArgs args, PipelineState state);
        
        internal override PipeElementResult ExecuteInternal(object args, PipelineState state)
        {
            try
            {
                var converter = Converters.SingleOrDefault(t => t.CanConvert(args.GetType()));
                var input = converter == null ? args : converter.Convert(args);
                return Execute((TSArgs)input, state);
            }
            catch (Exception ex)
            {
                return this.Error(ex);
            }
        }
    }
}