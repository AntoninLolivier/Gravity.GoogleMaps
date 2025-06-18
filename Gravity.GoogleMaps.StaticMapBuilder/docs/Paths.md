# 🧭 Paths – Draw Lines and Shapes on Your Static Map

This guide shows how to use the `Path` class to draw lines, polygons, and complex routes using the `Gravity.GoogleMaps.StaticMapBuilder` package.

> 📚 Reference: [Google Maps Static API – Paths](https://developers.google.com/maps/documentation/maps-static/start#Paths)

---

## 🔹 Add a Basic Path (Points + Style)

### 🌐 URL Equivalent

```text
&path=color:0x0000ff|weight:5|40.737102,-73.990318|40.749825,-73.987963|40.752946,-73.987384|40.755823,-73.986397
````

### ✅ Code

```csharp
var path = new Path(
    Color: new HexColor("0x0000ff"),
    Weight: 5);

path.AddPoint(40.737102, -73.990318);
path.AddPoint(40.749825, -73.987963);
path.AddPoint(40.752946, -73.987384);
path.AddPoint(40.755823, -73.986397);
```

---

## 🎨 Path with Transparency

### 🌐 URL Equivalent

```text
&path=color:0xff0000ff|weight:5|...
```

### ✅ Code

```csharp
var path = new Path(
    Color: new HexColor("0xff0000ff"),
    Weight: 5);

path.AddPoint(40.737102, -73.990318);
path.AddPoint(40.749825, -73.987963);
path.AddPoint(40.752946, -73.987384);
path.AddPoint(40.755823, -73.986397);
```

---

## 🟦 Polygon (with Fill Color)

### 🌐 URL Equivalent

```text
&path=color:0x00000000|weight:5|fillcolor:0xFFFF0033|8th Avenue & 34th St,New York,NY|...
```

### ✅ Code

```csharp
var path = new Path(
    Color: new HexColor("0x00000000"),
    Weight: 5,
    FillColor: new HexColor("0xFFFF0033"));

path.AddPoint("8th Avenue & 34th St,New York,NY");
path.AddPoint("8th Avenue & 42nd St,New York,NY");
path.AddPoint("Park Ave & 42nd St,New York,NY");
path.AddPoint("Park Ave & 34th St,New York,NY");
```

---

## 🔗 Using an Encoded Polyline

### 🌐 URL Equivalent (truncated for readability)

```text
&path=weight:3|color:orange|enc:_fisIp~uCU... (encoded)
```

### ✅ Code

```csharp
var path = new Path(
    Weight: 3,
    Color: StaticMapColor.Orange);

path.AddPolyline("_fisIp~uCU..."); // use full encoded value
```

---

## 🟧 Encoded Polygon with Fill Color

### 🌐 URL Equivalent

```text
&path=fillcolor:0xAA000033|color:0xFFFFFF00|enc:%7DzswFtikbM...
```

### ✅ Code

```csharp
var path = new Path(
    Color: new HexColor("0xFFFFFF00"),
    FillColor: new HexColor("0xAA000033"));

path.AddPolyline("%7DzswFtikbM..."); // full encoded path
```

---

## ⚠️ Important Notes

* A path can contain **either**:

    * a list of points via `.AddPoint(...)`
    * **or** a single encoded polyline via `.AddPolyline(...)`
      Never both.
* Fill colors are only rendered if the path forms a polygon (i.e., >=3 points).
* Use `HexColor` with alpha for transparency (e.g. `0xAA000033`).

---

## 💡 Tips

* Use [Google’s Polyline Encoder Tool](https://developers.google.com/maps/documentation/utilities/polylineutility) to generate encoded paths.
* Use `StaticMapsUrlBuilder.AddPaths(...)` to add one or more `Path` instances to your map.

---

## ✅ Example Integration

```csharp
var path = new Path(Weight: 5, Color: StaticMapColor.Red);
path.AddPoint(48.8584, 2.2945);
path.AddPoint(48.8606, 2.3376);

var builder = new StaticMapsUrlBuilder()
    .AddSize(600, 400)
    .AddCenterWithLocation("Paris")
    .AddPaths(path)
    .AddKey("YOUR_API_KEY");

string url = builder.Build();
```

---

## 📚 Related

* [`Markers.md`](./Markers.md)
* [Official Paths Guide](https://developers.google.com/maps/documentation/maps-static/start#Paths)

