# Experimental component editor for Stride in Avalonia

My idea for this weekend is to try creating a very simple component editor for Stride.

## Tasks

- [ ] Open a project file
- [ ] Select a scene
- [x] View the entity hierarchy
- [x] Select an entity and open its components in a panel
- [x] View components properties
- [x] Edit components properties
- [ ] Save the modified scene

### Enhancements

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

## Architecture
I really like the idea of Model View Update. So given a model you have a View "function" that renders that model. For one model there may be a few different ways of viewing it. Then the view can update the model by passing a Command with the Model to an Update "function".

In my implementation the `Stride.Editor.Presentation` module depends on `Design` and `Commands`. Each view implements `IView<TViewModel, TViewObjectBase>`. Here `TViewObjectBase` equals `IViewBuilder`, a virtual view builder of the [Avalonia.FuncUI](https://github.com/AvaloniaCommunity/Avalonia.FuncUI) for which I've written a wrapper.

Views inherit from the `ViewBase<TViewModel>` which imposes dependency on the `IViewBuilder` and requires an `IServiceRegistry` in its constructor. Through this registry the views get access to a command dispatcher which is used for creating view model updates.

Command dispatcher processes commands, modifies the view model and schedules an update to the view. View updater takes the current view model state, runs it through the view objects and gets a virtual control hierarchy, which is then compared to a previous state and the difference is applied to the actual Avalonia UI controls.
