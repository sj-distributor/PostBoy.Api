using Microsoft.Extensions.Configuration;

namespace PostBoy.Core.Settings.Logging;

public class SerilogApiKeySetting : IConfigurationSetting<string>
{
    public SerilogApiKeySetting(IConfiguration configuration)
    {
        Value = configuration.GetValue<string>("Serilog:Seq:ApiKey");
    }
    
    public string Value { get; set; }
}