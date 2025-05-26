namespace Gravity.GoogleMaps.StaticMapBuilder.Options;

public static class Elements
{
    public static readonly Element All = new("all");

    public static class Geometry
    {
        public static readonly Feature AllGeometry = new("geometry");
        public static readonly Element Fill = new ("fill");
        public static readonly Element Stroke = new Element("stroke");
    }

    public static class Labels
    {
        public static readonly Feature AllLabels = new("labels");
        public static readonly Element Icon = new Element("icon");

        public static class Text
        {
            public static readonly Element AllText = new Element("text");
            public static readonly Element Fill = new Element("fill");
            public static readonly Element Stroke = new Element("stroke");
        }
    }
}