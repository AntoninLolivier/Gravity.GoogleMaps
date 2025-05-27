# 🌍 Gravity.GoogleMaps.StaticMapBuilder

**Gravity.GoogleMaps.StaticMapBuilder** is a fully type-safe, fluent, and extensible `.NET` URL builder for the [Google Static Maps API](https://developers.google.com/maps/documentation/maps-static/start).
It helps you generate valid, optimized static map URLs effortlessly — with full IntelliSense, compile-time checks, and support for complex features like styles, markers, paths, map IDs, and more.

---

## 🚀 Features

* ✅ Fully type-safe and fluent API
* 🧠 Smart validation for all query parameters
* 🎨 Full support for **styles**, **map types**, and **custom markers**
* 🧭 Handles edge cases (map ID conflicts, marker limits, etc.)
* 🔐 Built with test coverage in mind (96%+)
* 🧪 Easy to unit test & extend

---

## 📦 Installation

```bash
dotnet add package Gravity.GoogleMaps.StaticMapBuilder
```

---

## ⚡ Quick Start

```csharp
var url = new StaticMapsUrlBuilder()
    .AddCenterWithCoordinates(48.8566, 2.3522)
    .AddZoom(13)
    .AddSize(600, 400)
    .AddMarkers(
        new LocationMarker("Eiffel Tower", label: 'E'),
        new CoordinatesMarker(48.8584, 2.2945, label: 'T')
    )
    .AddMapStyle(
        new MapStyle(
            new StyleRule(Color: new HexColor("0xFF0000")),
            Feature.Roads,
            Element.Geometry
        )
    )
    .AddKey("YOUR_API_KEY")
    .Build();
```

---

## 🧱 Fluent Builders

Every parameter of the API is covered through intuitive, self-documenting builder methods:

```csharp
.AddCenterWithLocation("Paris")
.AddZoom(12)
.AddScale(MapScale.Two)
.AddMapType(StaticMapType.Terrain)
.AddLanguage("en")
```

---

## 🎨 Custom Styles Made Simple

```csharp
var style = new MapStyle(
    new StyleRule(Hue: new HexColor("0x00FF00"), Visibility: Visibility.Simplified),
    Feature.Administrative.Country,
    Element.Labels.Text
);
```

---

## 📍 Marker Groups and Icons

```csharp
var group = new MarkerGroup(MarkerSize.Mid, iconUrl: "http://yourcdn.com/marker.png");
group.AddLocation("Lyon");
group.AddCoordinates(45.75, 4.85);
```

---

## 🧪 Test Coverage & Reliability

* **95%+ code coverage**
* Every builder method has unit and integration tests
* Constraints validated both at build-time and runtime

---

## 🛡️ License

[MIT License](LICENSE)

---

## 🙌 Credits

Made with ❤️ by **GravityApps**
Developed & maintained by freelance developer https://github.com/AntoninLolivier

---

Want to contribute or suggest an improvement?
👉 [Open an issue or pull request on GitHub](https://github.com/your-org/Gravity.GoogleMaps)
