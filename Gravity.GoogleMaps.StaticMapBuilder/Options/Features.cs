namespace Gravity.GoogleMaps.StaticMapBuilder.Options;

public static class Features
{
    public static readonly Feature All = new("all");
    
    public static class Administrative
    {
        public static readonly Feature AllAdministrative = new("administrative");
        public static readonly Feature Country = new("administrative.country");
        public static readonly Feature Province = new("administrative.land_parcel");
        public static readonly Feature Locality = new("administrative.locality");
        public static readonly Feature Neighborhood = new("administrative.neighborhood");
        public static readonly Feature LandParcel = new("administrative.province");
    }

    public static class Landscape
    {
        public static readonly Feature AllLandscape = new("landscape");
        public static readonly Feature ManMade = new("landscape.man_made");

        public static class Natural
        {
            public static readonly Feature AllLandscapeNatural = new("landscape.natural");
            public static readonly Feature Landcover = new("landscape.natural.landcover");
            public static readonly Feature Terrain = new("landscape.natural.terrain");
        }
    }

    public static class PointOfInterest
    {
        public static readonly Feature AllPointsOfInterest = new("poi");
        public static readonly Feature Attraction = new("poi.attraction");
        public static readonly Feature Business = new("poi.business");
        public static readonly Feature Governement = new("poi.government");
        public static readonly Feature Medical = new("poi.medical");
        public static readonly Feature Park = new("poi.park");
        public static readonly Feature PlaceOfWorship = new("poi.place_of_worship");
        public static readonly Feature School = new("poi.school");
        public static readonly Feature SportComplex = new("poi.sports_complex");
    }

    public static class Road
    {
        public static readonly Feature AllRoad = new("road");
        public static readonly Feature Arterial = new("road.arterial");

        public static class Highway
        {
            public static readonly Feature AllHighway = new("road.highway");
            public static readonly Feature ControlledAccessHighway = new("road.highway.controlled_access");
        }
        
        public static readonly Feature Local = new("road.local");
    }

    public static class Transit
    {
        public static readonly Feature AllTransit = new("transit");
        public static readonly Feature Line = new("transit.line");
        
        public static class Station
        {
            public static readonly Feature AllStation = new("transit.station");
            public static readonly Feature Airport = new("transit.station.airport");
            public static readonly Feature BusStation = new("transit.station.bus");
            public static readonly Feature RailStation = new("transit.station.rail");
        }
    }
    
    public static readonly Feature Water = new("water");
}