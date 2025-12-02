using Path = Gravity.GoogleMaps.StaticMapBuilder.Models.Path;

namespace Gravity.GoogleMaps.StaticMapBuilder.Builders;

public class StaticMapUrlBuilder() : GoogleMapsBuilder<StaticMapUrlBuilder, StaticMapQueryParameters, StaticMapBuilderOptions>(new StaticMapBuilderOptions())
{
    // Exception messages

    private const string WrongMethodForMarkerGroupExceptionMessage = $"This method should not be used for {nameof(MarkerGroup)} objects. Use {nameof(AddMarkerGroups)} instead.";
    
    // Properties
    
    /// <inheritdoc />
    protected override StaticMapQueryParameters Parameters { get; set; } = new();
    
    // Methods

    /// <inheritdoc />
    protected override string GetBaseUrl() => Options.UseHttp ? ApiEndpoints.StaticMapBaseUrlHttp : ApiEndpoints.StaticMapBaseUrlHttps;

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
    public StaticMapUrlBuilder AddCenterWithLocation(string location)
    {
        if (string.IsNullOrEmpty(location) || string.IsNullOrWhiteSpace(location)) throw new ArgumentNullException(nameof(location));

        Parameters.Add(new CenterQueryParameter(location));
        
        Parameters.LocationsCount++;
        
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
    public StaticMapUrlBuilder AddCenterWithCoordinates(double latitude, double longitude)
    {
        if (latitude is < -90 or > 90) throw new ArgumentOutOfRangeException(nameof(latitude));
        if (longitude is < -180 or > 180) throw new ArgumentOutOfRangeException(nameof(longitude));
        
        Parameters.Add(new CenterQueryParameter(latitude, longitude));
        
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
    public StaticMapUrlBuilder AddZoom(int zoom)
    {
        if (zoom is < 0 or > 22) throw new ArgumentOutOfRangeException(nameof(zoom),
            $"The zoom parameter must be between 0 and 22. The resulting static map will " +
            $"use either zoom 0 if their is no markers or a scale-based zoom if there are markers." +
            $" See : {DocumentationLinks.StaticMap.Sections.ZoomLevels} for more information.");
        
        Parameters.Add(new ZoomQueryParameter(zoom));
        
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
    public StaticMapUrlBuilder AddSize(int width, int height)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(width);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(height);
        
        Parameters.Add(new SizeQueryParameter(width, height));
        
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
    public StaticMapUrlBuilder AddScale(MapScale mapScale)
    {
        Parameters.Add(new ScaleQueryParameter(mapScale));
        
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
    public StaticMapUrlBuilder AddFormat(MapFormat format)
    {
        Parameters.Add(new FormatQueryParameter(format));
        
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
    public StaticMapUrlBuilder AddMapType(StaticMapType mapType)
    {
        Parameters.Add(new MapTypeQueryParameter(mapType));
        
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
    public StaticMapUrlBuilder AddLanguage(string language)
    {
        if (string.IsNullOrEmpty(language) || string.IsNullOrWhiteSpace(language)) throw new ArgumentNullException(nameof(language));
        if (language.Length != 2) throw new ArgumentException(ExceptionMessages.MalformedParametersExceptionMessages.LanguageMustBeInTwoLettersExceptionMessage, nameof(language));
        
        Parameters.Add(new LanguageQueryParameter(language));
        
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
    public StaticMapUrlBuilder AddRegion(string region) // TODO : Change this to use static values like Elements etc
    {
        if (string.IsNullOrEmpty(region) || string.IsNullOrWhiteSpace(region)) throw new ArgumentNullException(nameof(region));
        if (region.Length != 2) throw new ArgumentException(ExceptionMessages.MalformedParametersExceptionMessages.RegionMustBeInTwoLettersExceptionMessage, nameof(region));
        
        Parameters.Add(new RegionQueryParameter(region));
        
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
    public StaticMapUrlBuilder AddMapId(string mapId)
    {
        if (string.IsNullOrEmpty(mapId) || string.IsNullOrWhiteSpace(mapId)) throw new ArgumentNullException(nameof(mapId));
        if (mapId.Length > ApiLimits.MapIdMaxLength) throw new ArgumentException(ExceptionMessages.MalformedParametersExceptionMessages.RegionMustBeInTwoLettersExceptionMessage, nameof(mapId));
        
        Parameters.Add(new MapIdQueryParameter(mapId));
        
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
    /// var _ = new StaticMapUrlBuilder().AddMarkerGroups(markerGroup1, markerGroup2, markerGroup3);
    /// </code>
    /// </example>
    /// <param name="markerGroups">The groups of markers to add.</param>
    /// <returns>The builder</returns>
    public StaticMapUrlBuilder AddMarkerGroups(params MarkerGroup[] markerGroups)
    {
        ArgumentNullException.ThrowIfNull(markerGroups);
        
        markerGroups.ToList().ForEach(group =>
        {
            Parameters.Add(new MarkerQueryParameter(group));
            
            Parameters.LocationMarkersCount += group.LocationCount;
            
            if (group.IconUrl is not null) Parameters.MarkerIconUrls.Add(group.IconUrl);
        });

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
    /// var _ = new StaticMapUrlBuilder().AddMarkers(marker1, marker2, marker3);
    /// </code>
    /// </example>
    /// <param name="markers">The markers to add.</param>
    /// <returns>The builder</returns>
    /// <exception cref="InvalidOperationException">Thrown when one or more of the given <paramref name="markers"/> is a <see cref="MarkerGroup"/></exception>
    public StaticMapUrlBuilder AddMarkers(params Marker[] markers)
    {
        ArgumentNullException.ThrowIfNull(markers);
        if (markers.Any(marker => marker is MarkerGroup)) throw new InvalidOperationException(WrongMethodForMarkerGroupExceptionMessage);

        markers.ToList().ForEach(marker =>
        {
            Parameters.Add(new MarkerQueryParameter(marker));
            
            Parameters.LocationMarkersCount += marker is LocationMarker ? 1 : 0;

            if (marker.IconUrl is not null) Parameters.MarkerIconUrls.Add(marker.IconUrl);
        });

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
    /// var _ = new StaticMapUrlBuilder().AddPaths(path1, path2, path3);
    /// </code>
    /// </example>
    /// <param name="paths">The paths to add.</param>
    /// <returns>The builder.</returns>
    public StaticMapUrlBuilder AddPaths(params Path[] paths)
    {
        ArgumentNullException.ThrowIfNull(paths);
        
        paths.ToList().ForEach(path =>
        {
            Parameters.Add(new PathQueryParameter(path));
            Parameters.LocationPointForPathCount += path.LocationCount;
        });

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
    /// var _ = new StaticMapUrlBuilder().AddViewportWithLocation(location1, location2, location3);
    /// </code>
    /// </example>
    /// <param name="locations">The location of the viewport as a human-readable location.</param>
    /// <returns>The builder.</returns>
    public StaticMapUrlBuilder AddViewports(params string[] locations)
    {
        ArgumentNullException.ThrowIfNull(locations);
        
        if (locations.Any(l => string.IsNullOrEmpty(l) || string.IsNullOrWhiteSpace(l))) throw new ArgumentNullException(nameof(locations), $"One or more location in the {nameof(locations)} argument is null or empty !");

        Parameters.Add(new ViewportsQueryParameter(locations));

        Parameters.LocationsCount += locations.Length;
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
    /// var _ = new StaticMapUrlBuilder().AddVisiblePortWithCoordinates(coordinates1, coordinates2, coordinates3);
    /// </code>
    /// </example>
    /// <param name="coordinates">The coordinates of the viewport.</param>
    /// <returns>The builder.</returns>
    public StaticMapUrlBuilder AddViewports(params (double, double)[] coordinates)
    {
        ArgumentNullException.ThrowIfNull(coordinates);

        Parameters.Add(new ViewportsQueryParameter(coordinates));

        return this;
    }

    [Obsolete($"Use {nameof(AddViewports)} instead")]
    public StaticMapUrlBuilder AddVisiblePortWithCoordinates(params (double, double)[] coordinates)
    {
        return AddViewports(coordinates);
    }
    
    [Obsolete($"Use {nameof(AddViewports)} instead")]
    public StaticMapUrlBuilder AddViewportsWithLocation(params string[] locations)
    {
        return AddViewports(locations);
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
    /// var _ = new StaticMapUrlBuilder().AddMapStyle(mapStyle1, mapStyle2, mapStyle3);
    /// </code>
    /// </example>
    /// <param name="mapStyles">The map style.</param>
    /// <returns>The builder.</returns>
    public StaticMapUrlBuilder AddMapStyle(params MapStyle[] mapStyles)
    {
        ArgumentNullException.ThrowIfNull(mapStyles);
        
        foreach (MapStyle mapStyle in mapStyles)
        {
            Parameters.Add(new MapStyleQueryParameter(mapStyle));
        }
        
        return this;
    }

    /// <inheritdoc />
    public override string Build()
    {
        string url =  base.Build();
        
        if (url.Length > ApiLimits.StaticMapsApiUrlMaxSize)
        {
            throw new InvalidOperationException(ExceptionMessages.StaticMapUrlExceededLengthExceptionMessage);
        }
        
        return url;
    }
}