using System.Diagnostics;
using System.Globalization;
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
public class StaticMapsUrlBuilder // TODO Rename to StaticMapUrlBuilder (without s)
{
    // Exception messages

    private const string WrongMethodForMarkerGroupExceptionMessage = $"This method should not be used for {nameof(MarkerGroup)} objects. Use {nameof(AddMarkerGroups)} instead.";
    
    // Fields

    private readonly Dictionary<StaticMapRequestParameters, List<string>> _requestParameters = new();
    private readonly List<string> _markerIconUrls = [];
    private int _locationsCount;
    private int _locationMarkersCount;
    private int _locationPointsForPathsCount;
    private bool _isCenterOrZoomMandatory = true;
    private StaticMapBuilderOptions _options = new();

    // Methods

    /// <summary>
    /// Add the center parameter to the query using a location string.
    /// </summary>
    /// <remarks>
    /// See <see href="https://developers.google.com/maps/documentation/maps-static/start?#Locations">official documentation</see> for details.
    /// </remarks>
    /// <example>
    /// The location string can be any location human-readable string, like "1 Rue de la Paix, Paris, France"
    /// </example>
    /// <param name="location">The location string.</param>
    /// <returns>The builder.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the <paramref name="location"/> is null, empty or white space</exception>
    public StaticMapsUrlBuilder AddCenterWithLocation(string location)
    {
        if (string.IsNullOrEmpty(location) || string.IsNullOrWhiteSpace(location)) throw new ArgumentNullException(nameof(location), "Location cannot be null or empty");

        CheckIfParameterIsAlreadyAdded(StaticMapRequestParameters.Center);

        _requestParameters.Add(StaticMapRequestParameters.Center, [location]);
        
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
        // TODO : Manage the "Precision beyond the 6 decimal places is ignored"
        CheckIfParameterIsAlreadyAdded(StaticMapRequestParameters.Center);
        
        string coordinates = $"{latitude.ToString(CultureInfo.InvariantCulture)},{longitude.ToString(CultureInfo.InvariantCulture)}";
        
        _requestParameters.Add(StaticMapRequestParameters.Center, [coordinates]);
        
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
        CheckIfParameterIsAlreadyAdded(StaticMapRequestParameters.Zoom);

        if (zoom is < 0 or > 22)
        {
            Debug.WriteLine($"Warning: Warning The zoom parameter must be between 0 and 22. The resulting static map will " +
                            $"use either zoom 0 if their is no markers or a scale-based zoom if there are markers." +
                            $" See : {ProjectConstants.StaticMapDocumentationLinks.SectionLinks.ZoomLevels} for more information.");
        }
        
        string zoomString = zoom.ToString();
        
        _requestParameters.Add(StaticMapRequestParameters.Zoom, [zoomString]);
        
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
        if (width <= 0) throw new ArgumentException("Width should not be less than or equal to 0");
        if (height <= 0) throw new ArgumentException("Height should not be less than or equal to 0");
        
        CheckIfParameterIsAlreadyAdded(StaticMapRequestParameters.Size);
        
        string size = $"{width}x{height}";
        
        _requestParameters.Add(StaticMapRequestParameters.Size, [size]);
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
        CheckIfParameterIsAlreadyAdded(StaticMapRequestParameters.Scale);

        int mapScaleInt = (int)mapScale;
        
        _requestParameters.Add(StaticMapRequestParameters.Scale, [mapScaleInt.ToString()]);
        return this;
    }

    /// <summary>
    /// Add the format parameter to the query.
    /// </summary>
    /// <example>
    /// Use the <see cref="MapFormats"/> class to specify the format.
    /// </example>
    /// <remarks>
    /// See <see href="https://developers.google.com/maps/documentation/maps-static/start#ImageFormats">official documentation</see> for details.
    /// </remarks>
    /// <param name="format">The map format.</param>
    /// <returns>The builder</returns>
    public StaticMapsUrlBuilder AddFormat(MapFormat format)
    {
        CheckIfParameterIsAlreadyAdded(StaticMapRequestParameters.Format);
        
        _requestParameters.Add(StaticMapRequestParameters.Format, [format.ToString().ToLower()]);
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
        CheckIfParameterIsAlreadyAdded(StaticMapRequestParameters.MapType);
        
        _requestParameters.Add(StaticMapRequestParameters.MapType, [mapType.ToString().ToLower()]);
        return this;
    }

    /// <summary>
    /// Add the language parameter to the query.
    /// </summary>
    /// <param name="language">The language as a 2-letter string.</param>
    /// <remarks>
    /// See <see href="https://developers.google.com/maps/documentation/maps-static/start#map-parameters">official documentation</see> for details.
    /// </remarks>
    /// <returns>The builder.</returns>
    /// <exception cref="ArgumentException">Thrown when the <paramref name="language"/> is more than 2 characters long.</exception>
    public StaticMapsUrlBuilder AddLanguage(string language)
    {
        ArgumentNullException.ThrowIfNull(language);
        
        CheckIfParameterIsAlreadyAdded(StaticMapRequestParameters.Language);

        if (language.Length > 2) throw new ArgumentException(ExceptionMessages.MalformedParametersExceptionMessages.LanguageMustBeInTwoLettersExceptionMessage);
        
        _requestParameters.Add(StaticMapRequestParameters.Language, [language]);
        return this;
    }

    /// <summary>
    /// Add the region parameter to the query.
    /// </summary>
    /// <remarks>
    /// See <see href="https://developers.google.com/maps/coverage">official documentation</see> for details.
    /// </remarks>
    /// <param name="region">The region as a 2-letter string. See available regions on <see href="https://developers.google.com/maps/coverage">google documentation</see></param>
    /// <returns>The builder</returns>
    /// <exception cref="ArgumentException">Thrown when the <paramref name="region"/> is more than 2 characters long.</exception>
    public StaticMapsUrlBuilder AddRegion(string region)
    {
        ArgumentNullException.ThrowIfNull(region);
        
        CheckIfParameterIsAlreadyAdded(StaticMapRequestParameters.Region);
        
        if (region.Length > 2) throw new ArgumentException(ExceptionMessages.MalformedParametersExceptionMessages.RegionMustBeInTwoLettersExceptionMessage);
        
        _requestParameters.Add(StaticMapRequestParameters.Region, [region]);
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
        
        _requestParameters.Add(StaticMapRequestParameters.MapId, [mapId]);
        return this;
    }

    /// <summary>
    /// Add markers with the same style to the query.
    /// </summary>
    /// <remarks>
    /// Use the <see cref="AddMarkers"/> method to add markers with unique style.
    /// <br/>
    /// This method allows adding multiple <see cref="MarkerGroup"/>.
    /// <br/>
    /// See <see href="https://developers.google.com/maps/documentation/maps-static/start?#Markers">official documentation</see> for details.
    /// </remarks>
    /// <example>
    /// If you want to add multiple <see cref="MarkerGroup"/> do not call the method multiple times. Do this instead:
    /// <code>
    /// var _ = new StaticMapsUrlBuilder().AddMarkerGroups(markerGroup1, markerGroup2, markerGroup3);
    /// </code>
    /// </example>
    /// <param name="markerGroups">The groups of markers to add.</param>
    /// <returns>The builder</returns>
    public StaticMapsUrlBuilder AddMarkerGroups(params MarkerGroup[] markerGroups)
    {
        ArgumentNullException.ThrowIfNull(markerGroups);
        
        if (!_requestParameters.TryGetValue(StaticMapRequestParameters.Marker, out List<string>? list))
        {
            list = [];
            _requestParameters[StaticMapRequestParameters.Marker] = list;
        }

        foreach (MarkerGroup group in markerGroups)
        {
            list.Add(group.ToString());
            _locationMarkersCount += group.LocationCount;

            if (group.IconUrl is not null) _markerIconUrls.Add(group.IconUrl);
        }

        _isCenterOrZoomMandatory = false;
        return this;
    }

    /// <summary>
    /// Add markers with unique styles.
    /// </summary>
    /// <remarks>
    /// Use the <see cref="AddMarkerGroups"/> method to add multiple markers with the same style.
    /// <br/>
    /// This method allows adding multiple <see cref="Marker"/>. 
    /// <br/>
    /// See <see href="https://developers.google.com/maps/documentation/maps-static/start?#Markers">official documentation</see> for details.
    /// </remarks>
    /// <example>
    /// If you want to add multiple <see cref="Marker"/> do not call the method multiple times. Do this instead:
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

        if (!_requestParameters.TryGetValue(StaticMapRequestParameters.Marker, out List<string>? list))
        {
            list = [];
            _requestParameters[StaticMapRequestParameters.Marker] = list;
        }

        foreach (Marker marker in markers)
        {
            list.Add(marker.ToString());
            _locationMarkersCount += marker is LocationMarker ? 1 : 0;

            if (marker.IconUrl is not null) _markerIconUrls.Add(marker.IconUrl);
        }

        _isCenterOrZoomMandatory = false;
        return this;
    }

    /// <summary>
    /// Add paths to the query.
    /// </summary>
    /// <remarks>
    /// This method allows adding multiple <see cref="Path"/>.
    /// <br/>
    /// See <see href="https://developers.google.com/maps/documentation/maps-static/start#Paths">official documentation</see> for details.
    /// </remarks>
    /// <example>
    /// If you want to add multiple <see cref="Path"/> do not call the method multiple times. Do this instead:
    /// <code>
    /// var _ = new StaticMapsUrlBuilder().AddPaths(path1, path2, path3);
    /// </code>
    /// </example>
    /// <param name="paths">The paths to add.</param>
    /// <returns>The builder.</returns>
    public StaticMapsUrlBuilder AddPaths(params Path[] paths)
    {
        ArgumentNullException.ThrowIfNull(paths);
        
        if (!_requestParameters.TryGetValue(StaticMapRequestParameters.Path, out List<string>? list))
        {
            list = [];
            _requestParameters[StaticMapRequestParameters.Path] = list;
        }

        foreach (Path path in paths)
        {
            list.Add(path.ToString());
            _locationPointsForPathsCount += path.LocationCount;
        }

        _isCenterOrZoomMandatory = false;
        return this;
    }
    
    /// <summary>
    /// Add viewports to the map
    /// </summary>
    /// <remarks>
    /// This method allows adding multiple viewports with location strings.
    /// <br/>
    /// See <see href="https://developers.google.com/maps/documentation/maps-static/start#Viewports">official documentation</see> for details.
    /// </remarks>
    /// <example>
    /// If you want to add multiple view ports, do not call the method multiples times. Do this instead:
    /// <code>
    /// var _ = new StaticMapsUrlBuilder().AddViewportWithLocation(location1, location2, location3);
    /// </code>
    /// </example>
    /// <param name="locations">The location of the viewport as a human-readable location.</param>
    /// <returns>The builder.</returns>
    public StaticMapsUrlBuilder AddViewportWithLocation(params string[] locations)
    {
        ArgumentNullException.ThrowIfNull(locations);

        _locationsCount += locations.Length;

        if (!_requestParameters.TryGetValue(StaticMapRequestParameters.Visible, out List<string>? list))
        {
            list = [];
            _requestParameters[StaticMapRequestParameters.Visible] = list;
        }

        foreach (string location in locations)
        {
            if (string.IsNullOrWhiteSpace(location)) throw new ArgumentException("Location cannot be null or empty.", nameof(locations));

            list.Add(location);
        }

        _isCenterOrZoomMandatory = false;
        return this;
    }

    /// <summary>
    /// Add viewports to the map
    /// </summary>
    /// <remarks>
    /// This method allows adding multiple viewports with coordinates.
    /// <br/>
    /// See <see href="https://developers.google.com/maps/documentation/maps-static/start#Viewports">official documentation</see> for details.
    /// </remarks>
    /// <example>
    /// If you want to add multiple viewports, do not call the method multiples times. Do this instead:
    /// <code>
    /// var _ = new StaticMapsUrlBuilder().AddVisiblePortWithCoordinates(coordinates1, coordinates2, coordinates3);
    /// </code>
    /// </example>
    /// <param name="coordinates">The coordinates of the viewport.</param>
    /// <returns>The builder.</returns>
    public StaticMapsUrlBuilder AddVisiblePortWithCoordinates(params (double, double)[] coordinates)
    {
        ArgumentNullException.ThrowIfNull(coordinates);

        if (!_requestParameters.TryGetValue(StaticMapRequestParameters.Visible, out List<string>? list))
        {
            list = [];
            _requestParameters[StaticMapRequestParameters.Visible] = list;
        }

        foreach ((double lat, double lng) in coordinates)
        {
            string combined = $"{lat.ToString(CultureInfo.InvariantCulture)},{lng.ToString(CultureInfo.InvariantCulture)}";

            list.Add(combined); 
        }

        _isCenterOrZoomMandatory = false;
        return this;
    }

    /// <summary>
    /// Add map styles to the query.
    /// </summary>
    /// <remarks>
    /// See <see href="https://developers.google.com/maps/documentation/maps-static/styling">official documentation</see> for details.
    /// </remarks>
    /// <example>
    /// If you want to add multiple <see cref="MapStyle"/> do not call the method multiple times. Do this instead:
    /// <code>
    /// var _ = new StaticMapsUrlBuilder().AddMapStyle(mapStyle1, mapStyle2, mapStyle3);
    /// </code>
    /// </example>
    /// <param name="mapStyles">The map style.</param>
    /// <returns>The builder.</returns>
    public StaticMapsUrlBuilder AddMapStyle(params MapStyle[] mapStyles)
    {
        ArgumentNullException.ThrowIfNull(mapStyles);
        
        if (!_requestParameters.TryGetValue(StaticMapRequestParameters.Style, out List<string>? list))
        {
            list = [];
            _requestParameters[StaticMapRequestParameters.Style] = list;
        }

        foreach (MapStyle mapStyle in mapStyles)
        {
            list.Add(mapStyle.ToString());
        }
        
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
        
        _requestParameters.Add(StaticMapRequestParameters.Key, [key]);
        return this;
    }

    /// <summary>
    /// Use the HTTP protocol instead of HTTPS.
    /// </summary>
    /// <remarks>
    /// This method is obsolete and will be removed soon. Use <see cref="StaticMapsUrlBuilder.WithOptions"/> method and
    /// <see cref="StaticMapBuilderOptions.UseHttp"/>.
    /// </remarks>
    /// <returns>The builder.</returns>
    [Obsolete($"Use {nameof(WithOptions)} method and {nameof(StaticMapBuilderOptions)}.{nameof(StaticMapBuilderOptions.UseHttp)} instead.")]
    public StaticMapsUrlBuilder UseHttp()
    {
        _options = _options with { UseHttp = true };
        return this;
    }

    /// <summary>
    /// Disable the url encoding. The url is, by default, url encoded.
    /// </summary>
    /// <remarks>
    /// This method is obsolete and will be removed soon. Use <see cref="StaticMapsUrlBuilder.WithOptions"/> method and
    /// <see cref="StaticMapBuilderOptions.DisableUrlEncoding"/>.
    /// <br/>
    /// See <see href="https://developers.google.com/maps/url-encoding">official documentation</see> for details.
    /// </remarks>
    /// <returns>The builder.</returns>
    [Obsolete($"Use {nameof(WithOptions)} method and {nameof(StaticMapBuilderOptions)}.{nameof(StaticMapBuilderOptions.DisableUrlEncoding)} instead.")]
    public StaticMapsUrlBuilder DisableUrlEncoding()
    {
        _options = _options with { DisableUrlEncoding = true };
        return this;
    }

    /// <summary>
    /// Disable the api key check.
    /// </summary>
    /// <remarks>
    /// Disable the api key check allowing to have a "raw" request url.
    /// </remarks>
    /// <returns>The builder.</returns>
    [Obsolete($"Use {nameof(WithOptions)} method and {nameof(StaticMapBuilderOptions)}.{nameof(StaticMapBuilderOptions.DisableApiKeyCheck)} instead.")]
    public StaticMapsUrlBuilder DisableApiKeyCheck()
    {
        _options = _options with { DisableApiKeyCheck = true };
        return this;
    }

    /// <summary>
    /// Instructs the builder to return only the relative URL (query parameters),
    /// without including the full base address.
    /// </summary>
    /// <remarks>
    /// This is useful when using a preconfigured <see cref="HttpClient"/> that already includes
    /// the Google Static Maps base address. It prevents overriding the base URI and allows clean 
    /// composition of the full request URL using only the relevant parameters.
    /// </remarks>
    /// <returns>The builder.</returns>
    [Obsolete($"Use {nameof(WithOptions)} method and {nameof(StaticMapBuilderOptions)}.{nameof(StaticMapBuilderOptions.ReturnParametersOnly)} instead.")]
    public StaticMapsUrlBuilder ReturnRelativeUrlOnly()
    {
        _options = _options with { ReturnParametersOnly = true };
        return this;
    }

    /// <summary>
    /// Add builder option to custom the result of the <see cref="Build"/> method.
    /// </summary>
    /// <param name="options">The builder options</param>
    /// <returns>The builder.</returns>
    public StaticMapsUrlBuilder WithOptions(StaticMapBuilderOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);

        _options = options;

        return this;
    }

    /// <summary>
    /// Build the static map url.
    /// </summary>
    /// <remarks>
    /// This method validates the parameters and builds the url.
    /// <br/>
    /// The url is, by default, url encoded. Use <see cref="WithOptions"/> to disable the url encoding.
    /// <br/>
    /// The url is, by default, built using the HTTPS protocol. Use <see cref="UseHttp"/> to use the HTTP protocol.
    /// </remarks>
    /// <returns>The static map url generated according to added parameters and build options.</returns>
    /// <exception cref="ArgumentException">Thrown when the API Key is missing and the <see cref="StaticMapBuilderOptions.DisableApiKeyCheck"/> is false.</exception>
    /// <exception cref="ArgumentException">Thrown when no parameter was added to the builder.</exception>
    /// <exception cref="InvalidOperationException">Thrown when the url exceeds the maximum length.</exception>
    /// <exception cref="InvalidOperationException">Thrown when a "positioning argument" is missing.</exception>
    /// <exception cref="InvalidOperationException">Thrown when the size parameter is missing.</exception>
    /// <exception cref="InvalidOperationException">Thrown when the url exceeds the maximum length.</exception>
    /// <exception cref="InvalidOperationException">Thrown when the number of markers with a location path exceeds the limit. (15)</exception>
    /// <exception cref="InvalidOperationException">Thrown when the number of paths with a location path exceeds the limit. (15 in total)</exception>
    /// <exception cref="InvalidOperationException">Thrown when the map id parameter is combined with a map style parameter.</exception>
    /// <exception cref="InvalidOperationException">Thrown when the number of location paths exceeds the limit (excluding paths and markers, it's 3)</exception>
    public string Build()
    {
        ValidateParameters();
        if (!_options.DisableApiKeyCheck && !_requestParameters.ContainsKey(StaticMapRequestParameters.Key)) throw new ArgumentException(ExceptionMessages.ParametersMissingExceptionMessages.ApiKeyParameterMissingMessage);
        
        string url;

        if (_options.ReturnParametersOnly)
        {
            url = string.Empty;
        }
        else 
        {
            if (_options.UseHttp)
            {
                url = ProjectConstants.StaticMapBaseUrlHttp + "?";
            }
            else
            {
                url = ProjectConstants.StaticMapBaseUrlHttps + "?";
            }
        }

        List<string> parts = [];

        foreach ((StaticMapRequestParameters type, List<string> values) in _requestParameters)
        {
            var strategy = ParameterFormattingStrategySelector.Choose(type);

            if (!_options.DisableUrlEncoding)
            {
                strategy = new UrlEncodingDecorator(strategy);
            }
            
            string paramName = RequestParameterNames.Map[type];
            
            IReadOnlyList<string> formattedValues = strategy.FormatValues(values);

            parts.AddRange(formattedValues.Select(val => $"{paramName}={val}"));
        }
        
        url += string.Join("&", parts);
        
        if (url.Length > ProjectConstants.StaticMapsApiUrlMaxSize)
        {
            throw new InvalidOperationException(ExceptionMessages.StaticMapUrlExceededLengthExceptionMessage);
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

        if (_markerIconUrls.Distinct().Count() > ProjectConstants.CustomMakerIconsCountLimit)
        {
            throw new InvalidOperationException(ExceptionMessages.UrlParametersExceptionMessages.TooMuchDistinctCustomMarkerIcons);
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
