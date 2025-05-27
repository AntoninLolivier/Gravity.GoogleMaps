using System.Diagnostics;
using System.Globalization;
using Gravity.GoogleMaps.StaticMapBuilder.Options;
using Path = Gravity.GoogleMaps.StaticMapBuilder.Models.Path;

namespace Gravity.GoogleMaps.StaticMapBuilder.Builders;

/// <summary>
/// The main class used to build the static maps url.
/// </summary>
/// <example>
/// <code>
/// string url = new StaticMapsUrlBuilder()
///     .AddCenterWithLocation("New York, NY")
///     .AddZoom(12)
///     .AddSize(640, 480)
///     .AddScale(MapScale.One)
///     .AddFormat(MapFormats.Jpg)
///     .Build();
/// </code>
/// </example>
public class StaticMapsUrlBuilder
{
    // Exception messages

    private const string WrongMethodForMarkerGroupExceptionMessage = $"This method should not be used for {nameof(MarkerGroup)} objects. Use {nameof(AddMarkerGroups)} instead.";
    private const string DisableEncodingBeforeAddingParametersExceptionMessage = $"Use the {nameof(DisableUrlEncoding)} method after adding any paramters to the query.";
    
    // Fields

    private readonly Dictionary<StaticMapRequestParameters, string?> _requestParameters = new();
    private readonly List<string> _markerIconUrls = [];
    private int _locationsCount;
    private int _locationMarkersCount;
    private int _locationPointsForPathsCount;
    private bool _useHttp;
    private bool _encodeToUrl = true;
    private bool _isCenterOrZoomMandatory = true;
    
    // Methods

    /// <summary>
    /// Add the center parameter to the query using a location string.
    /// </summary>
    /// <remarks>
    /// See <see href="https://developers.google.com/maps/documentation/maps-static/start?#Locations">official documentation</see> for details.
    /// </remarks>
    /// <example>
    /// The location string can be any location human readable string, like "1 Rue de la Paix, Paris, France"
    /// </example>
    /// <param name="location">The location string.</param>
    /// <returns>The builder.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the <paramref name="location"/> is null, empty or white space</exception>
    public StaticMapsUrlBuilder AddCenterWithLocation(string location)
    {
        if (string.IsNullOrEmpty(location) || string.IsNullOrWhiteSpace(location)) throw new ArgumentNullException(nameof(location), "Location cannot be null or empty");

        CheckIfParameterIsAlreadyAdded(StaticMapRequestParameters.Center);

        location = _encodeToUrl ? Uri.EscapeDataString(location) : location;
        
        _requestParameters.Add(StaticMapRequestParameters.Center, $"center={location}");
        
        _locationsCount++;
        _isCenterOrZoomMandatory = false;
        
        return this;
    }

    /// <summary>
    /// Add the center parameter to the query using a latitude and longitude.
    /// </summary>
    /// <remarks>
    /// See <see href="https://developers.google.com/maps/documentation/maps-static/start?#Locations">official documentation</see> for details.
    /// </remarks>
    /// <param name="latitude">The latitude of the center.</param>
    /// <param name="longitude">The longitude of the center</param>
    /// <returns>The builder</returns>
    public StaticMapsUrlBuilder AddCenterWithCoordinates(double latitude, double longitude)
    {
        ArgumentNullException.ThrowIfNull(latitude);
        ArgumentNullException.ThrowIfNull(longitude);
        
        CheckIfParameterIsAlreadyAdded(StaticMapRequestParameters.Center);
        
        string coordinates = $"{latitude},{longitude}";
        
        coordinates = _encodeToUrl ? Uri.EscapeDataString(coordinates) : coordinates;
        
        _requestParameters.Add(StaticMapRequestParameters.Center, $"center={coordinates}");
        
        _isCenterOrZoomMandatory = false;
        
        return this;
    }

    /// <summary>
    /// Add the zoom parameter to the query.
    /// </summary>
    /// <remarks>
    /// See <see href="https://developers.google.com/maps/documentation/maps-static/start#Zoomlevels">official documentation</see> for details.
    /// </remarks>
    /// <param name="zoom">An integer between 0 and 22.</param>
    /// <returns>The builder.</returns>
    public StaticMapsUrlBuilder AddZoom(int zoom)
    {
        ArgumentNullException.ThrowIfNull(zoom);
        
        CheckIfParameterIsAlreadyAdded(StaticMapRequestParameters.Zoom);

        if (zoom is < 0 or > 22)
        {
            Debug.WriteLine($"Warning: Warning The zoom parameter must be between 0 and 22. The resulting static map will " +
                            $"use either zoom 0 if their is no markers or a scale-based zoom if there are markers." +
                            $" See : {ProjectConstants.StaticMapDocumentationLinks.SectionLinks.ZoomLevels} for more information.");
        }
        
        string zoomString = zoom.ToString();
        zoomString = _encodeToUrl ? Uri.EscapeDataString(zoomString) : zoomString; 
        
        _requestParameters.Add(StaticMapRequestParameters.Zoom, $"zoom={zoomString}");
        
        _isCenterOrZoomMandatory = false;
        
        return this;
    }
    
    /// <summary>
    /// Add the size parameter to the query.
    /// </summary>
    /// <remarks>
    /// See <see href="https://developers.google.com/maps/documentation/maps-static/start#Imagesizes">official documentation</see> for details.
    /// </remarks>
    /// <param name="width">The width of the resulting image.</param>
    /// <param name="height">The height of the resulting image.</param>
    /// <returns>The builder.</returns>
    /// <exception cref="ArgumentException">Thrown when the width or height is null or less or equal than 0.</exception>
    public StaticMapsUrlBuilder AddSize(int width, int height)
    {
        ArgumentNullException.ThrowIfNull(width);
        ArgumentNullException.ThrowIfNull(height);
        
        if (width <= 0) throw new ArgumentException("Width shoul not be less than or equal to 0");
        if (height <= 0) throw new ArgumentException("Height shoul not be less than or equal to 0");
        
        CheckIfParameterIsAlreadyAdded(StaticMapRequestParameters.Size);
        
        string size = $"{width}x{height}";
        
        size = _encodeToUrl ? Uri.EscapeDataString(size) : size;
        
        _requestParameters.Add(StaticMapRequestParameters.Size, $"size={size}");
        return this;
    }
    
    /// <summary>
    /// Add the scale parameter to the query.
    /// </summary>
    /// <remarks>
    /// See <see href="https://developers.google.com/maps/documentation/maps-static/start#scale_values">official documentation</see> for details.
    /// </remarks>
    /// <param name="mapScale">The scale as an enum value.</param>
    /// <returns>The builder.</returns>
    public StaticMapsUrlBuilder AddScale(MapScale mapScale)
    {
        ArgumentNullException.ThrowIfNull(mapScale);
        
        CheckIfParameterIsAlreadyAdded(StaticMapRequestParameters.Scale);

        int mapScaleInt = (int)mapScale;
        
        _requestParameters.Add(StaticMapRequestParameters.Scale, $"scale={mapScaleInt}");
        return this;
    }

    /// <summary>
    /// Add the foormat parameter to the query.
    /// </summary>
    /// <example>
    /// Use the <see cref="MapFormats"/> class to specify the format.
    /// </example>
    /// <remarks>
    /// See <see href="https://developers.google.com/maps/documentation/maps-static/start#ImageFormats">official documentation</see> for details.
    /// </remarks>
    /// <param name="format"></param>
    /// <returns>The builder</returns>
    public StaticMapsUrlBuilder AddFormat(MapFormat format)
    {
        ArgumentNullException.ThrowIfNull(format);
        
        CheckIfParameterIsAlreadyAdded(StaticMapRequestParameters.Format);
        
        string formatString = format.ToString().ToLower();
        formatString = _encodeToUrl ? Uri.EscapeDataString(formatString) : formatString;
        
        _requestParameters.Add(StaticMapRequestParameters.Format, $"format={formatString}");
        return this;
    }
    
    /// <summary>
    /// Add the maptype parameter to the query.
    /// </summary>
    /// <remarks>
    /// See <see href="https://developers.google.com/maps/documentation/maps-static/start?#MapTypes">official documentation</see> for details.
    /// </remarks>
    /// <param name="mapType">The map type as an enum value.</param>
    /// <returns>The builder.</returns>
    public StaticMapsUrlBuilder AddMapType(StaticMapType mapType)
    {
        ArgumentNullException.ThrowIfNull(mapType);
        
        CheckIfParameterIsAlreadyAdded(StaticMapRequestParameters.MapType);
        
        string mapTypeString = mapType.ToString().ToLower();
        mapTypeString = _encodeToUrl ? Uri.EscapeDataString(mapTypeString) : mapTypeString;
        
        _requestParameters.Add(StaticMapRequestParameters.MapType, $"maptype={mapTypeString}");
        return this;
    }

    /// <summary>
    /// Add the language parameter to the query.
    /// </summary>
    /// <param name="language">The language as a 2 letter string.</param>
    /// <remarks>
    /// See <see href="https://developers.google.com/maps/documentation/maps-static/start#map-parameters">official documentation</see> for details.
    /// </remarks>
    /// <returns>The builder.</returns>
    /// <exception cref="ArgumentException">Thrown when the <paramref name="language"/> is more than 2 charaters long.</exception>
    public StaticMapsUrlBuilder AddLanguage(string language)
    {
        ArgumentNullException.ThrowIfNull(language);
        
        CheckIfParameterIsAlreadyAdded(StaticMapRequestParameters.Language);

        if (language.Length > 2) throw new ArgumentException(ExceptionMessages.MalformedParametersExceptionMessages.LanguageMustBeInTwoLettersExceptionMessage);
        
        language = _encodeToUrl ? Uri.EscapeDataString(language) : language;
        
        _requestParameters.Add(StaticMapRequestParameters.Language, $"language={language}");
        return this;
    }

    /// <summary>
    /// Add the region parameter to the query.
    /// </summary>
    /// <remarks>
    /// See <see href="https://developers.google.com/maps/coverage">official documentation</see> for details.
    /// </remarks>
    /// <param name="region">The region as a 2 letter string. See available regions on <see href="https://developers.google.com/maps/coverage">google documentation</see></param>
    /// <returns>The builder</returns>
    /// <exception cref="ArgumentException">Thrown when the <paramref name="region"/> is more than 2 charaters long.</exception>
    public StaticMapsUrlBuilder AddRegion(string region)
    {
        ArgumentNullException.ThrowIfNull(region);
        
        CheckIfParameterIsAlreadyAdded(StaticMapRequestParameters.Region);
        
        if (region.Length > 2) throw new ArgumentException(ExceptionMessages.MalformedParametersExceptionMessages.RegionMustBeInTwoLettersExceptionMessage);
        
        region = _encodeToUrl ? Uri.EscapeDataString(region) : region;
        
        _requestParameters.Add(StaticMapRequestParameters.Region, $"region={region}");
        return this;
    }

    /// <summary>
    /// Add the mapid parameter to the query.
    /// </summary>
    /// <remarks>
    /// See <see href="https://developers.google.com/maps/documentation/javascript/map-ids/mapid-over">official documentation</see> for details.
    /// </remarks>
    /// <param name="mapId">The 16 characters long map id.</param>
    /// <returns>The builder.</returns>
    /// <exception cref="ArgumentException">Thrown if the <paramref name="mapId"/> is more than 16 character.</exception>
    public StaticMapsUrlBuilder AddMapId(string mapId)
    {
        ArgumentNullException.ThrowIfNull(mapId);
        
        CheckIfParameterIsAlreadyAdded(StaticMapRequestParameters.MapId);
        
        if (mapId.Length > ProjectConstants.MapIdMaxLength) throw new ArgumentException(ExceptionMessages.MalformedParametersExceptionMessages.MapIdMustBeSixteenCharactersMax);
        
        mapId = _encodeToUrl ? Uri.EscapeDataString(mapId) : mapId;
        
        _requestParameters.Add(StaticMapRequestParameters.MapId, $"map_id={mapId}");
        return this;
    }

    /// <summary>
    /// Add markers with the same style to the query.
    /// </summary>
    /// <remarks>
    /// Use the <see cref="AddMarkers"/> method to add markers with unique style.
    /// <br/>
    /// This method allow to add multiple <see cref="MarkerGroup"/>.
    /// <br/>
    /// See <see href="https://developers.google.com/maps/documentation/maps-static/start?#Markers">official documentation</see> for details.
    /// </remarks>
    /// <example>
    /// If you want to add multiple <see cref="MarkerGroup"/> do not call the method multiples times. Do this instead :
    /// <code>
    /// var _ = new StaticMapsUrlBuilder().AddMarkerGroups(markerGroup1, markerGroup2, markerGroup3);
    /// </code>
    /// </example>
    /// <param name="markerGroup">The groups of markers to add.</param>
    /// <returns>The builder</returns>
    public StaticMapsUrlBuilder AddMarkerGroups(params MarkerGroup[] markerGroup)
    {
        ArgumentNullException.ThrowIfNull(markerGroup);
        
        List<string> markerGroupsStrings = [];
        markerGroupsStrings.AddRange(markerGroup.Select(mg =>
        {
            string markerGroupString = _encodeToUrl ? Uri.EscapeDataString(mg.ToString()) : mg.ToString();
            return $"markers={markerGroupString}";
        }));
        string markers = string.Join("&", markerGroupsStrings);
        
        if (_requestParameters.TryGetValue(StaticMapRequestParameters.Marker, out string? existingMarkers))
        {
            _requestParameters[StaticMapRequestParameters.Marker] = existingMarkers + "&" + markers;
        }
        else
        {
            _requestParameters.Add(StaticMapRequestParameters.Marker, markers);
        }
        
        _locationMarkersCount += markerGroup.Sum(mg => mg.LocationCount);
        
        List<string> icons = markerGroup
            .Where(mg => mg.IconUrl is not null)
            .Select(mg =>
            {
                if (mg.IconUrl is null) throw new NullReferenceException();
                
                return mg.IconUrl;
            })
            .ToList();

        if (icons.Count > 0) _markerIconUrls.AddRange(icons);

        _isCenterOrZoomMandatory = false;
        
        return this;
    }

    /// <summary>
    /// Add markers with unique styles.
    /// </summary>
    /// <remarks>
    /// Use the <see cref="AddMarkerGroups"/> method to add multiple markers with the same style.
    /// <br/>
    /// This method allow to add multiple <see cref="Marker"/>. 
    /// <br/>
    /// See <see href="https://developers.google.com/maps/documentation/maps-static/start?#Markers">official documentation</see> for details.
    /// </remarks>
    /// <example>
    /// If you want to add multiple <see cref="Marker"/> do not call the method multiples times. Do this instead :
    /// <code>
    /// var _ = new StaticMapsUrlBuilder().AddMarkers(marker1, marker2, marker3);
    /// </code>
    /// </example>
    /// <param name="markers">The markers to add.</param>
    /// <returns>The builder</returns>
    /// <exception cref="InvalidOperationException">Thrown when one or more of the given <paramref name="markers"/> is a <see cref="MarkerGroup"/></exception>
    public StaticMapsUrlBuilder AddMarkers(params Marker[] markers)
    {
        ArgumentNullException.ThrowIfNull(markers);

        if (markers.Any(marker => marker is MarkerGroup)) throw new InvalidOperationException(WrongMethodForMarkerGroupExceptionMessage);

        List<string> markerStrings = [];
        markerStrings.AddRange(markers.Select(marker =>
        {
            string markerString = _encodeToUrl ? Uri.EscapeDataString(marker.ToString()) :  marker.ToString();
            return $"markers={markerString}";
        }));
        string markersString = string.Join("&", markerStrings);

        if (_requestParameters.TryGetValue(StaticMapRequestParameters.Marker, out string? existingMarkers))
        {
            _requestParameters[StaticMapRequestParameters.Marker] = existingMarkers + "&" + markersString;
        }
        else
        {
            _requestParameters.Add(StaticMapRequestParameters.Marker, string.Join("&", markerStrings));
        }
        
        _locationMarkersCount += markers.Count(marker => marker is LocationMarker);
        
        List<string> icons = markers
            .Where(mg => mg.IconUrl is not null)
            .Select(mg =>
            {
                if (mg.IconUrl is null) throw new NullReferenceException();
                
                return mg.IconUrl;
            })
            .ToList();

        if (icons.Count > 0) _markerIconUrls.AddRange(icons);

        _isCenterOrZoomMandatory = false;
        
        return this;
    }

    /// <summary>
    /// Add paths to the query.
    /// </summary>
    /// <remarks>
    /// This method allow to add multiple <see cref="Path"/>.
    /// <br/>
    /// See <see href="https://developers.google.com/maps/documentation/maps-static/start#Paths">official documentation</see> for details.
    /// </remarks>
    /// <example>
    /// If you want to add multiple <see cref="Path"/> do not call the method multiples times. Do this instead :
    /// <code>
    /// var _ = new StaticMapsUrlBuilder().AddPaths(path1, path2, path3);
    /// </code>
    /// </example>
    /// <param name="paths">The paths to add.</param>
    /// <returns>The builder.</returns>
    public StaticMapsUrlBuilder AddPaths(params Path[] paths)
    {
        ArgumentNullException.ThrowIfNull(paths);
        
        List<string> pathsStrings = [];
        pathsStrings.AddRange(paths.Select(path =>
        {
            string pathString = _encodeToUrl ? Uri.EscapeDataString(path.ToString()) :  path.ToString();
            return $"path={pathString}";
        }));
        
        _requestParameters.Add(StaticMapRequestParameters.Path, string.Join("&", pathsStrings));

        foreach (Path path in paths)
        {
            _locationPointsForPathsCount += path.LocationCount;
        }

        _isCenterOrZoomMandatory = false;
        
        return this;
    }
    
    /// <summary>
    /// Add viewports to the map
    /// </summary>
    /// <remarks>
    /// This method allow to add multiple viewports with locations strings.
    /// <br/>
    /// See <see href="https://developers.google.com/maps/documentation/maps-static/start#Viewports">official documentation</see> for details.
    /// </remarks>
    /// <example>
    /// If you want to add multiple view ports do not call the method multiples times. Do this instead :
    /// <code>
    /// var _ = new StaticMapsUrlBuilder().AddViewportWithLocation(location1, location2, location3);
    /// </code>
    /// </example>
    /// <param name="location">The location of the viewport as a human readable location.</param>
    /// <returns>The builder.</returns>
    public StaticMapsUrlBuilder AddViewportWithLocation(params string[] location)
    {
        ArgumentNullException.ThrowIfNull(location);
        
        _locationsCount += location.Length;
        string locations = string.Join("|", location);
        
        locations = _encodeToUrl ? Uri.EscapeDataString(locations) : locations;

        if (_requestParameters.TryGetValue(StaticMapRequestParameters.Visible, out string? value))
        {
            _requestParameters[StaticMapRequestParameters.Visible] = value + Uri.EscapeDataString("|") + locations;
        }
        else
        {
            _requestParameters.Add(StaticMapRequestParameters.Visible, $"visible={locations}");
        }

        _isCenterOrZoomMandatory = false;
        
        return this;
    }

    /// <summary>
    /// Add viewports to the map
    /// </summary>
    /// <remarks>
    /// This method allow to add multiple viewports with coordinates.
    /// <br/>
    /// See <see href="https://developers.google.com/maps/documentation/maps-static/start#Viewports">official documentation</see> for details.
    /// </remarks>
    /// <example>
    /// If you want to add multiple viewports do not call the method multiples times. Do this instead :
    /// <code>
    /// var _ = new StaticMapsUrlBuilder().AddVisibleportWithCoordinates(coordinates1, coordinates2, coordinates3);
    /// </code>
    /// </example>
    /// <param name="coordinates">The coordinates of the viewport.</param>
    /// <returns>The builder.</returns>
    public StaticMapsUrlBuilder AddVisibleportWithCoordinates(params (double, double)[] coordinates)
    {
        ArgumentNullException.ThrowIfNull(coordinates);
        
        string locations = string.Join("|", coordinates.Select(coordinate => 
            $"{coordinate.Item1.ToString(CultureInfo.InvariantCulture)},{coordinate.Item2.ToString(CultureInfo.InvariantCulture)}"));
        
        locations = _encodeToUrl ? Uri.EscapeDataString(locations) : locations;
        
        if (_requestParameters.TryGetValue(StaticMapRequestParameters.Visible, out string? value))
        {
            _requestParameters[StaticMapRequestParameters.Visible] = value + Uri.EscapeDataString("|") + locations;
        }
        else
        {
            _requestParameters.Add(StaticMapRequestParameters.Visible, $"visible={locations}");
        }

        _isCenterOrZoomMandatory = false;
        
        return this;
    }

    /// <summary>
    /// Add map styles to the query.
    /// </summary>
    /// <remarks>
    /// This method allow to add multiple viewports with coordinates.
    /// <br/>
    /// See <see href="https://developers.google.com/maps/documentation/maps-static/styling">official documentation</see> for details.
    /// </remarks>
    /// <example>
    /// If you want to add multiple <see cref="MapStyle"/> do not call the method multiples times. Do this instead :
    /// <code>
    /// var _ = new StaticMapsUrlBuilder().AddMapStyle(mapStyle1, mapStyle2, mapStyle3);
    /// </code>
    /// </example>
    /// <param name="mapStyles">The map style.</param>
    /// <returns>The builder.</returns>
    public StaticMapsUrlBuilder AddMapStyle(params MapStyle[] mapStyles)
    {
        ArgumentNullException.ThrowIfNull(mapStyles);
        
        List<string> stylesStrings = mapStyles.Select(style =>
        {
            string styleString = _encodeToUrl ? Uri.EscapeDataString(style.ToString()) :  style.ToString();
            return $"style={styleString}";
        }).ToList();

        _requestParameters.Add(StaticMapRequestParameters.Style, string.Join("&", stylesStrings));
        
        return this;
    }
    
    /// <summary>
    /// Add the API key to the query.
    /// </summary>
    /// <remarks>
    /// See <see href="https://developers.google.com/maps/documentation/maps-static/get-api-key">official documentation</see> for details.
    /// </remarks>
    /// <param name="key">You API Key.</param>
    /// <returns>The builder.</returns>
    public StaticMapsUrlBuilder AddKey(string key)
    {
        if (string.IsNullOrEmpty(key) || string.IsNullOrWhiteSpace(key)) throw new ArgumentNullException(nameof(key), "Key cannot be null or empty");
        
        _requestParameters.Add(StaticMapRequestParameters.Key, $"key={key}");
        return this;
    }

    /// <summary>
    /// Use the HTTP protocol instead of HTTPS.
    /// </summary>
    /// <returns>The builder.</returns>
    public StaticMapsUrlBuilder UseHttp()
    {
        _useHttp = true;
        return this;
    }

    /// <summary>
    /// Disable the url encoding. The url is, by default, url encoded.
    /// </summary>
    /// <remarks>
    /// This method should be used before adding any parameters to the builder.
    /// <br/>
    /// See <see href="https://developers.google.com/maps/url-encoding">official documentation</see> for details.
    /// </remarks>
    /// <returns>The builder</returns>
    /// <exception cref="InvalidOperationException">Thrown when a parameter was added in the query before calling this method.</exception>
    public StaticMapsUrlBuilder DisableUrlEncoding()
    {
        if (_requestParameters.Count > 0) throw new InvalidOperationException(DisableEncodingBeforeAddingParametersExceptionMessage);
        _encodeToUrl = false;
        return this;
    }

    /// <summary>
    /// Build the static map url.
    /// </summary>
    /// <remarks>
    /// This method validate the parameters and build the url.
    /// <br/>
    /// The url is, by default, url encoded. Use <see cref="DisableUrlEncoding"/> to disable the url encoding.
    /// <br/>
    /// The url is, by default, built using the HTTPS protocol. Use <see cref="UseHttp"/> to use the HTTP protocol.
    /// </remarks>
    /// <returns></returns>
    /// <exception cref="ArgumentException">Thrown when the API Key is missing.</exception>
    /// <exception cref="ArgumentException">Thrown when no parameter was added to the builder.</exception>
    /// <exception cref="InvalidOperationException">Thrown when the url exceed the maximum lenght.</exception>
    /// <exception cref="InvalidOperationException">Thrown when a "positionning argument" is missing.</exception>
    /// <exception cref="InvalidOperationException">Thrown when the size parameter is missing.</exception>
    /// <exception cref="InvalidOperationException">Thrown when the url exceed the maximum lenght.</exception>
    /// <exception cref="InvalidOperationException">Thrown when the number of marker with a location path exceed the limit. (15)</exception>
    /// <exception cref="InvalidOperationException">Thrown when the number of path with a location path exceed the limit. (15 total)</exception>
    /// <exception cref="InvalidOperationException">Thrown when the map id parameter is combined with a map style parameter.</exception>
    /// <exception cref="InvalidOperationException">Thrown when the number of location path exceed the limit (excluding paths and markers, it's 3)</exception>
    public string Build()
    {
        ValidateParameters();
        if (!_requestParameters.ContainsKey(StaticMapRequestParameters.Key)) throw new ArgumentException(ExceptionMessages.ParametersMissingExceptionMessages.ApiKeyParameterMissingMessage);
        
        string url = _useHttp ? ProjectConstants.StaticMapBaseUrlHttp + "?" : ProjectConstants.StaticMapBaseUrlHttps + "?";
        url += string.Join("&", _requestParameters.Select(requestParameter => requestParameter.Value));
        
        if (url.Length > ProjectConstants.StaticMapsApiUrlMaxSize)
        {
            throw new InvalidOperationException(ExceptionMessages.StaticMapUrlExceeededLengthExceptionMessage);
        }
        
        return url;
    }

    private void ValidateParameters()
    {
        if (_requestParameters.Count == 0) throw new ArgumentException(ExceptionMessages.NoParametersAddedExceptionMessage);
        
        ValidateCenterAndZoom();
        ValidateSize();
        ValidateMarkers();
        ValidatePaths();
        ValidateViewports();
        ValidateMapStyle();
    }

    private void ValidateCenterAndZoom()
    {
        if (_isCenterOrZoomMandatory && !_requestParameters.ContainsKey(StaticMapRequestParameters.Center) && !_requestParameters.ContainsKey(StaticMapRequestParameters.Zoom))
        {
            throw new InvalidOperationException(ExceptionMessages.ParametersMissingExceptionMessages.CenterParameterMissingMessage);
        }
    }
    
    private void ValidateSize()
    {
        if (!_requestParameters.ContainsKey(StaticMapRequestParameters.Size)) throw new InvalidOperationException(ExceptionMessages.ParametersMissingExceptionMessages.SizeParameterMissingMessage);
    }

    private void ValidateMarkers()
    {
        if (_locationMarkersCount > ProjectConstants.LocationMarkersCountLimit)
        {
            throw new InvalidOperationException(ExceptionMessages.UrlParametersExceptionMessages.TooMuchLocationMarkersExceptionMessage);
        }
    }

    private void ValidatePaths()
    {
        if (_locationPointsForPathsCount > ProjectConstants.LocationPointsForPathCountLimit)
        {
            throw new InvalidOperationException(ExceptionMessages.UrlParametersExceptionMessages.TooMuchLocationPointsForPathsExceptionMessage);
        }
    }

    private void ValidateMapStyle()
    {
        if (_requestParameters.ContainsKey(StaticMapRequestParameters.MapId)
            && _requestParameters.ContainsKey(StaticMapRequestParameters.Style))
        {
            throw new InvalidOperationException(ExceptionMessages.UrlParametersExceptionMessages.CombineMapIdAndMapStyleExceptionMessage);
        }
    }

    private void ValidateViewports()
    {
        if (_locationsCount > ProjectConstants.LocationsCountLimit)
        {
            throw new InvalidOperationException(ExceptionMessages.UrlParametersExceptionMessages.TooMuchLocationsExceptionMessage);       
        }
    }

    private void CheckIfParameterIsAlreadyAdded(StaticMapRequestParameters parameter)
    {
        if (_requestParameters.ContainsKey(parameter))
        {
            throw new ArgumentException(string.Format(ExceptionMessages.ParameterCanOnlyBeAddedOnceExceptionMessage, parameter.ToString()));
        }
    }
}
