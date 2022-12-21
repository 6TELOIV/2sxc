﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using ToSic.Eav.Apps;
using ToSic.Eav.Apps.Environment;
using ToSic.Eav.Apps.ImportExport;
using ToSic.Eav.Configuration;
using ToSic.Eav.Context;
using ToSic.Eav.Identity;
using ToSic.Eav.ImportExport.Json;
using ToSic.Eav.ImportExport.Serialization;
using ToSic.Lib.Logging;
using ToSic.Eav.Persistence.Logging;
using ToSic.Eav.Run;
using ToSic.Eav.WebApi.Assets;
using ToSic.Eav.WebApi.Dto;
using ToSic.Eav.WebApi.Validation;
using ToSic.Lib.DI;
using ToSic.Lib.Services;

namespace ToSic.Sxc.WebApi.ImportExport
{
    public class ImportContent: ServiceBase
    {

        #region DI Constructor

        public ImportContent(IZoneMapper zoneMapper, 
            IEnvironmentLogger envLogger,
            ILazySvc<Import> importerLazy,
            ILazySvc<XmlImportWithFiles> xmlImportWithFilesLazy,
            ZipImport zipImport,
            ILazySvc<JsonSerializer> jsonSerializerLazy, 
            IGlobalConfiguration globalConfiguration,
            IAppStates appStates,
            SystemManager systemManager) : base("Bck.Export")
        {
            ConnectServices(
                _zoneMapper = zoneMapper.Init(Log),
                _envLogger = envLogger,
                _importerLazy = importerLazy,
                _xmlImportWithFilesLazy = xmlImportWithFilesLazy,
                _zipImport = zipImport,
                _jsonSerializerLazy = jsonSerializerLazy,
                _globalConfiguration = globalConfiguration,
                _appStates = appStates,
                SystemManager = systemManager
            );
        }

        private readonly IZoneMapper _zoneMapper;
        private readonly IEnvironmentLogger _envLogger;
        private readonly ILazySvc<Import> _importerLazy;
        private readonly ILazySvc<XmlImportWithFiles> _xmlImportWithFilesLazy;
        private readonly ZipImport _zipImport;
        private readonly ILazySvc<JsonSerializer> _jsonSerializerLazy;
        private readonly IGlobalConfiguration _globalConfiguration;
        private readonly IAppStates _appStates;
        protected readonly SystemManager SystemManager;
        private IUser _user;

        public ImportContent Init(IUser user, ILog parentLog)
        {
            this.Init(parentLog);
            _user = user;
            return this;
        }

        #endregion

        public ImportResultDto Import(int zoneId, int appId, string fileName, Stream stream, string defaultLanguage)
        {
            Log.A("import content start");
            var result = new ImportResultDto();

            var allowSystemChanges = _user.IsSystemAdmin;
            if (fileName.EndsWith(".zip"))
            {   // ZIP
                try
                {
                    var zipImport = _zipImport;

                    zipImport.Init(zoneId, appId, _user.IsSystemAdmin, Log);
                    var temporaryDirectory = Path.Combine(_globalConfiguration.TemporaryFolder, Mapper.GuidCompress(Guid.NewGuid()).Substring(0, 8));

                    result.Success = zipImport.ImportZip(stream, temporaryDirectory);
                    result.Messages.AddRange(zipImport.Messages);
                }
                catch (Exception ex)
                {
                    _envLogger.LogException(ex);
                }
            }
            else
            {   // XML
                using (var fileStreamReader = new StreamReader(stream))
                {
                    var xmlImport = _xmlImportWithFilesLazy.Value.Init(defaultLanguage, allowSystemChanges, Log);
                    var xmlDocument = XDocument.Parse(fileStreamReader.ReadToEnd());
                    result.Success = xmlImport.ImportXml(zoneId, appId, xmlDocument);
                    result.Messages.AddRange(xmlImport.Messages);
                }
            }
            return result;
        }


        public ImportResultDto ImportContentType(int zoneId, int appId, List<FileUploadDto> files, string defaultLanguage)
        {
            var callLog = Log.Fn<ImportResultDto>($"{zoneId}, {appId}, {defaultLanguage}");

            try
            {
                // 0. Verify it's json etc.
                if (files.Any(file => !Json.IsValidJson(file.Contents)))
                    throw new ArgumentException("a file is not json");

                // 1. create the content type
                var serializer = _jsonSerializerLazy.Value.Init(Log).SetApp(_appStates.Get(new AppIdentity(zoneId, appId)));

                var types = files.Select(f => serializer.DeserializeContentType(f.Contents)).ToList();

                if (types.Any(t => t == null))
                    throw new NullReferenceException("One ContentType is null, something is wrong");

                // 2. Import the type
                var import = _importerLazy.Value.Init(Log).Init(zoneId, appId, true, true);
                import.ImportIntoDb(types, null);

                Log.A($"Purging {zoneId}/{appId}");
                SystemManager.Purge(zoneId, appId);

                // 3. possibly show messages / issues
                return callLog.ReturnAsOk(new ImportResultDto(true));
            }
            catch (Exception ex)
            {
                _envLogger.LogException(ex);
                return callLog.Return(new ImportResultDto(false, ex.Message, Message.MessageTypes.Error), "error");
            }
        }
    }
}
