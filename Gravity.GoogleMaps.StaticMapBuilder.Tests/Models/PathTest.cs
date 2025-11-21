using System.Globalization;
using OneOf;

namespace Gravity.GoogleMaps.StaticMapBuilder.Tests.Models;

[TestSubject(typeof(Path))]
public class PathTest
{
    [Fact]
    public void Constructor_Sets_Properties()
    {
        Path path = new(Weight: 10, Color: StaticMapColor.Blue, FillColor: new HexColor("0xFF0000"), Geodesic: true);

        Assert.Equal(10, path.Weight);
        Assert.True(path.Geodesic);
        Assert.True(path.Color?.IsT0);
        Assert.True(path.FillColor?.IsT1);
    }

    [Theory]
    [InlineData(48.85, 2.35)]
    [InlineData(-90, -180)]
    [InlineData(90, 180)]
    public void AddPoint_WithValidCoordinates_AddsPoint(double lat, double lng)
    {
        Path path = new();
        path.AddPoint(lat, lng);

        Assert.NotNull(path.Points);
        Assert.Contains($"{lat.ToString(CultureInfo.InvariantCulture)},{lng.ToString(CultureInfo.InvariantCulture)}",
            path.Points);
    }

    [Fact]
    public void AddPoint_WithLocation_AddsNamedPointAndIncrementsCounters()
    {
        Path path = new();
        path.AddPoint("Paris");

        if (path.Points is null) throw new NullReferenceException();
        
        Assert.Contains("Paris", path.Points);
        Assert.Equal(1, path.LocationCount);
    }

    [Theory]
    [InlineData(91, 0)]
    [InlineData(-91, 0)]
    [InlineData(0, 181)]
    [InlineData(0, -181)]
    public void AddPoint_WithInvalidCoordinates_Throws(double lat, double lng)
    {
        Path path = new();

        Assert.Throws<ArgumentOutOfRangeException>(() => path.AddPoint(lat, lng));
    }

    [Fact]
    public void AddPolyline_ThenAddPoint_Throws()
    {
        Path path = new();
        path.AddPolyline("encoded");

        Assert.Throws<ArgumentException>(() => path.AddPoint("Paris"));
    }

    [Fact]
    public void AddPoint_ThenAddPolyline_Throws()
    {
        Path path = new();
        path.AddPoint(48.85, 2.35);

        Assert.Throws<ArgumentException>(() => path.AddPolyline("encoded"));
    }

    [Fact]
    public void AddPolyline_WithValidEncodedString_AddsPolyline()
    {
        Path path = new();
        path.AddPolyline("abc123");
        
        Assert.Equal("abc123", path.Polyline);
    }

    [Fact]
    public void ToString_WithPolyline_ReturnsExpectedFormat()
    {
        Path path = new(Color: StaticMapColor.Green);
        path.AddPolyline("abc123");

        string result = path.ToString();

        Assert.Contains("color:green", result);
        Assert.Contains("enc:abc123", result);
    }

    [Fact]
    public void ToString_WithInsufficientPoints_Throws()
    {
        Path path = new();
        path.AddPoint(48.85, 2.35); // 1 seul point

        Assert.Throws<InvalidOperationException>(() => path.ToString());
    }

    [Theory]
    [InlineData(
        "weight:8|color:0x00FF00|fillcolor:red|geodesic:true|48.85,2.35|48.86,2.36",
        8,
        "0x00FF00",
        "red",
        true,
        48.85, 2.35,
        48.86, 2.36)]
    [InlineData(
        "48.1,2.1|48.2,2.2",
        5,
        null,
        null,
        false,
        48.1, 2.1,
        48.2, 2.2)]
    public void ToString_ReturnsExactExpectedString(
        string expected,
        int weight,
        string? color,
        string? fillColor,
        bool geodesic,
        double lat1, double lng1,
        double lat2, double lng2)
    {
        OneOf<StaticMapColor, HexColor>? parsedColor = color switch
        {
            null => null,
            _ when color.StartsWith("0x") => new HexColor(color),
            _ => Enum.TryParse(color, true, out StaticMapColor parsed) ? parsed : throw new ArgumentException("Invalid color")
        };

        OneOf<StaticMapColor, HexColor>? parsedFillColor = fillColor switch
        {
            null => null,
            _ when fillColor.StartsWith("0x") => new HexColor(fillColor),
            _ => Enum.TryParse(fillColor, true, out StaticMapColor parsed) ? parsed : throw new ArgumentException("Invalid fill color")
        };

        Path path = new(weight, parsedColor, parsedFillColor, geodesic);
        path.AddPoint(lat1, lng1);
        path.AddPoint(lat2, lng2);

        string result = path.ToString();

        Assert.Equal(expected, result);
    }

}