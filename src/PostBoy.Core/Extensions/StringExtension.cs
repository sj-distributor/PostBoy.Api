namespace PostBoy.Core.Extensions;

public static class StringExtension
{
    public static string ToCamelCase(this string str)
    {
        return char.ToLowerInvariant(str[0]) + str[1..];
    }
    
    public static string GenerateRandomNumbers(int length)
    {
        var random = new Random();
        var s = string.Empty;
        for (var i = 0; i < length; i++)
            s = string.Concat(s, random.Next(10).ToString());
        return s;
    }
}