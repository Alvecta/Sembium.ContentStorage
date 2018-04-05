using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Common
{
    public interface ILogger
    {
        void LogTrace(string message, params object[] args);
        void LogInfo(string message, params object[] args);
        void LogError(string message, Exception exception);
        void LogFatal(string message, params object[] args);
        void LogFatal(string message, Exception exception);
    }
}
