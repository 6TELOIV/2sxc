﻿using ToSic.Lib.Documentation;
using ToSic.Razor.Blade;

namespace ToSic.Sxc.Services
{
    [PrivateApi("WIP")]
    public interface ICmsService
    {
        IHtmlTag Show(object thing,
            string noParamOrder = Eav.Parameters.Protector,
            object container = default,
            string classes = default,
            bool debug = default,
            object imageSettings = default
        );
    }
}
