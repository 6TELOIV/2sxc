﻿using System.Collections.Immutable;
using ToSic.Eav.DataSources;
using ToSic.Eav.DataSources.Queries;
using ToSic.Lib.DI;
using ToSic.Lib.Documentation;
using ToSic.Lib.Services;
using ToSic.Sxc.Apps;
using ToSic.Sxc.Blocks;
using ToSic.Sxc.Context;
using IEntity = ToSic.Eav.Data.IEntity;

namespace ToSic.Sxc.DataSources
{
    /// <summary>
    /// This data-source delivers the core data for a CMS Block. <br/>
    /// It will look up the configuration in the CMS (like the Module-Settings in DNN) to determine what data is needed for the block. <br/>
    /// Usually it will then find a reference to a ContentBlock, from which it determines what content-items are assigned. <br/>
    /// It could also find that the template specifies a query, in which case it would retrieve that. <br/>
    /// <em>Was previously called ModuleDataSource</em>
    /// </summary>
    [PublicApi_Stable_ForUseInYourCode]
    [VisualQuery(
        NiceName = "CMS Block",
        UiHint = "Data for this CMS Block (instance/module)",
        Icon = Icons.RecentActor,
        Type = DataSourceType.Source, 
        GlobalName = "ToSic.Sxc.DataSources.CmsBlock, ToSic.Sxc",
        ExpectsDataOfType = "7c2b2bc2-68c6-4bc3-ba18-6e6b5176ba02",
        In = new []{Eav.Constants.DefaultStreamName},
        HelpLink = "https://docs.2sxc.org/api/dot-net/ToSic.Sxc.DataSources.CmsBlock.html",
        PreviousNames = new []{ "ToSic.SexyContent.DataSources.ModuleDataSource, ToSic.SexyContent" })]
    public sealed partial class CmsBlock : DataSource
    {
        [PrivateApi] internal const string InstanceLookupName = "module";
        [PrivateApi] internal const string ModuleIdKey = "Id";
        [PrivateApi] internal const string FieldInstanceId = "InstanceId";

        /// <summary>
        /// The instance-id of the CmsBlock (2sxc instance, DNN ModId). <br/>
        /// It's named Instance-Id to be more neutral as we're opening it to other platforms
        /// </summary>
        [Configuration(Field = FieldInstanceId, Fallback = "[" + InstanceLookupName + ":" + ModuleIdKey + "]")]
        public int? ModuleId
        {
            get
            {
                Configuration.Parse();
                var listIdString = Configuration.GetThis();
                return int.TryParse(listIdString, out var listId) ? listId : new int?();
            }
            set => Configuration.SetThis(value);
        }

        #region Constructor

        public new class MyServices: MyServicesBase<DataSource.MyServices>
        {
            public LazySvc<CmsRuntime> LazyCmsRuntime { get; }
            public LazySvc<IModule> ModuleLazy { get; }
            public LazySvc<DataSourceFactory> DataSourceFactory { get; }

            public MyServices(DataSource.MyServices rootServices,
                LazySvc<CmsRuntime> lazyCmsRuntime,
                LazySvc<IModule> moduleLazy,
                LazySvc<DataSourceFactory> dataSourceFactory) : base(rootServices)
            {
                ConnectServices(
                    LazyCmsRuntime = lazyCmsRuntime,
                    ModuleLazy = moduleLazy,
                    DataSourceFactory = dataSourceFactory
                );
            }
        }

        public CmsBlock(MyServices services): base(services, $"SDS.CmsBks")
        {
            _services = services;

            Provide(GetContent);
            Provide(ViewParts.StreamHeader, GetHeader);
            Provide(ViewParts.StreamHeaderOld, GetHeader);
        }
        private readonly MyServices _services;
        #endregion


        private ImmutableArray<IEntity> GetContent()
        {
            // First check if BlockConfiguration works - to give better error if not
            if (GetErrorStreamIfConfigOrViewAreMissing(out var immutableArray)) 
                return immutableArray;

            return GetStream(BlockConfiguration.Content, View.ContentItem,
                BlockConfiguration.Presentation, View.PresentationItem, false);
        }

        private ImmutableArray<IEntity> GetHeader()
        {
            // First check if BlockConfiguration works - to give better error if not
            if (GetErrorStreamIfConfigOrViewAreMissing(out var immutableArray)) 
                return immutableArray;

            return GetStream(BlockConfiguration.Header, View.HeaderItem,
                BlockConfiguration.HeaderPresentation, View.HeaderPresentationItem, true);
        }

        private bool GetErrorStreamIfConfigOrViewAreMissing(out ImmutableArray<IEntity> immutableArray)
        {
            if (BlockConfiguration == null)
            {
                immutableArray = new DataSourceErrorHandling()
                    .CreateErrorList(title: "CmsBlock Configuration Missing", message: "Cannot find configuration of current CmsBlock");
                return true;
            }

            if (View == null)
            {
                immutableArray = new DataSourceErrorHandling()
                    .CreateErrorList(title: "CmsBlock View Missing", message: "Cannot find View configuration of current CmsBlock");
                return true;
            }

            immutableArray = new ImmutableArray<IEntity>();
            return false;
        }

    }
}