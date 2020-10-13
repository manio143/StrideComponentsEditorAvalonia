using System;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Stride.Core.Diagnostics;

namespace Stride.Editor.Design.Core.Logging
{
    /// <summary>
    /// Logging scope which also measures execution time. Safe to use across async calls
    /// </summary>
    /// <example>
    /// static LoggingScope MyScope = new LoggingScope(GlobalLogger.GetLogger(nameof(MyClass)));
    /// 
    /// using(var scope = new TimedScope(MyScope, startWith: TimedScope.Status.Failure))
    /// {
    ///     try
    ///     {
    ///         MyWork();
    ///         scope.Result = TimedScope.Status.Success;
    ///     } catch (Exception e)
    ///     {
    ///         scope.Error(e.Message, e);
    ///         throw;
    ///     }
    /// }
    /// </example>
    public class TimedScope : IDisposable, ILogger
    {
        public enum Status
        {
            Failure = 0,
            Success = 1,
            Warning = 2,
        }

        /// <summary>
        /// Creates a new <see cref="TimedScope"/>, logs start info and starts the stopwatch.
        /// </summary>
        /// <param name="loggingScope">Logging scope to be used - the reported scope name comes form <see cref="LoggingScope.Module"/> property.</param>
        /// <param name="startWith">Initial result status - use <see cref="Status.Failure"/> when the action may throw.</param>
        /// <remarks>Use a <c>using</c> block when possible, otherwise you have to remember to call <see cref="Dispose"/>.</remarks>
        public TimedScope(LoggingScope loggingScope, Status startWith = Status.Success)
        {
            LoggingScope = loggingScope;
            Result = startWith;
            ScopeStack = ScopeStack.Push(this);

            LoggingScope.Info("TimedScope started.");
            Stopwatch = Stopwatch.StartNew();
        }

        private LoggingScope LoggingScope { get; }

        private Stopwatch Stopwatch { get; }

        /// <summary>
        /// Result reported when the <see cref="TimedScope"/> finishes.
        /// </summary>
        public Status Result { get; set; }

        private void Stop()
        {
            Stopwatch.Stop();

            var message = $"TimedScope finished with {Result} in {GetElapsedTime()}.";
            switch (Result)
            {
                case Status.Failure:
                    LoggingScope.Error(message);
                    break;
                case Status.Success:
                    LoggingScope.Info(message);
                    break;
                case Status.Warning:
                    LoggingScope.Warning(message);
                    break;
                default:
                    throw new NotSupportedException($"Status value '{Result}' is not supported.");
            }
        }

        private string GetElapsedTime()
        {
            var elapsed = Stopwatch.Elapsed;
            if (elapsed.TotalMinutes > 1)
                return $"{elapsed.TotalMinutes} min";
            else if (elapsed.TotalSeconds > 1)
                return $"{elapsed.TotalSeconds} sec";
            else return $"{elapsed.TotalMilliseconds} ms";
        }

        /// <summary>
        /// Stops the stopwatch, logs the result, pops nested scope stack.
        /// </summary>
        public void Dispose()
        {
            Stop();
            ScopeStack = ScopeStack.Pop();
        }

        string ILogger.Module => CurrentScopePath; // assumes scope is always stack local
        void ILogger.Log(ILogMessage logMessage) => LoggingScope.Log(logMessage);

        public static string CurrentScopePath
        {
            get
            {
                var stack = ScopeStack.Select(ts => ts.LoggingScope.Module).ToArray();
                if (stack.Length == 0)
                    return "";
                return $"{string.Join("/", stack.Reverse())}";
            }
        }

        internal static bool IsCurrentLoggingScope(LoggingScope scope)
            => !ScopeStack.IsEmpty && ScopeStack.Peek().LoggingScope == scope;

        private static readonly AsyncLocal<ImmutableStack<TimedScope>> LocalScopeStack = new AsyncLocal<ImmutableStack<TimedScope>>();

        private static ImmutableStack<TimedScope> ScopeStack
        {
            get
            {
                if (LocalScopeStack.Value == null)
                    LocalScopeStack.Value = ImmutableStack<TimedScope>.Empty;
                return LocalScopeStack.Value;
            }
            set
            {
                LocalScopeStack.Value = value;
            }
        }
    }
}
