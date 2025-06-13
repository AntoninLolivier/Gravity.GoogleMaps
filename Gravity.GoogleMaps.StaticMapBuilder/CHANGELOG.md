# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),  
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

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