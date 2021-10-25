﻿using DotNetNuke.Services.Log.EventLog;
using ToSic.Sxc.Services;

namespace ToSic.Sxc.Dnn.Services
{
    public class DnnLogService : ILogService
    {
        public void Add(string title, string message)
        {
            var logInfo = new LogInfo
            {
                LogTypeKey = EventLogController.EventLogType.ADMIN_ALERT.ToString()
            };
            logInfo.AddProperty(title, message);
            EventLogController.Instance.AddLog(logInfo);
        }
    }
}
