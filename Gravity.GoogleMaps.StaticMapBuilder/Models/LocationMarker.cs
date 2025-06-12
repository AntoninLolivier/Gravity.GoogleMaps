namespace Gravity.GoogleMaps.StaticMapBuilder.Models;

/// <inheritdoc />
/// <remarks>
/// This class allows adding a styled marker using a human-readable place name.
/// It inherits all styling capabilities from <see cref="Marker"/>, including label, color, size, scale, anchor, and custom icons.
/// </remarks>
/// <param name="location">
/// The geocodable location string (e.g., city, address, place ID). Must be a valid input accepted by the Google Maps geocoder.
/// </param>
/// <param name="size">
/// Optional marker size (default is <see cref="MarkerSize.Default"/>).
/// </param>
/// <param name="color">
/// Optional marker color. Can be a predefined <see cref="StaticMapColor"/> or a custom <see cref="HexColor"/> (without alpha).
/// </param>
/// <param name="label">
/// Optional label (1 alphanumeric character). Cannot be used with <see cref="MarkerSize.Tiny"/> or <see cref="MarkerSize.Small"/>.
/// </param>
/// <param name="markerScale">
/// Optional scale factor. Use <see cref="MarkerScale.Two"/> for high-DPI (retina) markers.
/// </param>
/// <param name="anchor">
/// Optional anchor for custom icon positioning, either a <see cref="MarkerAnchor"/> value or pixel coordinates (x, y).
/// Only applicable when <paramref name="iconUrl"/> is set.
/// </param>
/// <param name="iconUrl">
/// Optional custom marker icon URL. Must be a fully qualified public URL.
/// </param>
public class LocationMarker(
    string location,
    MarkerSize size = MarkerSize.Default,
    OneOf<StaticMapColor, HexColor>? color = null,
    char? label = null,
    MarkerScale markerScale = MarkerScale.One,
    OneOf<MarkerAnchor, (int, int)>? anchor = null,
    string? iconUrl = null) : Marker(size, color, label, markerScale, anchor, iconUrl)
{
    /// <inheritdoc />
    public override string ToString()
    {
        string style = base.ToString();

        return $"{style}|{location}";
    }
}