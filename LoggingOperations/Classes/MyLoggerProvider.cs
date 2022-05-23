using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;
using System.IO;

namespace LoggingOperations.Classes
{
    internal class MyLoggerProvider : ILoggerProvider
    {
        /*Класс провайдера логгирования должен реализовать интерфейс ILoggerProvider. В этом интерфейсе определены
         * два метода:
         * CreateLogger: создает и возвращает объект логгера. Для создания логгера используется путь к файлу,
         * который передается через конструктор;
         * Dispose: управляет освобождение ресурсов. В данном случае пустая реализация */
        public ILogger CreateLogger(string categoryName)
        {
            return new MyLogger();
        }

        public void Dispose() { }

        private class MyLogger : ILogger
        {
            public IDisposable BeginScope<TState>(TState state)
            {
                return null;
            }

            public bool IsEnabled(LogLevel logLevel)
            {
                return true;
            }

            public void Log<TState>(LogLevel logLevel, EventId eventId,
                    TState state, Exception exception, Func<TState, Exception, string> formatter)
            {
                File.AppendAllText("log.txt", formatter(state, exception));
                Console.WriteLine(formatter(state, exception));
            }
        }
    }
}
