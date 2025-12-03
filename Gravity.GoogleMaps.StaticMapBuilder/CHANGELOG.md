# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),  
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

---

## [3.0.0] - 2025-12-03

### Summary
This release focuses on consistent exception handling, clearer builder APIs,
and internal refactoring to support upcoming Google Maps API integrations.

---

### ⚠️ Breaking Changes

- **BREAKING:** `AddZoom()` now throws `ArgumentOutOfRangeException` when the zoom
  value is outside the range 0–22 (inclusive).
- **BREAKING:** `AddSize()` now throws `ArgumentOutOfRangeException` (instead of `ArgumentException`)
  when `width` or `height` is less than or equal to 0.
- **BREAKING:** `Build()` now throws `InvalidOperationException` when no parameters have been added
  (previously threw `ArgumentException`).
- **BREAKING:** `Build()` now throws `InvalidOperationException` when no API key is provided
  while `DisableApiKeyCheck` is set to `false` (previously threw `ArgumentException`).

---

### 🚧 Deprecated

- `StaticMapsUrlBuilder` is now marked as `Obsolete` and will be removed in **4.0.0**.
  Use `StaticMapUrlBuilder` instead.
- `AddVisiblePortWithCoordinates` and `AddViewportsWithLocation` are marked as `Obsolete`
  and replaced by the two new overloads of `AddViewports()`.

---

### ❌ Removed

- `UseHttp()`, `DisableUrlEncoding()`, and `ReturnRelativeUrlOnly()` were removed
  as previously announced.  
  Use `WithOptions()` instead.

---

### 🧪 Internal Changes

- Major refactoring with the introduction of the `Gravity.GoogleMaps.Core` project
  to reduce duplication and improve validation and builder flow.
  This will simplify the addition of new API features.

## **[2.2.0] – 2025-11-19**

### ✨ Added

* Introduced the new method `WithOptions(StaticMapBuilderOptions options)` to centralize and simplify builder configuration.
* Added the new type [`StaticMapBuilderOptions`](Builders/StaticMapBuilderOptions.cs), which replaces several individual configuration methods.

### ⛔ Deprecated

The following methods are now obsolete and will be removed in a future release.
They are fully replaced by the corresponding properties in `StaticMapBuilderOptions`:

* `UseHttp()` → replaced by `StaticMapBuilderOptions.UseHttp`
* `DisableUrlEncoding()` → replaced by `StaticMapBuilderOptions.DisableUrlEncoding`
* `ReturnRelativeUrlOnly()` → replaced by `StaticMapBuilderOptions.ReturnParametersOnly`

### 🔧 Changed

* Calling `DisableUrlEncoding` (either via the old method or via `StaticMapBuilderOptions`) **before adding parameters no longer throws an exception**.
  URL encoding is now applied when calling `.Build()`, not during parameter registration.

### 🧪 Internal

* `Feature`, `Element`, and `MapFormat` have been migrated from `sealed class` to `readonly record struct`, improving immutability, value semantics, and performance.

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