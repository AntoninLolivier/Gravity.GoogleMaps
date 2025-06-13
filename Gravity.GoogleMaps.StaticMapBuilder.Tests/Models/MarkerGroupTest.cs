using Gravity.GoogleMaps.StaticMapBuilder.Enums;
using Gravity.GoogleMaps.StaticMapBuilder.Models;

namespace Gravity.GoogleMaps.StaticMapBuilder.Tests.Models;

[TestSubject(typeof(MarkerGroup))]
public class MarkerGroupTest
{

    [Fact]
    public void ToString_ContainsAllLocations_AddedByName()
    {
        MarkerGroup group = new(MarkerSize.Mid);
        group.AddLocation("Paris");
        group.AddLocation("Berlin");

        string result = group.ToString();

        Assert.Contains("Paris", result);
        Assert.Contains("Berlin", result);
        Assert.Contains("size:mid", result);
    }
    
    [Fact]
    public void ToString_ContainsAllCoordinates_AddedAsLatLng()
    {
        MarkerGroup group = new(MarkerSize.Small);
        group.AddCoordinates(48.85, 2.35);
        group.AddCoordinates(52.52, 13.40);

        string result = group.ToString();

        Assert.Contains("48.85,2.35", result);
        Assert.Contains("52.52,13.4", result);
    }

    [Fact]
    public void ToString_PreservesOrderOfLocations()
    {
        MarkerGroup group = new(MarkerSize.Small);
        group.AddLocation("A");
        group.AddCoordinates(1, 2);
        group.AddLocation("B");

        string result = group.ToString();

        Assert.Matches(@"A\|1,2\|B$", result); 
    }
    
    [Fact]
    public void AddLocation_IncrementsLocationCount_OnlyForNamedLocations()
    {
        MarkerGroup group = new(MarkerSize.Default);
        group.AddLocation("Paris");
        group.AddLocation("Rome");
        group.AddCoordinates(10, 20);

        Assert.Equal(2, group.LocationCount);
    }
}