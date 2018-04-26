using System;
using System.Collections.Generic;
using HGMF2018.Core;
using HGMF2018.Droid;
using Microsoft.AppCenter.Crashes;
using Xamarin.Forms;

[assembly: Dependency(typeof(LoggerService))]
namespace HGMF2018.Droid
{
    public class LoggerService : ILoggerService
    {
        IVersionRetrievalService _VersionRetrievalService;

        public LoggerService()
        {
            _VersionRetrievalService = DependencyService.Get<IVersionRetrievalService>();
        }

        void ILoggerService.LogException(Exception ex)
        {
            try
            {
                if (ex == null)
                    return;

                Crashes.TrackError(ex, new Dictionary<string, string>() { {"Version", $"{_VersionRetrievalService.Version}" } });
            }
            catch
            {
                // if we fail here, then there's nowhere to report it, so just duck it
            }
        }
    }
}
