﻿using System.Text.Json.Serialization;
using ToSic.Sxc.Code.Generate.Internal;

namespace ToSic.Sxc.Backend.Admin;

/// <summary>
/// Used to serialize 'editions' to json for UI
/// </summary>
public class EditionsDto: RichResult
{
    public bool IsConfigured { get; set; }
    public List<EditionDto> Editions { get; set; } = [];

    public List<GeneratorDto> Generators { get; set; } = [];
}

public class EditionDto
{
    public string Name { get; set; }
    public string Description { get; set; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool IsDefault { get; set; }
}

public class GeneratorDto(IFileGenerator generator)
{
    public string Name => generator.Name;
    public string Version => generator.Version;
    public string Description => generator.Description;
    public string OutputLanguage => generator.OutputLanguage;
    public string OutputType => generator.OutputType;
}