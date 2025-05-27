using System.Diagnostics;
using System.Globalization;
using Path = Gravity.GoogleMaps.StaticMapBuilder.Models.Path;

namespace Gravity.GoogleMaps.StaticMapBuilder.Builders;

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
    
    public StaticMapsUrlBuilder AddScale(MapScale mapScale)
    {
        ArgumentNullException.ThrowIfNull(mapScale);
        
        CheckIfParameterIsAlreadyAdded(StaticMapRequestParameters.Scale);

        int mapScaleInt = (int)mapScale;
        
        _requestParameters.Add(StaticMapRequestParameters.Scale, $"scale={mapScaleInt}");
        return this;
    }

    public StaticMapsUrlBuilder AddFormat(MapFormat format)
    {
        ArgumentNullException.ThrowIfNull(format);
        
        CheckIfParameterIsAlreadyAdded(StaticMapRequestParameters.Format);
        
        string formatString = format.ToString().ToLower();
        formatString = _encodeToUrl ? Uri.EscapeDataString(formatString) : formatString;
        
        _requestParameters.Add(StaticMapRequestParameters.Format, $"format={formatString}");
        return this;
    }
    
    public StaticMapsUrlBuilder AddMapType(StaticMapType mapType)
    {
        ArgumentNullException.ThrowIfNull(mapType);
        
        CheckIfParameterIsAlreadyAdded(StaticMapRequestParameters.MapType);
        
        string mapTypeString = mapType.ToString().ToLower();
        mapTypeString = _encodeToUrl ? Uri.EscapeDataString(mapTypeString) : mapTypeString;
        
        _requestParameters.Add(StaticMapRequestParameters.MapType, $"maptype={mapTypeString}");
        return this;
    }

    public StaticMapsUrlBuilder AddLanguage(string language)
    {
        ArgumentNullException.ThrowIfNull(language);
        
        CheckIfParameterIsAlreadyAdded(StaticMapRequestParameters.Language);

        if (language.Length > 2) throw new ArgumentException(ExceptionMessages.MalformedParametersExceptionMessages.LanguageMustBeInTwoLettersExceptionMessage);
        
        language = _encodeToUrl ? Uri.EscapeDataString(language) : language;
        
        _requestParameters.Add(StaticMapRequestParameters.Language, $"language={language}");
        return this;
    }

    public StaticMapsUrlBuilder AddRegion(string region)
    {
        ArgumentNullException.ThrowIfNull(region);
        
        CheckIfParameterIsAlreadyAdded(StaticMapRequestParameters.Region);
        
        if (region.Length > 2) throw new ArgumentException(ExceptionMessages.MalformedParametersExceptionMessages.RegionMustBeInTwoLettersExceptionMessage);
        
        region = _encodeToUrl ? Uri.EscapeDataString(region) : region;
        
        _requestParameters.Add(StaticMapRequestParameters.Region, $"region={region}");
        return this;
    }

    public StaticMapsUrlBuilder AddMapId(string mapId)
    {
        ArgumentNullException.ThrowIfNull(mapId);
        
        CheckIfParameterIsAlreadyAdded(StaticMapRequestParameters.MapId);
        
        if (mapId.Length > ProjectConstants.MapIdMaxLength) throw new ArgumentException(ExceptionMessages.MalformedParametersExceptionMessages.MapIdMustBeSixteenCharactersMax);
        
        mapId = _encodeToUrl ? Uri.EscapeDataString(mapId) : mapId;
        
        _requestParameters.Add(StaticMapRequestParameters.MapId, $"map_id={mapId}");
        return this;
    }

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
    
    public StaticMapsUrlBuilder AddVisiblePlaceWithLocation(params string[] location)
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

    public StaticMapsUrlBuilder AddVisiblePlaceWithCoordinates(params (double, double)[] coordinates)
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
    
    public StaticMapsUrlBuilder AddKey(string key)
    {
        if (string.IsNullOrEmpty(key) || string.IsNullOrWhiteSpace(key)) throw new ArgumentNullException(nameof(key), "Key cannot be null or empty");
        
        _requestParameters.Add(StaticMapRequestParameters.Key, $"key={key}");
        return this;
    }

    public StaticMapsUrlBuilder UseHttp()
    {
        _useHttp = true;
        return this;
    }

    public StaticMapsUrlBuilder DisableUrlEncoding()
    {
        if (_requestParameters.Count > 0) throw new InvalidOperationException(DisableEncodingBeforeAddingParametersExceptionMessage);
        _encodeToUrl = false;
        return this;
    }

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
        ValidateVisible();
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

    private void ValidateVisible()
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
