using Microsoft.Extensions.Logging;

namespace Alkami.Framework
{
    /// <summary>
    /// The ApplicationLogging class is a static class that provides access to a
    /// LoggerFactory instance to facilitate application logging.
    /// </summary>
    public static class ApplicationLogging
    {
        /// <summary>
        /// A static instance of the configured LoggerFactory to facilitate application logging.
        /// </summary>
        /// <returns>Returns an instance of the application LoggerFactory.</returns>
        public static ILoggerFactory LoggerFactory {get;} = new LoggerFactory()
            .AddConsole((category, logLevel) => (logLevel >= LogLevel.Trace), includeScopes: true)
            .AddDebug((category, logLevel) => (logLevel >= LogLevel.Trace));

        /// <summary>
        /// Creates a type-specific logger from the configured LoggerFactory.
        /// </summary>
        /// <returns>Returns a type-specific logger.</returns>
        public static ILogger CreateLogger<T>() =>
            LoggerFactory.CreateLogger<T>();
    }
}