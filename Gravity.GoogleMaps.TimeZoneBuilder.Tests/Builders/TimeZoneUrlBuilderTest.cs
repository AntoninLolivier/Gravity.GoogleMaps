using System;
using System.Globalization;
using JetBrains.Annotations;
using Xunit;

namespace Gravity.GoogleMaps.TimeZoneBuilder.Tests.Builders;

[TestSubject(typeof(TimeZoneUrlBuilder))]
public class TimeZoneUrlBuilderTest
{
    // Options
    
    private static readonly TimeZoneBuilderOptions DisableUrlEncodingOptions = new()
    {
        DisableUrlEncoding = true
    };
    
    // Tests
    
    [Fact]
    public void AddLocation_ValidLocation_ShouldAddParameter()
    {
        TimeZoneUrlBuilder builder = new();

        builder.AddLocation(42.057399,2.056392)
            .AddTimeStamp(DateTimeOffset.UtcNow)
            .AddKey("key");

        string result = builder.Build();
        Assert.Contains("location=42.057399%2C2.056392", result);
    }

    [Theory]
    [InlineData(95.1234, 2.3522)]
    [InlineData(48.8566, -210.4455)]
    [InlineData(-130.7788, 190.5566)]
    public void AddLocation_InvalidLocation_ShouldThrowArgumentException(double latitude, double longitude)
    {
        TimeZoneUrlBuilder builder = new();
        Assert.Throws<InvalidOperationException>(() => builder.AddLocation(latitude, longitude).Build());
    }

    [Fact]
    public void AddTimestamp_ValidTimestamp_ShouldAddParameter()
    {
        long timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        
        TimeZoneUrlBuilder builder = new();
        builder
            .AddTimeStamp(DateTimeOffset.FromUnixTimeSeconds(timestamp))
            .AddLocation(1, 1)
            .AddKey("key");

        string result = builder.Build();
        Assert.Contains($"timestamp={timestamp}", result);
    }

    [Theory]
    [InlineData("en")]
    [InlineData("fr")]
    public void AddLanguage_ValidLanguage_ShouldAddParameter(string language)
    {
        TimeZoneUrlBuilder builder = new();

        builder.AddLanguage(language)
            .AddLocation(0, 0)
            .AddTimeStamp(DateTimeOffset.UtcNow)
            .AddKey("key");

        string result = builder.Build();
        Assert.Contains($"language={language}", result);
    }

    [Fact]
    public void AddLanguage_InvalidLanguage_ShouldThrowArgumentException()
    {
        TimeZoneUrlBuilder builder = new();
        Assert.Throws<InvalidOperationException>(() => builder.AddLanguage("english").Build());
    }
    
    [Fact]
    public void Build_WithNoParameters_Throws()
    {
        TimeZoneUrlBuilder builder = new();
        Assert.Throws<InvalidOperationException>(() => builder.Build());
    }
    
    [Fact]
    public void Build_WithoutKey_Throws()
    {
        TimeZoneUrlBuilder builder = new TimeZoneUrlBuilder()
            .AddTimeStamp(DateTimeOffset.UtcNow)
            .AddLocation(0, 0);

        Assert.Throws<InvalidOperationException>(() => builder.Build());
    }

    [Fact]
    public void Build_WithoutKeyDisabledChecking_Success()
    {
        long timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        string url = new TimeZoneUrlBuilder()
            .AddTimeStamp(DateTimeOffset.FromUnixTimeSeconds(timestamp))
            .AddLocation(0, 0)
            .WithOptions(new TimeZoneBuilderOptions
            {
                DisableApiKeyCheck = true
            })
            .Build();

        Assert.StartsWith("https://", url);
        Assert.Contains($"timestamp={timestamp}", url);
        Assert.Contains("location=0%2C0", url);
    }

    [Fact]
    public void Build_RelativeUrlOnly_DoesNotContainHost()
    {
        long timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        string url = new TimeZoneUrlBuilder()
            .AddTimeStamp(DateTimeOffset.FromUnixTimeSeconds(timestamp))
            .AddLocation(0, 0)
            .AddKey("key")
            .WithOptions(new TimeZoneBuilderOptions
            {
                ReturnParametersOnly = true
            })
            .Build();
        
        Assert.DoesNotContain(ProjectConstants.TimeZoneBaseUrl, url);
        Assert.Contains($"timestamp={timestamp}", url);
        Assert.Contains("location=0%2C0", url);
        Assert.Contains("key=key", url);
    }
    
    [Fact]
    public void Build_WithValidParameters_ReturnsUrl()
    {
        long timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        string url = new TimeZoneUrlBuilder()
            .AddTimeStamp(DateTimeOffset.FromUnixTimeSeconds(timestamp))
            .AddLocation(0, 0)
            .AddKey("key")
            .Build();

        Assert.StartsWith(ProjectConstants.TimeZoneBaseUrl, url);
        Assert.Contains($"timestamp={timestamp}", url);
    }

    [Fact]
    public void Build_WithAllValidParameters_ReturnsUrl()
    {
        long timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        double latitude = 47.3943556;
        double longitude = 0.6123703;
        string language = "fr";

        string url = new TimeZoneUrlBuilder()
                .AddLocation(latitude, longitude)
                .AddTimeStamp(DateTimeOffset.FromUnixTimeSeconds(timestamp))
                .AddLanguage(language)
                .AddKey("key")
                .Build();
        
        Assert.StartsWith(ProjectConstants.TimeZoneBaseUrl + '?', url);
        Assert.Contains($"timestamp={timestamp}", url);
        Assert.Contains($"location={latitude.ToString(CultureInfo.InvariantCulture)}%2C{longitude.ToString(CultureInfo.InvariantCulture)}", url);
        Assert.Contains($"language={language}", url);
        Assert.Contains("key=key", url);
    }
}