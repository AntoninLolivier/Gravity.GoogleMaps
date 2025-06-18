# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),  
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

---

## [2.1.0] - 2025-06-13

### ✨ Added

- `MapStyle` now supports styles without explicitly specifying a `Feature` or `Element`.
  - Example: `new MapStyle(new StyleRule(Color: ...))`
- Added documentation files:
  - [`Markers.md`](docs/Markers.md)
  - [`Paths.md`](docs/Paths.md)
  - [`MapStyles.md`](docs/MapStyles.md)

### 🐛 Fixed

- Fixed `Path.ToString()` when only a polyline is used and no style is defined:
  - Prevents malformed output like `path=|enc:...`
- Improved test coverage for polyline support and path validation

### 🧪 Internal

- Added new test cases for:
  - `AddPaths` with polylines
  - optional parameters in `MapStyle`
  - URL encoding validation
- Refactored `MarkerGroup` to have default constructor values
- Internal TODO added on coordinates precision in markers

---

## [2.0.0] - 2025-06-13

### ⚠️ Breaking Changes

- Changed `MapFormat`, `Element`, and `Feature` from `readonly record struct` to `sealed class` with internal constructors.
    - ⚠️ These types are no longer publicly instantiable (`new MapFormat("jpg")` is now invalid).
    - ✅ Use the predefined values via `MapFormats`, `Elements`, and `Features` static classes.

### ✨ Added

- `StaticMapsUrlBuilder.ReturnRelativeUrlOnly()`  
  Enables generating only the query string part of the URL (for `HttpClient` usage).

- `StaticMapsUrlBuilder.DisableApiKeyCheck()`  
  Allows bypassing the internal API key check for testing or public rendering.

- ⛔ Limit enforcement: A maximum of **5 custom marker icons** (`icon:`) is now allowed in the builder.  
  This matches the documented [Google Maps Static API](https://developers.google.com/maps/documentation/maps-static/start#CustomIcons) limitations.