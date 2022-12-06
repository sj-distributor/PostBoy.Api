using System.Reflection;

namespace PostBoy.Messages.Swagger;

public static class SwaggerDocs
{
    public static readonly string XmlName = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
}