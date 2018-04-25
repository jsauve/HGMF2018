using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using HGMF2018.Core;
using HGMF2018.Droid;
using Microsoft.AppCenter.Analytics;
using Xamarin.Forms;

[assembly: Dependency(typeof(LoggerService))]
namespace HGMF2018.Droid
{
    public class LoggerService : ILoggerService
    {
        void ILoggerService.LogException(Exception ex)
        {
            try
            {
                if (ex == null)
                    return;

                MethodBase site = ex?.TargetSite;

                var typeName = site?.ReflectedType?.DeclaringType?.Name;

                var methodName = site?.ReflectedType?.Name;

                StackFrame frame = (new StackTrace(ex, true))?.GetFrame(0);

                int lineNumber = 0;
                try { lineNumber = frame.GetFileLineNumber(); } catch { }

                int columnNumber = 0;
                try { columnNumber = frame.GetFileColumnNumber(); } catch { }

                var additionalInfo = new Dictionary<string, string>();

                if (!string.IsNullOrWhiteSpace(ex.Message))
                    additionalInfo.Add("Message", (ex.Message.Length > 64) ? ex.Message.Substring(0, 64) : ex.Message);

                if (!string.IsNullOrWhiteSpace(typeName))
                    additionalInfo.Add("Type", (typeName.Length > 64) ? typeName.Substring(0, 64) : typeName);

                if (!string.IsNullOrWhiteSpace(methodName))
                    additionalInfo.Add("Method", (methodName.Length > 64) ? methodName.Substring(0, 64) : methodName);

                if (lineNumber != 0)
                    additionalInfo.Add("Line", lineNumber.ToString());

                if (columnNumber != 0)
                    additionalInfo.Add("Column", columnNumber.ToString());

                var eventName = $"{ex.GetType().Name}: {ex.Message}";

                if (eventName.Length > 256)
                    eventName = eventName.Substring(0, 256);

                // Azure Mobile Center doesn't yet support handled exceptions, so Im hijacking the Events feature of the service to record handled exceptions
                Analytics.TrackEvent(eventName, additionalInfo);
            }
            catch (Exception ex2)
            {
                // if we fail here, then there's nowher to report it, so just duck
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }
    }
}
