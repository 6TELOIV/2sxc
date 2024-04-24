﻿using Custom.Data;
using ToSic.Eav;
using ToSic.Eav.Apps;

namespace ToSic.Sxc.Code.Generate.Internal;

[PrivateApi]
[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
internal class CSharpCodeSpecs: FileGeneratorSpecs
{
    private const string ItemAccessorDefault = nameof(CustomItem._item);
    private const string DefAutoGenNamespace = "AutoGenerated";

    public string AppName { get; set; } = "App";

    internal IAppState AppState { get; set; }

    public List<IContentType> ExportedContentContentTypes { get; set; }

    /// <summary>
    /// Namespace for the data classes
    /// </summary>
    public string DataNamespace { get; set; } = Constants.AppCode + ".Data";

    public string NamespaceAutoGen { get; set; } = DefAutoGenNamespace;

    /// <summary>
    /// Namespace for the data classes
    /// </summary>
    public string DataNamespaceGenerated => $"{DataNamespace}.{NamespaceAutoGen}";

    /// <summary>
    /// This is added to the end of the class name to indicate that it was auto-generated.
    /// It's quite verbose, to ensure that it's unlikely to clash with a real class name.
    /// </summary>
    public string DataClassGeneratedSuffix { get; set; } = ""; // "AutoGenerated";

    public string DataClassGeneratedPrefix { get; set; } = "ZAutoGen";

    /// <summary>
    /// Default class to inherit from - ATM CustomItem only
    /// </summary>
    public string DataInherits { get; set; } = "Custom.Data." + nameof(CustomItem);

    public string ItemAccessor { get; set; } = ItemAccessorDefault;

    ///// <summary>
    ///// The edition of the generated code
    ///// </summary>
    //public string Edition { get; set; } = "";


    /// <summary>
    /// This is added to the end of a file name to indicate that it was auto-generated.
    /// It's shorter for practical reasons, and should start with a dot and a letter > "c"
    /// so that in the list of files it will be after the ".cs" file which may be created.
    /// </summary>
    public string FileGeneratedSuffix { get; set; } = ".Generated";

    /// <summary>
    /// Tab size for the generated code
    /// </summary>
    public int TabSize { get; set; } = 2;

    /// <summary>
    /// Tabs in front of namespace code
    /// </summary>
    public int TabsNamespace { get; set; } = 0;

    /// <summary>
    /// Tabs in front of class code
    /// </summary>
    public int TabsClass { get; set; } = 1;

    /// <summary>
    /// Tabs in front of property code
    /// </summary>
    public int TabsProperty { get; set; } = 2;
}