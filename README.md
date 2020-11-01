# Experimental component editor for Stride in Avalonia

My idea for ~~this weekend~~ the month of October is to try creating a very simple component editor for Stride.

## Tasks

- [x] Open a project file
- [x] Select a scene
- [x] View the entity hierarchy
- [x] Select an entity and open its components in a panel
- [x] View components properties
- [x] Edit components properties
- [x] Save the modified scene

### Enhancements

- [ ] Make Undo/Redo more usable (currently too many events are registered)
- [ ] Figure out a sane docking system implementation
- [ ] Better documentation
- [ ] Styling
- [ ] Custom views for:
    - [ ] `Vector2`
    - [ ] `Vector3`
    - [ ] `Vector4`
    - [ ] `Quaternion`
    - [ ] `Color3`
    - [ ] `Color4`
    - [ ] Collections (lists/arrays/dictionaries)
    - [ ] Entity References
    - [ ] EntityComponent References
    - [ ] Abstract members (i.e. interface/abstract class implementations)
    - [ ] Asset References
- [ ] Member inlining
- [ ] Drag and drop entities onto component members with compatibility checking
- [ ] Hiding members with `[Display(Browsable = false)]` and `bool Enabled`
- [ ] Use singleton pattern for views and commands to avoid allocations

## Architecture
The project has been divided into main 5 parts:
 
* Design - abstract interfaces and view models,
* Commands - methods that modify the view models or call services,
* Services - more complex classes that manage objects in the application,
* Presentation - views for the view models,
    * Virtual - a wrapper over Avalonia.FuncUI to allow writing MVU-like views,
* Editor - initialization logic and anything that has to touch both service logic and presentation.

This kind of horizontal division has been used to impose strict dependencies (e.g. most services are UI independant and should not reference UI logic directly, only through some abstraction). In practice, however, it would often be easier to have just 1-2 projects.

### Plugins
_Everything should be a plugin_ was the motto.

* Menu items are registered in `IMenuProvider`
* Member views (for _property grid_) are registered in `MemberViewProvider`
* Views are registered in `ViewRegistry`
* Asset Editors are registered in `IAssetEditorRegistry`

There's not yet a mechanism for loading external plugins, but it shouldn't be hard to implement.

### Views
The views are stateless classes with a method that takes a view model and returns a virtual view object. Later this virtual view is applied to the actual UI system.

### Commands
Commands are stateless classes that require to be passed a context with data to execute. Commands are executed only by the CommandDispatcher, which processes commands after the UI thread finishes processing user input and then updates the view.
