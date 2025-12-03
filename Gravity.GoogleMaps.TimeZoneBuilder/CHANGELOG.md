# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),  
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

---

## [2.0.0] - 2025-12-03

### Summary
This release introduces more precise exception types for input validation
and refactors internal logic to prepare for future API expansions.

---

### ⚠️ Breaking Changes

- `AddLocation()` now throws `ArgumentOutOfRangeException` (instead of `InvalidOperationException`)
  when the `latitude` value is outside the valid range of -90 to 90.
- `AddLocation()` now throws `ArgumentOutOfRangeException` (instead of `InvalidOperationException`)
  when the `longitude` value is outside the valid range of -180 to 180.
- `AddLanguage()` now throws `ArgumentException` (instead of `InvalidOperationException`)
  when the language code is not exactly 2 letters long.

---

### 🔧 Internal Changes

A substantial internal refactoring was performed with the introduction of
`Gravity.GoogleMaps.Core` to reduce duplication and improve parameter
addition and validation flows.  
This prepares the codebase for supporting additional Google Maps APIs.

