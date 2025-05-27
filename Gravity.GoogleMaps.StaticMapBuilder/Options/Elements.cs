namespace Gravity.GoogleMaps.StaticMapBuilder.Options;

public static class Elements
{
    public static readonly Element All = new("all");

    public static class Geometry
    {
        public static readonly Feature AllGeometry = new("geometry");
        public static readonly Element Fill = new ("geometry.fill");
        public static readonly Element Stroke = new("geometry.stroke");
    }

    public static class Labels
    {
        public static readonly Feature AllLabels = new("labels");
        public static readonly Element Icon = new("labels.icon");

        public static class Text
        {
            public static readonly Element AllText = new("labels.text");
            public static readonly Element Fill = new("labels.text.fill");
            public static readonly Element Stroke = new("labels.text.stroke");
        }
    }
}