# 🌐 Gravity.GoogleMaps.TimeZoneBuilder

**Gravity.GoogleMaps.TimeZoneBuilder** is a fully type-safe, fluent, and extensible `.NET` URL builder for the [Google Time Zone API](https://developers.google.com/maps/documentation/timezone/overview).
It helps you generate correct and validated Time Zone API request URLs — with full IntelliSense, compile-time safety, and built-in validation for coordinates, timestamps, languages, and required parameters.

---

## 🚀 Features

* ✅ Fully type-safe and fluent API
* 📍 Strong validation for **latitude/longitude bounds**
* ⏱️ Automatic handling of Unix timestamps from `DateTimeOffset`
* 🌐 Optional `language` support with format validation
* 🔐 API key validation (can be disabled for testing)
* ⚙️ Options to disable URL encoding or return only query parameters
* 🧪 Fully unit-tested (100% coverage on builders)

---

## 📦 Installation

```bash
dotnet add package Gravity.GoogleMaps.TimeZoneBuilder
```

---

## 📚 Documentation

The builder supports all core parameters of the Time Zone API:

* **location** → latitude/longitude
* **timestamp** → `DateTimeOffset`, converted internally to Unix time
* **language** (optional)
* **key** → required unless disabled in options

> 📌 Refer to the official
> [Google Time Zone API documentation](https://developers.google.com/maps/documentation/timezone/overview)
> for parameter semantics and output fields.

---

## ⚡ Quick Start

```csharp
var url = new TimeZoneUrlBuilder()
    .AddLocation(48.8566, 2.3522)
    .AddTimeStamp(DateTimeOffset.UtcNow)
    .AddLanguage("fr")
    .AddKey("YOUR_API_KEY")
    .Build();
```

This will generate a fully validated Time Zone API request URL.

---

## 🧱 Fluent API

Every part of the URL is constructed through intuitive, discoverable methods:

```csharp
.AddLocation(latitude: 40.6892, longitude: -74.0445)
.AddTimeStamp(DateTimeOffset.UtcNow)
.AddLanguage("en")
.AddKey("my-api-key")
```

---

## ⚙️ Advanced Options

The builder also supports configuration options through:

```csharp
.WithOptions(new TimeZoneBuilderOptions
{
    DisableUrlEncoding = true,
    DisableApiKeyCheck = true,
    ReturnParametersOnly = false
});
```

### Available options:

| Option                 | Description                                              |
| ---------------------- | -------------------------------------------------------- |
| `DisableUrlEncoding`   | Leaves commas and values unescaped (useful for debugging) |
| `DisableApiKeyCheck`   | Allows building URLs without `.AddKey()` (tests, mocks)  |
| `ReturnParametersOnly` | Returns only `param=value…` without the base host        |

---

## 🧪 Test Coverage & Reliability

The builder is fully covered by unit tests:

* ✔️ Valid & invalid coordinates
* ✔️ Valid & invalid language codes
* ✔️ Timestamp conversion
* ✔️ Missing API key handling
* ✔️ Options overrides
* ✔️ URL encoding checks

Every scenario is validated both at build-time and runtime.

---

## 🛡️ License

[MIT License](../LICENSE)

---

## 🙌 Credits

Made with ❤️ by **GravityApps**
Developed & maintained by [https://github.com/AntoninLolivier](https://github.com/AntoninLolivier)