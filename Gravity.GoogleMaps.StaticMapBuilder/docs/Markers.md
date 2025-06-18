# 🗺️ Markers – Add Points to Your Static Map

This guide shows how to use the `Gravity.GoogleMaps.StaticMapBuilder` package to add markers to your static map, using either standalone markers or grouped markers with shared styles.

> 📚 Reference: [Official Google Maps Static API documentation](https://developers.google.com/maps/documentation/maps-static/start#Markers)

---

## 🎯 Marker Types

You can add markers in two distinct ways:

- **Standalone markers**: Each marker has its own style.  
  Use `CoordinatesMarker` (with lat/lng) or `LocationMarker` (with a human-readable location).

- **Grouped markers**: All markers share the same style.  
  Use `MarkerGroup` to define the shared style and add multiple locations.

---

## ✳️ Standalone Markers

These appear as separate `&markers=...` entries in the URL.

### 🔍 Google API Example

```
&markers=color:blue|label:S|62.107733,-145.541936
&markers=size:tiny|color:green|Delta Junction,AK
&markers=size:mid|color:0xFFFF00|label:C|Tok,AK
```

### ✅ StaticMapBuilder Equivalent

```csharp
var sMarker = new CoordinatesMarker(
    62.107733,
    -145.541936,
    color: StaticMapColor.Blue,
    label: 'S');

var deltaMarker = new LocationMarker(
    "Delta Junction,AK",
    size: MarkerSize.Tiny,
    color: StaticMapColor.Green);

var tokMarker = new LocationMarker(
    "Tok,AK",
    size: MarkerSize.Mid,
    color: new HexColor("0xFFFF00"),
    label: 'C');

builder.AddMarkers(sMarker, deltaMarker, tokMarker);
```

> 💡 You may call `AddMarkers()` multiple times, but grouping in one call is more efficient.

---

## 👥 Grouped Markers (Same Style)

These markers share the same style and appear as a **single** `&markers=...` parameter.

### 🔍 Google API Example

```text
&markers=color:blue|label:S|11211|11206|11222
```

### ✅ StaticMapBuilder Equivalent

```csharp
var nyMarkersGroup = new MarkerGroup(
    color: StaticMapColor.Blue,
    label: 'S');

nyMarkersGroup.AddLocation("11211");
nyMarkersGroup.AddLocation("11206");
nyMarkersGroup.AddLocation("11222");

builder.AddMarkerGroups(nyMarkersGroup);
```

> 📦 You can define and add multiple `MarkerGroup` instances as needed.

---

## 🧠 Notes & Best Practices

* You can mix `AddMarkers()` and `AddMarkerGroups()` in the same builder.
* Avoid exceeding the markers limits 
    * **Maximum 5 custom marker icons**
    * **Up to 15 markers with custom locations (lat/lng or addresses)**
* Use `HexColor` for custom color values (`0xRRGGBB` but NOT `0xRRGGBBAA`, transparency is not allowed for markers).

---

## 📌 Example Usage Summary

```csharp
var builder = new StaticMapsUrlBuilder()
    .AddSize(600, 400)
    .AddCenterWithLocation("New York")
    .AddZoom(10)
    .AddMarkers(sMarker, deltaMarker)
    .AddMarkerGroups(nyMarkersGroup)
    .AddKey("YOUR_API_KEY");

string url = builder.Build();
```

## 📚 Related Docs

* [`MapStyle`](./MapStyle.md)
* [`Paths`](./Paths.md)
* [HexColor documentation](https://developers.google.com/maps/documentation/maps-static/start#Color)