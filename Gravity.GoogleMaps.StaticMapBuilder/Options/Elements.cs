#pragma warning disable CS1591 
namespace Gravity.GoogleMaps.StaticMapBuilder.Options;

/// <summary>
/// The collection of available <see cref="Element"/>.
/// </summary>
/// <remarks>
/// See <see href="https://developers.google.com/maps/documentation/maps-static/styling#elements">official documentation</see>
/// for details about the available elements.
/// </remarks>
public static class Elements
{
    /// <summary>
    /// Selects all elements of the specified feature.
    /// </summary>
    public static readonly Element All = new("all");

    public static class Geometry
    {
        /// <summary>
        /// Selects all geometric elements of the specified feature.
        /// </summary>
        public static readonly Feature AllGeometry = new("geometry");
        
        /// <summary>
        /// Selects only the fill of the feature's geometry.
        /// </summary>
        public static readonly Element Fill = new ("geometry.fill");
        
        /// <summary>
        /// Selects only the stroke of the feature's geometry.
        /// </summary>
        public static readonly Element Stroke = new("geometry.stroke");
    }

    public static class Labels
    {
        /// <summary>
        /// Selects the textual labels associated with the specified feature.
        /// </summary>
        public static readonly Feature AllLabels = new("labels");
        
        /// <summary>
        /// Selects only the icon displayed within the feature's label.
        /// </summary>
        public static readonly Element Icon = new("labels.icon");

        public static class Text
        {
            /// <summary>
            /// Selects only the text of the label.
            /// </summary>
            public static readonly Element AllText = new("labels.text");
            
            /// <summary>
            /// Selects only the fill of the label. The fill of a label is typically rendered as a colored outline that surrounds the label text.
            /// </summary>
            public static readonly Element Fill = new("labels.text.fill");
            
            /// <summary>
            /// Selects only the stroke of the label's text.
            /// </summary>
            public static readonly Element Stroke = new("labels.text.stroke");
        }
    }
}