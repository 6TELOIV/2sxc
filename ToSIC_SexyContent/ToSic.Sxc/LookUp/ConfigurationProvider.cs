﻿using System;
using System.Collections.Specialized;
using System.Globalization;
using ToSic.Eav;
using ToSic.Eav.Apps;
using ToSic.Eav.Logging.Simple;
using ToSic.Eav.LookUp;
using ToSic.Sxc.Blocks;
using ToSic.Sxc.Web;
using IApp = ToSic.Sxc.Apps.IApp;
#if NETSTANDARD
using System.Linq;
#endif

namespace ToSic.Sxc.LookUp
{
    public class ConfigurationProvider
    {
        /// <summary>
        /// Generate a delegate which will be used to build the configuration based on a new sxc-instance
        /// </summary>
        internal static Func<App, IAppDataConfiguration> Build(IBlockBuilder blockBuilder, bool useExistingConfig)
        {
            return appToUse =>
            {
                // the module id
                var envInstanceId = blockBuilder.Context.Container.Id;

                // check if we'll use the config already on the sxc-instance, or generate a new one
                var config = useExistingConfig
                    ? blockBuilder.Block.Data.Configuration.LookUps
                    : GetConfigProviderForModule(envInstanceId, appToUse as IApp, blockBuilder);

                // return results
                return new AppDataConfiguration(blockBuilder.UserMayEdit,
                    blockBuilder.Environment.PagePublishing.IsEnabled(envInstanceId), config);
            };
        }

        /// <summary>
        /// Generate a delegate which will be used to build the configuration based existing stuff
        /// </summary>
        internal static Func<App, IAppDataConfiguration> Build(bool userMayEdit, bool publishingEnabled, ILookUpEngine config) 
            => appToUse => new AppDataConfiguration(userMayEdit, publishingEnabled, config);

        /// <summary>
        /// Generate a delegate which will be used to build a basic configuration with very little context
        /// </summary>
        internal static Func<App, IAppDataConfiguration> Build(bool userMayEdit, bool publishingEnabled)
            => appToUse => new AppDataConfiguration(userMayEdit, publishingEnabled,
                GetConfigProviderForModule(0, appToUse as IApp, null));



        // note: not sure yet where the best place for this method is, so it's here for now
        // will probably move again some day
        internal static LookUpEngine GetConfigProviderForModule(int moduleId, IApp app, IBlockBuilder blockBuilder)
        {
            var log = new Log("Stc.GetCnf", blockBuilder?.Log);

            // Find the standard DNN property sources if PortalSettings object is available
            var envLookups = Factory.Resolve<IGetEngine>().GetEngine(moduleId, blockBuilder?.Log);
            log.Add($"Environment provided {envLookups.Sources.Count} sources");

            var provider = new LookUpEngine(envLookups, blockBuilder?.Log);

            // Add QueryString etc. when running inside an http-context. Otherwise leave them away!
            var http = Factory.Resolve<IHttp>();
            if (http.Current != null)
            {
                log.Add("Found Http-Context, will ty to add params for querystring, server etc.");

                // new
                var paramList = new NameValueCollection();
                if (blockBuilder?.Context.Page.Parameters != null)
                    foreach (var pair in blockBuilder.Context.Page.Parameters)
                        paramList.Add(pair.Key, pair.Value);
                else
                    paramList = http.QueryString;
                provider.Add(new LookUpInNameValueCollection("querystring", paramList));

                // old
#if NET451
                provider.Add(new LookUpInNameValueCollection("server", http.Request.ServerVariables));
                provider.Add(new LookUpInNameValueCollection("form", http.Request.Form));
#else
                // "Not Yet Implemented in .net standard #TodoNetStandard" - might not actually support this
#endif
            }
            else
                log.Add("No Http-Context found, won't add http params to look-up");


            provider.Add(new LookUpInAppProperty("app", app));

            // add module if it was not already added previously
            if (!provider.HasSource("module"))
            {
                var modulePropertyAccess = new LookUpInDictionary("module");
                modulePropertyAccess.Properties.Add("ModuleID", moduleId.ToString(CultureInfo.InvariantCulture));
                provider.Add(modulePropertyAccess);
            }

            // provide the current SxcInstance to the children where necessary
            if (!provider.HasSource(LookUpConstants.InstanceContext) && blockBuilder != null)
            {
                var blockBuilderLookUp = new LookUpCmsBlock(LookUpConstants.InstanceContext, blockBuilder);
                provider.Add(blockBuilderLookUp);
            }
            return provider;
        }
    }
}