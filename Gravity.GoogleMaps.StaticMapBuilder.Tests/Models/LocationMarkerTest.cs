using Gravity.GoogleMaps.StaticMapBuilder.Models;

namespace Gravity.GoogleMaps.StaticMapBuilder.Tests.Models;

[TestSubject(typeof(LocationMarker))]
public class LocationMarkerTest
{
    [Theory]
    [InlineData("Paris")]
    [InlineData("New York")]
    [InlineData("123 Main St, Springfield")]
    public void ToString_EndsWithLocation(string location)
    {
        LocationMarker marker = new(location);
        string result = marker.ToString();

        Assert.EndsWith($"{location}", result);
    }
}