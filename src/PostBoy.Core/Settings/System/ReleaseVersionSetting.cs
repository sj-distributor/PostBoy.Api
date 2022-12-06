using Microsoft.Extensions.Configuration;

namespace PostBoy.Core.Settings.System;

public class ReleaseVersionSetting : IConfigurationSetting<string>
{
    public ReleaseVersionSetting(IConfiguration configuration)
    {
        Value = configuration.GetValue<string>("ReleaseVersion");
    }
    
    public string Value { get; set; }
}