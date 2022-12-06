using Microsoft.Extensions.Configuration;

namespace PostBoy.Core.Settings.System;

public class PostBoyConnectionString : IConfigurationSetting<string>
{
    public PostBoyConnectionString(IConfiguration configuration)
    {
        Value = configuration.GetConnectionString("PostBoyConnectionString");
    }
    
    public string Value { get; set; }
}