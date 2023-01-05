# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/)

## [1.9.0] - 2023-01-05
### Added
- Devided NavigatorMenu into base class Navigator with NagivatorMenu.cs & NavigatorSubMenu

## [1.8.0] - 2022-12-12
### Changed
- UI.UINavigator to UI.Navigator

## [1.7.0] - 2022-10-18
### Added
- ScreenSafeArea.cs
- ContentSizeFitterRefresh.cs
- ReorderableUnityEventDrawer.cs
- RectTransformExtensions.cs new methods

### Changed
- Moved SLIDDES.UI.asmdef to scripts folder
- MenuEditorVisualizer.cs, now supports sub-menus, custom menu prefixes & updated inspector look

## [1.6.1] - 2022-09-07
### Removed
- SLIDDES top toolbar dropdown from unity removed

## [1.6.0] - 2022-08-31
### Added
- ScrollRectLinker
### Changed
- UINavigatorMenu, now has beginStartMethod instead of a single bool to close on start
- MenuEditorVisualizer, cleaned up inspector UI

## [1.5.1] - 2022-04-02
### Fixed
- Asmdef wrong configuration with editor

## [1.5.0] - 2022-03-26
### Added
- AddComponentMenu ordering
- MenuItem intergration
### Fixed
- Menu Editor Visualizer organize bug

## [1.4.2] - 2022-02-22
### Fixed
- Bug in MenuEditorVisualizer

## [1.4.1] - 2022-02-20
### Fixed
- Bug in MenuEditorVisualizer

## [1.4.0] - 2022-02-05
### Added
- UI Navigator Open() function can now be done with index

## [1.3.0] - 2022-01-24
### Added
- UI Navigator

## [1.2.0] - 2022-01-24
### Added
- ImageSlideShow.cs

## [1.1.0] - 2022-01-22
### Changed
- Moved Menu Editor Visualizer Editor folder
- Automatic menu detection now checks for "[Menu]" string in object name

## [1.0.1] - 2022-01-22
### Fixed
- Build error with Editor only script

## [1.0.0] - 2022-01-22
### Added
- First public release
- Menu Editor Visualizer