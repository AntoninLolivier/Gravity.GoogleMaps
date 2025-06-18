# 🎨 Map Styles – Customize the Visual Appearance of Your Map

The Google Static Maps API allows you to style your maps with fine-grained control over features and elements.  
This guide shows how to use the `MapStyle`, `StyleRule`, `Feature`, and `Element` types in `Gravity.GoogleMaps.StaticMapBuilder`.

> 📚 Reference: [Google Maps Static API – Styling](https://developers.google.com/maps/documentation/maps-static/styling)

---

## 🧱 Basic Concepts

A map style is composed of:

- **Feature**: The map component to target (e.g., roads, landscape, points of interest)
- **Element**: The visual part of the feature (e.g., fill, stroke, label)
- **StyleRule**: The visual modifications to apply (color, lightness, visibility, etc.)

---

## 🖌️ Example: Style Roads, Landscape, and Labels

### 🌐 URL Equivalent

```text
&style=feature:road.local|element:geometry|color:0x00ff00
&style=feature:landscape|element:geometry.fill|color:0x000000
&style=element:labels|invert_lightness:true
&style=feature:road.arterial|element:labels|invert_lightness:false
````

### ✅ Code Equivalent

```csharp
var roadStyle = new MapStyle(
    Feature: Features.Road.Local,
    Element: Elements.Geometry.AllGeometry,
    StyleRule: new StyleRule(Color: new HexColor("0x00ff00")));

var landscapeStyle = new MapStyle(
    Feature: Features.Landscape.AllLandscape,
    Element: Elements.Geometry.Fill,
    StyleRule: new StyleRule(Color: new HexColor("0x000000")));

var labelsStyle = new MapStyle(
    Element: Elements.Labels.AllLabels,
    StyleRule: new StyleRule(InvertLightness: true));

var arterialRoadsStyle = new MapStyle(
    Feature: Features.Road.Arterial,
    Element: Elements.Labels.AllLabels,
    StyleRule: new StyleRule(InvertLightness: false));

builder.AddMapStyle(roadStyle, landscapeStyle, labelsStyle, arterialRoadsStyle);
```

---

## 🛠️ Available Properties in `StyleRule`

| Property          | Type         | Description                                                        |
| ----------------- | ------------ | ------------------------------------------------------------------ |
| `Color`           | `HexColor`   | Base color of the element (e.g. `0x000000`)                        |
| `Lightness`       | `int?`       | Brightness adjustment (-100 to 100)                                |
| `Gamma`           | `double?`    | Gamma correction for contrast                                      |
| `Saturation`      | `int?`       | Color saturation (-100 to 100)                                     |
| `Weight`          | `int?`       | Stroke weight (useful for lines)                                   |
| `InvertLightness` | `bool?`      | Reverses lightness rendering for dark/light modes                  |
| `Visibility`      | `Visibility` | Shows, hides or simplifies the element (`On`, `Off`, `Simplified`) |

---

## 🎯 Example: Hide All Labels

```csharp
var hideLabels = new MapStyle(
    Element: Elements.Labels.AllLabels,
    StyleRule: new StyleRule(Visibility: Visibility.Off));

builder.AddMapStyle(hideLabels);
```

### 🌐 URL Output

```text
&style=element:labels|visibility:off
```

---

## 🎨 Color Formats

Use `StaticMapColor` for predefined Google-safe values, or `HexColor` for custom ones:

```csharp
new HexColor("0xFF0000") // bright red
new HexColor("0x00000000") // transparent black (alpha allowed)
StaticMapColor.Red
```

---

## 🧪 Tips

* Combine multiple styles with `.AddMapStyle(...)`
* Use `Feature.All` or `Element.All` to apply a rule globally
* Make sure to escape characters like `|` and `:` if building the query manually

---

## ✅ Integration Example

```csharp
var builder = new StaticMapsUrlBuilder()
    .AddSize(800, 600)
    .AddCenterWithLocation("New York")
    .AddZoom(12)
    .AddMapStyle(hideLabels)
    .AddKey("YOUR_API_KEY");

string url = builder.Build();
```

---

## 📚 Related

* [`Markers.md`](./Markers.md)
* [`Paths.md`](./Paths.md)
* [Google Styling Reference](https://developers.google.com/maps/documentation/maps-static/styling)