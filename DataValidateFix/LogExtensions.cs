using System;
using NLog;

namespace DataValidateFix
{
    internal static class LogExtensions
    {
        internal static void TryPatching(this Logger log, Action action)
        {
            try
            {
                action();
                log.Info("Patching Successful!");
            }
            catch (Exception e)
            {
                log.Error(e, "Patching failed");
            }
        }
    }
}