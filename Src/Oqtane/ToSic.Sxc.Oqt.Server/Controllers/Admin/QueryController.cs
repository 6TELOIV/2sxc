﻿using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using ToSic.Eav.Apps;
using ToSic.Eav.Apps.Parts;
using ToSic.Eav.DataSources.Catalog;
using ToSic.Eav.WebApi;
using ToSic.Eav.WebApi.Dto;
using ToSic.Eav.WebApi.PublicApi;
using ToSic.Sxc.Apps;
using ToSic.Sxc.LookUp;
using ToSic.Sxc.Oqt.Shared;

namespace ToSic.Sxc.Oqt.Server.Controllers.Admin
{
    /// <summary>
    /// Proxy Class to the EAV PipelineDesignerController (Web API Controller)
    /// </summary>
    [ValidateAntiForgeryToken]
    [Authorize(Roles = Oqtane.Shared.Constants.AdminRole)]
    [Route(WebApiConstants.WebApiStateRoot + "/admin/[controller]/[action]")]
    public class QueryController : OqtStatefulControllerBase, IQueryController
    {
        private readonly Lazy<QueryApi> _queryLazy;
        private readonly Lazy<CmsManager> _cmsManagerLazy;
        private readonly Lazy<AppConfigDelegate> _configProviderLazy;
        protected override string HistoryLogName => "Api.Query";

        public QueryController(StatefulControllerDependencies dependencies, 
            Lazy<QueryApi> queryLazy,
            Lazy<CmsManager> cmsManagerLazy,
            Lazy<AppConfigDelegate> configProviderLazy
            ) : base(dependencies)
        {
            _queryLazy = queryLazy;
            _cmsManagerLazy = cmsManagerLazy;
            _configProviderLazy = configProviderLazy;
        }

        /// <summary>
        /// Get a Pipeline with DataSources
        /// </summary>
        [HttpGet]
        public QueryDefinitionDto Get(int appId, int? id = null) => _queryLazy.Value.Init(appId, Log).Definition(appId, id);

        /// <summary>
        /// Get installed DataSources from .NET Runtime but only those with [PipelineDesigner Attribute]
        /// </summary>
        [HttpGet]
        public IEnumerable<DataSourceDto> DataSources() => new DataSourceCatalog(Log).QueryDataSources();// QueryRuntime.QueryDataSources();

        /// <summary>
        /// Save Pipeline
        /// </summary>
        /// <param name="data">JSON object { pipeline: pipeline, dataSources: dataSources }</param>
        /// <param name="appId">AppId this Pipeline belongs to</param>
        /// <param name="id">PipelineEntityId</param>
        [HttpPost]
        public QueryDefinitionDto Save([FromBody] QueryDefinitionDto data, int appId, int id)
            => _queryLazy.Value.Init(appId, Log).Save(data, appId, id);


        /// <summary>
        /// Query the Result of a Pipeline using Test-Parameters
        /// </summary>
        [HttpGet]
        public QueryRunDto Run(int appId, int id)
        {
            var block = GetBlock();
            var context = GetContext();
            var config = _configProviderLazy.Value.Init(Log).GetConfigProviderForModule(context, block?.App, block);
            return _queryLazy.Value.Init(appId, Log).Run(appId, id, config);
        }

        /// <summary>
	    /// Clone a Pipeline with all DataSources and their configurations
	    /// </summary>
	    [HttpGet]
        public void Clone(int appId, int id) => _queryLazy.Value.Init(appId, Log).Clone(appId, id);


        /// <summary>
        /// Delete a Pipeline with the Pipeline Entity, Pipeline Parts and their Configurations. Stops if the if the Pipeline Entity has relationships to other Entities or is in use in a 2sxc-Template.
        /// </summary>
        [HttpDelete]
        public bool Delete(int appId, int id)
            => _cmsManagerLazy.Value.Init(State.Identity(null, appId), true, Log)
                .DeleteQueryIfNotUsedByView(id, Log);

        [HttpPost]
        public bool Import(EntityImportDto args) => _queryLazy.Value.Init(args.AppId, Log).Import(args);

    }
}