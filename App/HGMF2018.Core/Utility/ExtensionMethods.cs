using System;
using MvvmHelpers;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Text.RegularExpressions;
using Xamarin.Forms;

namespace HGMF2018.Core
{
    public static class ExceptionExtensions
    {
        public static void ReportError(this Exception ex)
        {
            //#if !DEBUG
            //            DependencyService.Get<ILoggerService> ().LogException (ex);
            //#endif
        }
    }

    public static class StringExtensions
    {
        public static string ToTimeSinceFormat(this DateTime createdAt)
        {
            var elapsed = DateTime.UtcNow - createdAt.ToUniversalTime();

            if (elapsed.TotalSeconds < 60)
            {
                if (elapsed.TotalSeconds < 1)
                    return "just now";

                if (elapsed.TotalSeconds < 2)
                    return $"{(int)elapsed.TotalSeconds} sec ago";

                return $"{(int)elapsed.TotalSeconds} secs ago";
            }


            if (elapsed.TotalMinutes < 60)
            {
                if ((int)elapsed.TotalMinutes < 2)
                    return $"{(int)elapsed.TotalMinutes} min ago";

                return $"{(int)elapsed.TotalMinutes} mins ago";
            }

            if (elapsed.TotalHours < 24)
            {
                if ((int)elapsed.TotalHours < 2)
                    return $"{(int)elapsed.TotalHours} hr ago";

                return $"{(int)elapsed.TotalHours} hrs ago";
            }


            if (elapsed.TotalDays < 7)
            {
                if (elapsed.TotalDays < 2)
                    return $"{(int)elapsed.TotalDays} day ago";

                return $"{(int)elapsed.TotalDays} days ago";
            }

            if (((int)elapsed.TotalDays / 7) < 2)
                return $"{(int)elapsed.TotalDays / 7} wk ago ";

            return $"{(int)elapsed.TotalDays / 7} wks ago";
        }
    }
}
