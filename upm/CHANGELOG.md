# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [1.0.1] - 2025-09-04

### Fixed

- Fix link PackageReleaseNotes
- Rename Moroshka.Xcp.asmdef to correct assembly definition file name

### Changed

- Seal the Benchmark class to prevent inheritance
- Improve metadata structure in Moroshka.Xcp.csproj for better package information
- Minor update to README.md documentation

## [1.0.0] - 2025-08-27

### Added

- Base Exception Class:
  - Introduced `DetailedException` as the base class for all custom exceptions in the module.
  - Includes properties such as `Code`, `Context`, `Member` and `Line` to provide detailed error information.
  - Improved `ToString()` method that provides complete exception information, including the contents of `Data`.
- Specialized Exceptions:
  - Added `ArgException` for general argument-related errors with additional `Param` property.
  - Added `ArgNullException` for handling null arguments with additional `Param` property.
  - Added `ArgOutOfRangeException` for handling arguments that fall outside valid ranges with additional `Param` and `ActualValue` properties.
  - Added `InvOpException` for handling invalid operations due to object state.
  - Added `ObjDisposedException` to handle the use of an object after it has been released with additional `Object` property.
