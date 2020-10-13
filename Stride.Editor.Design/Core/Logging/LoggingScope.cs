using Stride.Core.Diagnostics;

namespace Stride.Editor.Design.Core.Logging
{
    /// <summary>
    /// A named scope unit connected of the <see cref="TimedScope"/>. Use <see cref="LoggingScope"/> to include scope stack information.
    /// </summary>
    public class LoggingScope : ILogger
    {
        public LoggingScope(ILogger logger)
            => this.Logger = logger;

        /// <inheritdoc/>
        public string Module => Logger.Module;

        /// <summary>
        /// Target to log into.
        /// </summary>
        private ILogger Logger { get; }

        /// <inheritdoc/>
        /// <remarks>Includes <see cref="TimedScope.CurrentScopePath"/> information.</remarks>
        public void Log(ILogMessage logMessage)
        {
            var path = TimedScope.CurrentScopePath;
            (logMessage as LogMessage).Module = TimedScope.IsCurrentLoggingScope(this) 
                ? path
                : $"{path}{(string.IsNullOrEmpty(path) ? "" : "/")}{Module}";

            Logger.Log(logMessage);
        }

        /// <summary>
        /// (Only when initialized with a <see cref="Stride.Core.Diagnostics.Logger"/> instance) <br/>
        /// Activates the log for this logger for a range of <see cref="LogMessageType"/>.
        /// </summary>
        /// <param name="fromLevel">The lowest inclusive level to log for.</param>
        /// <param name="toLevel">The highest inclusive level to log for.</param>
        /// <param name="enabledFlag">if set to <c>true</c> this will enable the log, false otherwise. Default is true.</param>
        /// <remarks>
        /// Outside the specified range the log message type are disabled (!enabledFlag).
        /// </remarks>
        public void ActivateLog(LogMessageType fromLevel, LogMessageType toLevel = LogMessageType.Fatal, bool enabledFlag = true)
        {
            if (Logger is Logger log)
                log.ActivateLog(fromLevel, toLevel, enabledFlag);
            else 
                throw new System.NotSupportedException($"The target logger does not inherit from '{typeof(Logger).FullName}'.");
        }
    }
}
