﻿using ToSic.Eav.Data;
using ToSic.Lib.Helpers;
using ToSic.Sxc.Code;
using ToSic.Sxc.Data;
using ToSic.Sxc.Data.Decorators;

namespace ToSic.Sxc.Apps;

public partial class App
{
    #region Dynamic Properties: Configuration, Settings, Resources

    // Create config object. Note that AppConfiguration could be null, then it would use default values
    /// <inheritdoc />
    public IAppConfiguration Configuration => AppStateWIP.Configuration;// _appConfig.Get(() => new AppConfiguration(AppConfiguration, Log));
    //private readonly GetOnce<IAppConfiguration> _appConfig = new();

    private DynamicEntity MakeDynProperty(IEntity contents, bool propsRequired)
    {
        var wrapped = CmsEditDecorator.Wrap(contents, false);
        return _cdfLazy.Value.AsDynamic(wrapped, propsRequired: propsRequired);
    }

    internal void SetupAsConverter(CodeDataFactory cdf) => _cdfLazy.Inject(cdf);

    /// <inheritdoc cref="IDynamicCode12.Settings" />
    public dynamic Settings => AppSettings == null ? null : _settings.Get(() => MakeDynProperty(AppSettings, propsRequired: false));
    private readonly GetOnce<dynamic> _settings = new();

    /// <inheritdoc cref="IDynamicCode12.Resources" />
    public dynamic Resources => AppResources == null ? null : _res.Get(() => MakeDynProperty(AppResources, propsRequired: false));
    private readonly GetOnce<dynamic> _res = new();

    #endregion


}