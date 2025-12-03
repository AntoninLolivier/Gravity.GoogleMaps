using System.Globalization;

namespace Gravity.GoogleMaps.StaticMapBuilder.Parameters;

/// <summary>
/// The center parameter of the Static Map query parameter
/// </summary>
public class CenterQueryParameter : IQueryParameter
{
    // Properties

    /// <inheritdoc />
    public string Key => "center";

    /// <inheritdoc />
    public bool IsUnique => true;

    /// <inheritdoc />
    public ParameterValue Value { get; }
    
    // Constructors

    /// <summary>
    /// Initialize a new instance of the <see cref="CenterQueryParameter"/> with a human-readable location.
    /// </summary>
    /// <param name="locationCenter">The center as a location string (e.i. Paris)</param>
    public CenterQueryParameter(string locationCenter)
    {
        Value = new SimpleParameterValue(locationCenter);
    }
    
    /// <summary>
    /// Initialize a new instance of the <see cref="CenterQueryParameter"/> with coordinates.
    /// </summary>
    /// <param name="latitude">The center latitude.</param>
    /// <param name="longitude">The center longitude.</param>
    public CenterQueryParameter(double latitude, double longitude)
    {
        Value = new SimpleParameterValue($"{latitude.ToString(CultureInfo.InvariantCulture)},{longitude.ToString(CultureInfo.InvariantCulture)}");
    }
}