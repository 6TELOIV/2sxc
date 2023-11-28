﻿using System;
using ToSic.Eav.Plumbing;
using ToSic.Lib.Coding;
using ToSic.Lib.DI;
using ToSic.Lib.Documentation;
using ToSic.Lib.Services;


// ReSharper disable MethodOverloadWithOptionalParameter

namespace ToSic.Sxc.Services;

[PrivateApi("Hide implementation")]
[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
internal class ConvertService16: ServiceBase, IConvertService16
{
    private readonly LazySvc<ConvertForCodeService> _code;
    private readonly ConvertValueService _cnvSvc;

    public ConvertService16(ConvertValueService cnvSvc, LazySvc<ConvertForCodeService> code, LazySvc<IJsonService> json): base("Sxc.CnvSrv")
    {
        ConnectServices(
            _cnvSvc = cnvSvc,
            _code = code,
            _jsonLazy = json
        );
    }

    public bool OptimizeNumbers => true;

    public bool OptimizeBoolean => true;

    public T To<T>(object value) => value.ConvertOrDefault<T>(numeric: OptimizeNumbers, truthy: OptimizeBoolean);

    public T To<T>(object value, NoParamOrder noParamOrder = default, T fallback = default) => _cnvSvc.To(value, noParamOrder, fallback);

    public int ToInt(object value) => _cnvSvc.To<int>(value);
    public int ToInt(object value, NoParamOrder noParamOrder = default, int fallback = 0) => _cnvSvc.To(value, fallback: fallback);

    public Guid ToGuid(object value) => _cnvSvc.To<Guid>(value);
    public Guid ToGuid(object value, NoParamOrder noParamOrder = default, Guid fallback = default) => _cnvSvc.To(value, fallback: fallback);

    public float ToFloat(object value) => _cnvSvc.To<float>(value);
    public float ToFloat(object value, NoParamOrder noParamOrder = default, float fallback = default) => _cnvSvc.To(value, fallback: fallback);

    public decimal ToDecimal(object value) => _cnvSvc.To<decimal>(value);
    public decimal ToDecimal(object value, NoParamOrder noParamOrder = default, decimal fallback = default) => _cnvSvc.To(value, fallback: fallback);

    public double ToDouble(object value) => _cnvSvc.To<double>(value);
    public double ToDouble(object value, NoParamOrder noParamOrder = default, double fallback = default) => _cnvSvc.To(value, fallback: fallback);

    public bool ToBool(object value) => _cnvSvc.To<bool>(value);
    public bool ToBool(object value, NoParamOrder noParamOrder = default, bool fallback = false) => _cnvSvc.To(value, fallback: fallback);
        
    public string ToString(object value) => _cnvSvc.To<string>(value);

    public string ToString(object value, NoParamOrder noParamOrder = default, string fallback = default, bool fallbackOnNull = true) 
        => _cnvSvc.ToString(value, noParamOrder, fallback, fallbackOnNull);

    public string ForCode(object value) => _code.Value.ForCode(value);
    public string ForCode(object value, NoParamOrder noParamOrder = default, string fallback = default) => _code.Value.ForCode(value, noParamOrder, fallback);
        

    public IJsonService Json => _jsonLazy.Value;
    private readonly LazySvc<IJsonService> _jsonLazy;

    #region Invisible Converts for backward compatibility

    public int ToInt32(object value) => ToInt(value);

    public float ToSingle(object value) => ToFloat(value);

    #endregion
}