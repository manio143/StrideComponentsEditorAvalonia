# Experimental component editor for Stride in Avalonia

My idea for this weekend is to try creating a very simple component editor for Stride.

## Tasks

- [ ] Open a scene file
- [ ] View the entity hierarchy
- [ ] Select an entity and open its components in a panel
- [ ] View components properties
- [ ] Edit components properties
- [ ] Save the modified scene

## Architecture
I really like the idea of Model View Update. So given a model you have a View "function" that renders that model. For one model there may be a few different ways of viewing it. Then the view can update the model by passing a Command with the Model to an Update "function". I know that there's [Avalonia.FuncUI](https://github.com/AvaloniaCommunity/Avalonia.FuncUI) but it's written in F# and I don't see a point in complicating Stride by doing multi language development.

As such I'll try to implement the following:

- Model is any class
- Command is any class
- View is a class implementing `IView<TModel, TCommand>`, which can have XAML associated with it.
- Update is a class implementing `IUpdate<TModel, TCommand>` and is the behaviour implementation, called by the View

I'm a little uncertain on the details of practicallity of the above. I'll see if it works when I start implementing it.
