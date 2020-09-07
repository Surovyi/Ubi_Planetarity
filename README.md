Novykov Borys

Test task for Unity developer position

Features list
    Implemented:
- pdeudo-2d perspective
- star system
- gravity force (only for rockets)
- random planet generation
- config support
- orbit visualizer
- game start camera animation
- dynamic camera move
- pause/resume game
- main menu
- planet hud
- win/loss conditions

     Not implemented:
- different rocket types
    the game supports a set of missile config files, but only one missile is graphically implemented.
- save/load
    the game has interfaces for saving and loading the game defining the architectural boundary of the application. Unfortunately, there was not enough time to implement at least one option.
- gameplay balancing
    it seems to me that at the moment the game is not balanced enough for the most interesting gameplay experience. Failed to correctly configure the configuration file due to lack of time.
- advanced graphics
    I want to add variety to the game in the form of graphic resources and processing shaders. There was simply not enough time for the sun shader for example.
- true gravity mode (for all celestial object)
    at the moment the game is saved by the flag "ClampMaxGravity" inside gameConfig.json from the fact that the rockets cannot go out from gravity of the released planet.
- portrait orientation
    it seems to me now the game is tied to the landscape orientation mode, since I paid less attention to this item than to the rest.
- profiling and optimization
    I wanted to implement as many functional features as possible in a short period of time, and there was simply not enough time for optimization
