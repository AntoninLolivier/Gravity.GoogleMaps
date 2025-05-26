namespace Gravity.GoogleMaps.StaticMapBuilder.Extensions;

public static class StringExtensions
{
    public static string EncodeToUrl(this string unencoded)
    {
        unencoded = unencoded.Replace("+", " ");
        return Uri.EscapeDataString(unencoded);
    }
}