﻿using System;
using ToSic.Eav.Apps.Interfaces;
using ToSic.Eav.Logging;
using ToSic.SexyContent;

namespace ToSic.Sxc.Interfaces
{
    internal interface IMapAppToInstance
    {
        int? GetAppIdFromInstance(IInstanceInfo instance, int zoneId);
        void SetAppIdForInstance(IInstanceInfo instance, IAppEnvironment env, int? appId, ILog parentLog);


        void ClearPreviewTemplate(int instanceId);

        void SetPreviewTemplate(int instanceId, Guid previewTemplateGuid);

        void SetContentGroup(int instanceId, bool wasCreated, Guid guid);

        ContentGroup GetInstanceContentGroup(ContentGroupManager cgm, ILog log, int instanceId, int? pageId);

        void UpdateTitle(SxcInstance sxcInstance, Eav.Interfaces.IEntity titleItem);
    }
}