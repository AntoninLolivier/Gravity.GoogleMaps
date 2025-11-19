using Gravity.GoogleMaps.StaticMapBuilder.Builders;
using Gravity.GoogleMaps.StaticMapBuilder.Enums;
using Gravity.GoogleMaps.StaticMapBuilder.Models;
using Gravity.GoogleMaps.StaticMapBuilder.Models.MapStyles;
using Gravity.GoogleMaps.StaticMapBuilder.Options;
using Gravity.GoogleMaps.StaticMapBuilder.Resources;
using Gravity.GoogleMaps.StaticMapBuilder.Types;

namespace Gravity.GoogleMaps.StaticMapBuilder.Tests.Builders;

[TestSubject(typeof(StaticMapsUrlBuilder))]
public class StaticMapsUrlBuilderTest
{
    // Options
    
    private static readonly StaticMapBuilderOptions DisableUrlEncodingOptions = new()
    {
        DisableUrlEncoding = true
    };
    
    // Tests
    
    [Fact]
    public void AddCenterWithLocation_ValidLocation_ShouldAddParameter()
    {
        StaticMapsUrlBuilder builder = new();

        builder.AddCenterWithLocation("New York")
            .AddZoom(0)
            .AddSize(1, 1)
            .AddKey("key");

        string result = builder.Build();
        Assert.Contains("center=New%20York", result);
    }

    [Theory]
    [InlineData("")]
    public void AddCenterWithLocation_InvalidLocation_ShouldThrowArgumentException(string location)
    {
        StaticMapsUrlBuilder builder = new();
        Assert.Throws<ArgumentNullException>(() => builder.AddCenterWithLocation(location));
    }

    [Fact]
    public void AddCenterWithCoordinates_ValidCoordinates_ShouldAddParameter()
    {
        StaticMapsUrlBuilder builder = new();

        builder.AddCenterWithCoordinates(40.712, -74.0060)
            .AddZoom(0)
            .AddSize(1, 1)
            .AddKey("key");

        string result = builder.Build();
        Assert.Contains("center=40.712%2C-74.006", result);
    }

    [Theory]
    [InlineData(5)]
    [InlineData(0)]
    [InlineData(22)]
    public void AddZoom_ValidZoomLevel_ShouldAddParameter(int zoom)
    {
        StaticMapsUrlBuilder builder = new();

        builder.AddZoom(zoom)
            .AddSize(1, 1)
            .AddKey("key");

        string result = builder.Build();
        Assert.Contains($"zoom={zoom}", result);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(23)]
    public void AddZoom_InvalidZoomLevel_ShouldTriggerWarning(int zoom)
    {
        StaticMapsUrlBuilder builder = new();
        builder.AddZoom(zoom)
            .AddSize(1, 1)
            .AddKey("key");

        // No exception for now, but the log should be triggered (ensure fail gracefully)
        string result = builder.Build();
        Assert.Contains($"zoom={Uri.EscapeDataString(zoom.ToString())}", result);
    }

    [Fact]
    public void AddSize_ValidSize_ShouldAddParameter()
    {
        StaticMapsUrlBuilder builder = new();

        builder.AddSize(600, 1)
            .AddCenterWithLocation("Paris")
            .AddKey("key");

        string result = builder.Build();
        Assert.Contains("size=600x1", result);
    }

    [Theory]
    [InlineData(0, 1)]
    [InlineData(600, 0)]
    public void AddSize_InvalidSize_ShouldThrowArgumentException(int width, int height)
    {
        StaticMapsUrlBuilder builder = new();
        Assert.Throws<ArgumentException>(() => builder.AddSize(width, height));
    }

    [Theory]
    [InlineData(MapScale.One)]
    [InlineData(MapScale.Two)]
    public void AddScale_ValidMapScale_ShouldAddParameter(MapScale scale)
    {
        StaticMapsUrlBuilder builder = new();

        builder.AddScale(scale)
            .AddCenterWithLocation("Paris")
            .AddSize(1, 1)
            .AddKey("key");

        string result = builder.Build();
        Assert.Contains($"scale={(int)scale}", result);
    }

    [Fact]
    public void AddFormat_ValidFormat_ShouldAddParameter()
    {
        StaticMapsUrlBuilder builder = new();
        MapFormat format = MapFormats.Jpg;

        builder.AddFormat(format)
            .AddCenterWithLocation("Paris")
            .AddSize(1, 1)
            .AddKey("key");

        string result = builder.Build();
        Assert.Contains($"format={format.ToString().ToLower()}", result);
    }

    [Theory]
    [InlineData(StaticMapType.Roadmap)]
    [InlineData(StaticMapType.Satellite)]
    [InlineData(StaticMapType.Terrain)]
    [InlineData(StaticMapType.Hybrid)]
    public void AddMapType_ValidMapType_ShouldAddParameter(StaticMapType mapType)
    {
        StaticMapsUrlBuilder builder = new();

        builder.AddMapType(mapType)
            .AddCenterWithLocation("Paris")
            .AddSize(1, 1)
            .AddKey("key");

        string result = builder.Build();
        Assert.Contains($"maptype={mapType.ToString().ToLower()}", result);
    }

    [Theory]
    [InlineData("en")]
    [InlineData("fr")]
    public void AddLanguage_ValidLanguage_ShouldAddParameter(string language)
    {
        StaticMapsUrlBuilder builder = new();

        builder.AddLanguage(language)
            .AddCenterWithLocation("Paris")
            .AddSize(1, 1)
            .AddKey("key");

        string result = builder.Build();
        Assert.Contains($"language={language}", result);
    }

    [Fact]
    public void AddLanguage_InvalidLanguage_ShouldThrowArgumentException()
    {
        StaticMapsUrlBuilder builder = new();
        Assert.Throws<ArgumentException>(() => builder.AddLanguage("english")); // More than 2 letters
    }
    
    [Theory]
    [InlineData("US")]
    [InlineData("CA")]
    public void AddRegion_ValidRegion_ShouldAddParameter(string region)
    {
        StaticMapsUrlBuilder builder = new();

        builder.AddRegion(region)
            .AddCenterWithLocation("Paris")
            .AddSize(1, 1)
            .AddKey("key");

        string result = builder.Build();
        Assert.Contains($"region={region}", result);
    }

    [Fact]
    public void AddRegion_InvalidRegion_ShouldThrowArgumentException()
    {
        StaticMapsUrlBuilder builder = new();
        Assert.Throws<ArgumentException>(() => builder.AddRegion("USA")); // More than 2 letters
    }

    [Fact]
    public void AddMapId_ValidId_ShouldAddParameter()
    {
        StaticMapsUrlBuilder builder = new();
        
        builder.AddMapId("mapId")
            .AddCenterWithLocation("Paris")
            .AddSize(1, 1)
            .AddKey("key");
        
        string result = builder.Build();
        Assert.Contains("map_id=mapId", result);
    }
    
    [Fact]
    public void AddMapId_InvalidId_ShouldThrowArgumentException()
    {
        StaticMapsUrlBuilder builder = new();
        Assert.Throws<ArgumentException>(() => builder.AddMapId(new string('x', 17))); // More than 12 characters
    }
    
    [Fact]
    public void AddMarkerGroups_AddsMarkersCorrectly()
    {
        StaticMapsUrlBuilder builder = new();
        MarkerGroup group = new(MarkerSize.Mid);
        group.AddLocation("Paris");

        string url = builder
            .AddMarkerGroups(group)
            .AddSize(1, 1)
            .AddKey("key")
            .Build();

        Assert.Contains("markers=", url);
        Assert.Contains("Paris", url);
    }

    [Fact]
    public void AddMarkerGroups_AccumulatesMarkersIfAlreadyPresent()
    {
        StaticMapsUrlBuilder builder = new();
        MarkerGroup group1 = new(MarkerSize.Small);
        group1.AddLocation("Lyon");
    
        MarkerGroup group2 = new(MarkerSize.Mid);
        group2.AddLocation("Marseille");

        string url = builder
            .AddMarkerGroups(group1)
            .AddMarkerGroups(group2)
            .AddSize(1, 1)
            .AddKey("key")
            .Build();

        Assert.Contains("Lyon", url);
        Assert.Contains("Marseille", url);
        Assert.Equal(1, group1.LocationCount);
        Assert.Equal(1, group2.LocationCount);
    }

    [Fact]
    public void AddMarkerGroups_IncrementsLocationMarkerCount()
    {
        StaticMapsUrlBuilder builder = new();
        MarkerGroup group = new(MarkerSize.Mid);
        group.AddLocation("Nice");
        group.AddLocation("Toulouse");

        builder.AddMarkerGroups(group);

        for (int i = 0; i < 45; i++)
        {
            MarkerGroup g = new(MarkerSize.Small);
            g.AddLocation($"City{i}");
            builder.AddMarkerGroups(g);
        }

        builder.AddSize(1, 1)
            .AddKey("key");

        Assert.Throws<InvalidOperationException>(() => builder.Build()); 
    }

    [Fact]
    public void AddMarkerGroups_AddsIconUrlIfPresent()
    {
        MarkerGroup group = new(MarkerSize.Mid, iconUrl: "http://example.com/icon.png");
        group.AddLocation("Tours");

        string url = new StaticMapsUrlBuilder()
            .AddMarkerGroups(group)
            .AddSize(1, 1)
            .AddKey("key")
            .Build();

        Assert.Contains(Uri.EscapeDataString("icon:http://example.com/icon.png"), url);
    }
    
    [Fact]
    public void AddMarkers_WithMarkerGroup_Throws()
    {
        StaticMapsUrlBuilder builder = new();
        MarkerGroup group = new(MarkerSize.Mid);
        group.AddLocation("Paris");

        Assert.Throws<InvalidOperationException>(() =>
            builder.AddMarkers(group));
    }
    
    [Fact]
    public void AddMarkers_AddsMarkerCorrectly()
    {
        LocationMarker marker = new("Lyon");
        string url = new StaticMapsUrlBuilder()
            .AddMarkers(marker)
            .AddSize(1, 1)
            .AddKey("key")
            .Build();

        Assert.Contains("markers=", url);
        Assert.Contains("Lyon", url);
    }
    
    [Fact]
    public void AddMarkers_AppendsMarkersIfAlreadyPresent()
    {
        LocationMarker m1 = new("Paris");
        LocationMarker m2 = new("Berlin");

        string url = new StaticMapsUrlBuilder()
            .AddMarkers(m1)
            .AddMarkers(m2)
            .AddSize(1, 1)
            .AddKey("key")
            .Build();

        Assert.Contains("Paris", url);
        Assert.Contains("Berlin", url);
    }
    
    [Fact]
    public void AddMarkers_LocationMarkersCountValidated()
    {
        StaticMapsUrlBuilder builder = new();

        for (int i = 0; i < ProjectConstants.LocationMarkersCountLimit + 1; i++)
        {
            LocationMarker m = new($"City{i}");
            builder.AddMarkers(m);
        }

        builder.AddSize(1, 1).AddKey("key");

        Assert.Throws<InvalidOperationException>(() => builder.Build()); // dépasse le quota de location markers
    }
    
    [Fact]
    public void AddMarkers_AddsIconUrlIfPresent()
    {
        LocationMarker marker = new("Toulouse", iconUrl: "http://example.com/icon.png");

        string url = new StaticMapsUrlBuilder()
            .AddMarkers(marker)
            .AddSize(1, 1)
            .AddKey("key")
            .Build();

        Assert.Contains(Uri.EscapeDataString("icon:http://example.com/icon.png"), url);
    }
    
    [Fact]
    public void AddPaths_AddsPathCorrectly()
    {
        Path path = new(Color:new HexColor("0xFFFFFF"));
        path.AddPoint("8th Avenue & 34th St,New York,NY");
        path.AddPoint("8th Avenue & 42nd St,New York,NY");

        string url = new StaticMapsUrlBuilder()
            .AddPaths(path)
            .AddSize(1, 1)
            .AddKey("key")
            .Build();

        Assert.Contains("path=", url);
        Assert.DoesNotContain("path=|", url);
        Assert.Contains("color%3A0xFFFFFF", url);
        Assert.Contains("8th%20Avenue%20%26%2034th%20St%2CNew%20York%2CNY", url);
        Assert.Contains("8th%20Avenue%20%26%2042nd%20St%2CNew%20York%2CNY", url);
    }
    
    [Fact]
    public void AddPaths_AddsMultiplePathsCorrectly()
    {
        Path path1 = new();
        path1.AddPoint("Paris");
        path1.AddPoint("Lyon");

        Path path2 = new();
        path2.AddPoint("Nice");
        path2.AddPoint("Toulouse");

        string url = new StaticMapsUrlBuilder()
            .AddPaths(path1, path2)
            .AddSize(1, 1)
            .AddKey("key")
            .Build();

        Assert.DoesNotContain("path=|", url);
        Assert.Contains("Paris", url);
        Assert.Contains("Nice", url);
    }
    
    [Fact]
    public void AddPaths_AddsPolylineCorrectly()
    {
        Path path = new();
        path.AddPolyline("_fisIp~u|U}|a@pytA_~b@hhCyhS~hResU||x@oig@rwg@amUfbjA}f[roaAynd@|vXxiAt{ZwdUfbjAewYrqGchH~vXkqnAria@c_o@inc@k{g@i`]o|F}vXaj\\h`]ovs@?yi_@rcAgtO|j_AyaJren@nzQrst@zuYh`]v|GbldEuzd@||x@spD|trAzwP|d_@yiB~vXmlWhdPez\\_{Km_`@~re@ew^rcAeu_@zhyByjPrst@ttGren@aeNhoFemKrvdAuvVidPwbVr~j@or@f_z@ftHr{ZlwBrvdAmtHrmT{rOt{Zz}E|c|@o|Lpn~AgfRpxqBfoVz_iAocAhrVjr@rh~@jzKhjp@``NrfQpcHrb^k|Dh_z@nwB|kb@a{R|yh@uyZ|llByuZpzw@wbd@rh~@||Fhqs@teTztrAupHhyY}t]huf@e|Fria@o}GfezAkdW|}[ocMt_Neq@ren@e~Ika@pgE|i|AfiQ|`l@uoJrvdAgq@fppAsjGhg`@|hQpg{Ai_V||x@mkHhyYsdP|xeA~gF|}[mv`@t_NitSfjp@c}Mhg`@sbChyYq}e@rwg@atFff}@ghN~zKybk@fl}A}cPftcAite@tmT__Lha@u~DrfQi}MhkSqyWivIumCria@ciO_tHifm@fl}A{rc@fbjAqvg@rrqAcjCf|i@mqJtb^s|@fbjA{wDfs`BmvEfqs@umWt_Nwn^pen@qiBr`xAcvMr{Zidg@dtjDkbM|d_@");

        string url = new StaticMapsUrlBuilder()
            .AddPaths(path)
            .AddSize(1, 1)
            .AddKey("key")
            .Build();

        Assert.Contains("path=", url);
        Assert.DoesNotContain("path=|", url);
        Assert.Contains("enc%3A_fisIp~u%7CU%7D%7Ca%40pytA_~b%40hhCyhS~hResU%7C%7Cx%40oig%40rwg%40amUfbjA%7Df%5BroaAynd%40%7CvXxiAt%7BZwdUfbjAewYrqGchH~vXkqnAria%40c_o%40inc%40k%7Bg%40i%60%5Do%7CF%7DvXaj%5Ch%60%5Dovs%40%3Fyi_%40rcAgtO%7Cj_AyaJren%40nzQrst%40zuYh%60%5Dv%7CGbldEuzd%40%7C%7Cx%40spD%7CtrAzwP%7Cd_%40yiB~vXmlWhdPez%5C_%7BKm_%60%40~re%40ew%5ErcAeu_%40zhyByjPrst%40ttGren%40aeNhoFemKrvdAuvVidPwbVr~j%40or%40f_z%40ftHr%7BZlwBrvdAmtHrmT%7BrOt%7BZz%7DE%7Cc%7C%40o%7CLpn~AgfRpxqBfoVz_iAocAhrVjr%40rh~%40jzKhjp%40%60%60NrfQpcHrb%5Ek%7CDh_z%40nwB%7Ckb%40a%7BR%7Cyh%40uyZ%7CllByuZpzw%40wbd%40rh~%40%7C%7CFhqs%40teTztrAupHhyY%7Dt%5Dhuf%40e%7CFria%40o%7DGfezAkdW%7C%7D%5BocMt_Neq%40ren%40e~Ika%40pgE%7Ci%7CAfiQ%7C%60l%40uoJrvdAgq%40fppAsjGhg%60%40%7ChQpg%7BAi_V%7C%7Cx%40mkHhyYsdP%7CxeA~gF%7C%7D%5Bmv%60%40t_NitSfjp%40c%7DMhg%60%40sbChyYq%7De%40rwg%40atFff%7D%40ghN~zKybk%40fl%7DA%7DcPftcAite%40tmT__Lha%40u~DrfQi%7DMhkSqyWivIumCria%40ciO_tHifm%40fl%7DA%7Brc%40fbjAqvg%40rrqAcjCf%7Ci%40mqJtb%5Es%7C%40fbjA%7BwDfs%60BmvEfqs%40umWt_Nwn%5Epen%40qiBr%60xAcvMr%7BZidg%40dtjDkbM%7Cd_%40", url);
    }
    
    [Fact]
    public void AddPaths_DisabledEncoding_UsesRawPath()
    {
        Path path = new();
        path.AddPoint("Champs Elysées");
        path.AddPoint("Place de la République");

        string url = new StaticMapsUrlBuilder()
            .AddPaths(path)
            .AddSize(1, 1)
            .AddKey("key")
            .WithOptions(DisableUrlEncodingOptions)
            .Build();

        Assert.DoesNotContain("path=|", url);
        Assert.Contains("Champs Elysées", url); 
    }
    
    [Fact]
    public void AddVisiblePlaceWithLocation_AddsVisibleParameter()
    {
        StaticMapsUrlBuilder builder = new();
        string url = builder
            .AddViewportWithLocation("Paris")
            .AddSize(1, 1)
            .AddKey("key")
            .Build();

        Assert.Contains("visible=Paris", url);
    }
    
    [Fact]
    public void AddVisiblePlaceWithLocation_AddsMultiplePlaces()
    {
        StaticMapsUrlBuilder builder = new();
        string url = builder
            .AddViewportWithLocation("Paris", "Lyon", "Marseille")
            .AddSize(1, 1)
            .AddKey("key")
            .Build();

        string expected = Uri.EscapeDataString("Paris|Lyon|Marseille");
        Assert.Contains(expected, url);
    }
    
    [Fact]
    public void AddVisiblePlaceWithLocation_AppendsIfAlreadyPresent()
    {
        StaticMapsUrlBuilder builder = new();
        string url = builder
            .AddViewportWithLocation("Paris")
            .AddViewportWithLocation("Nice")
            .AddSize(1, 1)
            .AddKey("key")
            .Build();

        Assert.Contains("Paris", url);
        Assert.Contains("Nice", url);
    }
    
    [Fact]
    public void AddVisiblePlaceWithLocation_ExceedLimit_Throws()
    {
        StaticMapsUrlBuilder builder = new();

        for (int i = 0; i < ProjectConstants.LocationsCountLimit + 1; i++) 
        {
            builder.AddViewportWithLocation($"City{i}");
        }

        builder.AddSize(1, 1)
            .AddKey("key");

        Assert.Throws<InvalidOperationException>(() => builder.Build());
    }

    [Fact]
    public void AddVisiblePlaceWithLocation_DisabledEncoding_UsesRaw()
    {
        StaticMapsUrlBuilder builder = new();
        string url = builder
            .AddViewportWithLocation("Champs Elysées")
            .AddSize(1, 1)
            .AddKey("key")
            .WithOptions(DisableUrlEncodingOptions)
            .Build();

        Assert.Contains("Champs Elysées", url);
    }
    
    [Fact]
    public void AddVisiblePlaceWithCoordinates_AddsVisibleParameter()
    {
        StaticMapsUrlBuilder builder = new();

        string url = builder
            .AddVisiblePortWithCoordinates((48.85, 2.35))
            .AddSize(1, 1)
            .AddKey("key")
            .Build();

        string expected = Uri.EscapeDataString("48.85,2.35");
        Assert.Contains(expected, url);
    }

    [Fact]
    public void AddVisiblePlaceWithCoordinates_AddsMultipleCoordinates()
    {
        StaticMapsUrlBuilder builder = new();

        string url = builder
            .AddVisiblePortWithCoordinates((48.85, 2.35), (45.75, 4.85))
            .AddSize(1, 1)
            .AddKey("key")
            .Build();

        string expected = Uri.EscapeDataString("48.85,2.35|45.75,4.85");
        Assert.Contains(expected, url);
    }

    [Fact]
    public void AddVisiblePlaceWithCoordinates_AppendsIfAlreadyPresent()
    {
        StaticMapsUrlBuilder builder = new();

        builder.AddVisiblePortWithCoordinates((48.85, 2.35));
        builder.AddVisiblePortWithCoordinates((43.6, 1.43));

        string url = builder.AddSize(1, 1)
            .AddKey("key")
            .Build();

        Assert.Contains("48.85", url);
        Assert.Contains("1.43", url);
    }
    
    [Fact]
    public void AddVisiblePlaceWithCoordinates_DisabledEncoding_UsesRaw()
    {
        StaticMapsUrlBuilder builder = new();

        string url = builder
            .AddVisiblePortWithCoordinates((48.85, 2.35), (45.75, 4.85))
            .AddSize(1, 1)
            .AddKey("key")
            .WithOptions(DisableUrlEncodingOptions)
            .Build();

        Assert.Contains("48.85,2.35|45.75,4.85", url);
    }
    
    [Fact]
    public void AddMapStyle_AddsSingleStyleCorrectly()
    {
        MapStyle style = new(
            new StyleRule(Color: new HexColor("0xFF0000")),
            Features.Road.Local,
            Elements.Geometry.Fill
        );

        string url = new StaticMapsUrlBuilder()
            .AddMapStyle(style)
            .AddCenterWithLocation("Paris")
            .AddZoom(0)
            .AddSize(1, 1)
            .AddKey("key")
            .Build();

        string expected = Uri.EscapeDataString("feature:road.local|element:geometry.fill|color:0xFF0000");
        Assert.Contains($"style={expected}", url);
    }

    [Fact]
    public void AddMapStyle_AddsMultipleStyles()
    {
        MapStyle style1 = new(new StyleRule(Visibility: Visibility.Simplified), new Feature("road.local"));
        MapStyle style2 = new(new StyleRule(Color: new HexColor("0x00FF00")), null, new Element("geometry"));

        string url = new StaticMapsUrlBuilder()
            .AddMapStyle(style1, style2)
            .AddCenterWithLocation("Paris")
            .AddZoom(0)
            .AddSize(1, 1)
            .AddKey("key")
            .Build();

        Assert.Contains("style=", url);
        Assert.Contains("road.local", url);
        Assert.Contains("geometry", url);
    }
    
    [Fact]
    public void AddMapStyle_DisabledEncoding_UsesRaw()
    {
        MapStyle style = new(new StyleRule(Color: new HexColor("0x00FF00")));

        string url = new StaticMapsUrlBuilder()
            .AddMapStyle(style)
            .AddCenterWithLocation("Paris")
            .AddZoom(10)
            .AddSize(400, 400)
            .AddKey("key")
            .WithOptions(DisableUrlEncodingOptions)
            .Build();

        Assert.Contains("color:0x00FF00", url);
    }

    [Fact]
    public void UseHttp_SetsHttpSchemeInUrl()
    {
        string url = new StaticMapsUrlBuilder()
            .AddCenterWithLocation("Paris")
            .AddZoom(0)
            .AddSize(1, 1)
            .AddKey("key")
            .WithOptions(new StaticMapBuilderOptions
            {
                UseHttp = true
            })
            .Build();

        Assert.StartsWith(ProjectConstants.StaticMapBaseUrlHttp, url);
    }
    
    [Fact]
    public void DisableUrlEncoding_DisablesEncoding_WhenCalledBeforeParameters()
    {
        string url = new StaticMapsUrlBuilder()
            .AddCenterWithLocation("Champs Elysées")
            .AddZoom(0)
            .AddSize(1, 1)
            .AddKey("key")
            .WithOptions(DisableUrlEncodingOptions)
            .Build();

        Assert.Contains("Champs Elysées", url); 
    }
    
    [Fact]
    public void Build_WithNoParameters_Throws()
    {
        StaticMapsUrlBuilder builder = new();
        Assert.Throws<ArgumentException>(() => builder.Build());
    }
    
    [Fact]
    public void Build_WithoutCenterOrZoom_Throws()
    {
        StaticMapsUrlBuilder builder = new StaticMapsUrlBuilder()
            .AddSize(1, 1)
            .AddKey("key");

        Assert.Throws<InvalidOperationException>(() => builder.Build());
    }
    
    [Fact]
    public void Build_WithoutKey_Throws()
    {
        StaticMapsUrlBuilder builder = new StaticMapsUrlBuilder()
            .AddCenterWithLocation("Paris")
            .AddZoom(10)
            .AddSize(1, 1);

        Assert.Throws<ArgumentException>(() => builder.Build());
    }

    [Fact]
    public void Build_WithoutSize_Throws()
    {
        StaticMapsUrlBuilder builder = new StaticMapsUrlBuilder()
            .AddCenterWithLocation("Paris")
            .AddZoom(10)
            .AddKey("key");

        Assert.Throws<InvalidOperationException>(() => builder.Build());
    }
    
    [Fact]
    public void Build_WithTooManyMarkers_Throws()
    {
        StaticMapsUrlBuilder builder = new StaticMapsUrlBuilder()
            .AddSize(1, 1)
            .AddKey("key");

        for (int i = 0; i < ProjectConstants.LocationMarkersCountLimit + 1; i++)
        {
            builder.AddMarkers(new LocationMarker($"City{i}"));
        }

        Assert.Throws<InvalidOperationException>(() => builder.Build());
    }
    
    [Fact]
    public void Build_WithTooManyPathPoints_Throws()
    {
        StaticMapsUrlBuilder builder = new StaticMapsUrlBuilder()
            .AddSize(1, 1)
            .AddKey("key");
        
        Path[] paths = new Path[ProjectConstants.LocationPointsForPathCountLimit/2 + 1];

        for (int i = 0; i < ProjectConstants.LocationPointsForPathCountLimit/2 + 1; i++) 
        {
            Path path = new();
            path.AddPoint("A");
            path.AddPoint("B");
            paths[i] = path;
        }

        builder.AddPaths(paths);
        
        Assert.Throws<InvalidOperationException>(() => builder.Build());
    }

    [Fact]
    public void Build_WithTooManyVisiblePlaces_Throws()
    {
        StaticMapsUrlBuilder builder = new StaticMapsUrlBuilder()
            .AddSize(1, 1)
            .AddKey("key");

        for (int i = 0; i < ProjectConstants.LocationsCountLimit + 1; i++)
        {
            builder.AddViewportWithLocation($"City{i}");
        }

        Assert.Throws<InvalidOperationException>(() => builder.Build());
    }
    
    [Fact]
    public void Build_WithTooManyVisiblePlacesAndCenterAsLocation_Throws()
    {
        StaticMapsUrlBuilder builder = new StaticMapsUrlBuilder()
            .AddSize(1, 1)
            .AddCenterWithLocation("Paris")
            .AddKey("key");

        for (int i = 0; i < ProjectConstants.LocationsCountLimit; i++)
        {
            builder.AddViewportWithLocation($"City{i}");
        }

        Assert.Throws<InvalidOperationException>(() => builder.Build());
    }

    [Fact]
    public void Build_WithMapIdAndStyle_Throws()
    {
        MapStyle style = new(new StyleRule(Color: new HexColor("0xFF0000")));

        StaticMapsUrlBuilder builder = new StaticMapsUrlBuilder()
            .AddMapId("1234567890123456")
            .AddMapStyle(style)
            .AddSize(1, 1)
            .AddKey("key");

        Assert.Throws<InvalidOperationException>(() => builder.Build());
    }
    
    
    [Fact]
    public void Build_WithoutKeyDisabledChecking_Success()
    {
        string url = new StaticMapsUrlBuilder()
            .AddCenterWithLocation("Paris")
            .AddZoom(10)
            .AddSize(1, 1)
            .WithOptions(new StaticMapBuilderOptions
            {
                DisableApiKeyCheck = true
            })
            .Build();

        Assert.StartsWith("https://", url);
        Assert.Contains("center=Paris", url);
        Assert.Contains("zoom=10", url);
        Assert.Contains("size=1x1", url);
    }

    [Fact]
    public void Build_RelativeUrlOnly_DoesNotContainHost()
    {
        string url = new StaticMapsUrlBuilder()
            .AddCenterWithLocation("Paris")
            .AddZoom(10)
            .AddSize(1, 1)
            .AddKey("key")
            .WithOptions(new StaticMapBuilderOptions
            {
                ReturnParametersOnly = true
            })
            .Build();
        
        Assert.DoesNotContain(ProjectConstants.StaticMapBaseUrlHttps, url);
        Assert.DoesNotContain(ProjectConstants.StaticMapBaseUrlHttp, url);
        Assert.StartsWith("center", url);
        Assert.Contains("center=Paris", url);
        Assert.Contains("zoom=10", url);
        Assert.Contains("size=1x1", url);
        Assert.Contains("key=key", url);
    }
    
    [Fact]
    public void Build_WithValidParameters_ReturnsUrl()
    {
        string url = new StaticMapsUrlBuilder()
            .AddCenterWithLocation("Paris")
            .AddZoom(0)
            .AddSize(1, 1)
            .AddKey("key")
            .Build();

        Assert.StartsWith(ProjectConstants.StaticMapBaseUrlHttps, url);
        Assert.Contains("center=Paris", url);
    }
    
    [Fact]
    public void Build_WithTooLongUrl_Throws()
    {
        StaticMapsUrlBuilder builder = new StaticMapsUrlBuilder()
            .AddCenterWithLocation("Paris")
            .AddZoom(0)
            .AddSize(1, 1)
            .AddKey(new string('A', ProjectConstants.StaticMapsApiUrlMaxSize));

        Assert.Throws<InvalidOperationException>(() => builder.Build());
    }
    
    [Fact]
    public void Build_WithMinimalValidConfiguration_ReturnsCorrectUrl()
    {
        string url = new StaticMapsUrlBuilder()
            .AddCenterWithCoordinates(48.8566, 2.3522)
            .AddZoom(12)
            .AddSize(600, 400)
            .AddKey("test-api-key")
            .Build();

        Assert.StartsWith("https://", url);
        Assert.Contains("center=48.8566%2C2.3522", url);
        Assert.Contains("zoom=12", url);
        Assert.Contains("size=600x400", url);
        Assert.Contains("key=test-api-key", url);
    }

    
    [Fact]
    public void Build_WithAllMajorParameters_ReturnsFullUrl()
    {
        CoordinatesMarker marker = new(48.8584, 2.2945, label: 'E');
    
        MapStyle style = new MapStyle(
            new StyleRule(Color: new HexColor("0xFF0000"), Visibility: Visibility.Simplified),
            Features.Road.AllRoad,
            Elements.Geometry.AllGeometry
        );
    
        Path path = new Path(Color: new HexColor("0x00FF00"));
        path.AddPoint(48.85, 2.35);
        path.AddPoint(48.86, 2.36);

        string url = new StaticMapsUrlBuilder()
            .AddCenterWithCoordinates(48.8566, 2.3522)
            .AddZoom(13)
            .AddSize(640, 480)
            .AddMarkers(marker)
            .AddMapStyle(style)
            .AddPaths(path)
            .AddKey("test-api-key")
            .Build();

        Assert.Contains("markers=", url);
        Assert.Contains("style=", url);
        Assert.Contains("path=", url);
        Assert.Contains("center=48.8566%2C2.3522", url);
        Assert.Contains("key=test-api-key", url);
    }
}