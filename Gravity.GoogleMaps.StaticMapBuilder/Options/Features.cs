#pragma warning disable CS1591 
namespace Gravity.GoogleMaps.StaticMapBuilder.Options;

/// <summary>
/// The collection of available <see cref="Feature"/>.
/// </summary>
/// <remarks>
/// See <see href="https://developers.google.com/maps/documentation/maps-static/styling#features">official documentation.</see>
/// for details about the available features.
/// </remarks>
public static class Features
{
    /// <summary>
    /// Selects all features of the map.
    /// </summary>
    public static readonly Feature All = new("all");
    
    public static class Administrative
    {
        /// <summary>
        /// Selects all administrative areas. Styling affects only the labels of administrative areas, not the geographical borders or fill
        /// </summary>
        public static readonly Feature AllAdministrative = new("administrative");
        
        /// <summary>
        /// Selects countries.
        /// </summary>
        public static readonly Feature Country = new("administrative.country");
        
        /// <summary>
        /// Selects land parcels.
        /// </summary>
        public static readonly Feature LandParcel = new("administrative.land_parcel");
         
        /// <summary>
        /// Selects localities.
        /// </summary>
        public static readonly Feature Locality = new("administrative.locality");
        
        /// <summary>
        /// Select neighborhoods.
        /// </summary>
        public static readonly Feature Neighborhood = new("administrative.neighborhood");
        
        /// <summary>
        /// Selects provinces.
        /// </summary>
        public static readonly Feature Province = new("administrative.province");
    }

    public static class Landscape
    {
        /// <summary>
        /// Selects all landscapes.
        /// </summary>
        public static readonly Feature AllLandscape = new("landscape");
        
        /// <summary>
        /// Selects man-made features, such as buildings and other structures.
        /// </summary>
        public static readonly Feature ManMade = new("landscape.man_made");

        public static class Natural
        {
            /// <summary>
            /// Selects natural features, such as mountains, rivers, deserts, and glaciers.
            /// </summary>
            public static readonly Feature AllLandscapeNatural = new("landscape.natural");
            
            /// <summary>
            /// Selects land cover features, the physical material that covers the earth's surface, such as forests, grasslands, wetlands, and bare ground.
            /// </summary>
            public static readonly Feature Landcover = new("landscape.natural.landcover");
            
            /// <summary>
            /// Selects terrain features of a land surface, such as elevation, slope, and orientation.
            /// </summary>
            public static readonly Feature Terrain = new("landscape.natural.terrain");
        }
    }

    public static class PointOfInterest
    {
        /// <summary>
        /// Selects all points of interest.
        /// </summary>
        public static readonly Feature AllPointsOfInterest = new("poi");
        
        /// <summary>
        /// Selects tourist attractions.
        /// </summary>
        public static readonly Feature Attraction = new("poi.attraction");
        
        /// <summary>
        /// Selects businesses.
        /// </summary>
        public static readonly Feature Business = new("poi.business");
        
        /// <summary>
        /// Selects government buildings.
        /// </summary>
        public static readonly Feature Governement = new("poi.government");
        
        /// <summary>
        /// Selects emergency services, including hospitals, pharmacies, police, doctors, and others.
        /// </summary>
        public static readonly Feature Medical = new("poi.medical");
        
        /// <summary>
        /// Selects parks.
        /// </summary>
        public static readonly Feature Park = new("poi.park");
        
        /// <summary>
        /// Selects places of worship, including churches, temples, mosques, and others.
        /// </summary>
        public static readonly Feature PlaceOfWorship = new("poi.place_of_worship");
        
        /// <summary>
        /// Selects schools.
        /// </summary>
        public static readonly Feature School = new("poi.school");
        
        /// <summary>
        /// Selects sports complexes.
        /// </summary>
        public static readonly Feature SportComplex = new("poi.sports_complex");
    }

    public static class Road
    {
        /// <summary>
        /// Selects all roads.
        /// </summary>
        public static readonly Feature AllRoad = new("road");
        
        /// <summary>
        /// Selects arterial roads.
        /// </summary>
        public static readonly Feature Arterial = new("road.arterial");

        public static class Highway
        {
            /// <summary>
            /// Selects highways.
            /// </summary>
            public static readonly Feature AllHighway = new("road.highway");
            
            /// <summary>
            /// Selects highways with controlled access.
            /// </summary>
            public static readonly Feature ControlledAccessHighway = new("road.highway.controlled_access");
        }
       
        /// <summary>
        /// Selects highways with controlled access.
        /// </summary>
        public static readonly Feature Local = new("road.local");
    }

    public static class Transit
    {
        /// <summary>
        /// Selects all transit stations and lines.
        /// </summary>
        public static readonly Feature AllTransit = new("transit");
        
        /// <summary>
        /// Selects transit lines.
        /// </summary>
        public static readonly Feature Line = new("transit.line");
        
        public static class Station
        {
            /// <summary>
            /// Selects all transit stations.
            /// </summary>
            public static readonly Feature AllStation = new("transit.station");
            
            /// <summary>
            /// Selects airports.
            /// </summary>
            public static readonly Feature Airport = new("transit.station.airport");
            
            /// <summary>
            /// Selects bus stops.
            /// </summary>
            public static readonly Feature BusStation = new("transit.station.bus");
            
            /// <summary>
            /// Selects rail stations.
            /// </summary>
            public static readonly Feature RailStation = new("transit.station.rail");
        }
    }
    
    /// <summary>
    /// Selects bodies of water.
    /// </summary>
    public static readonly Feature Water = new("water");
}