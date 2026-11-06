# Snake Game – Unity

## 1. Game Overview

A 2D Snake game developed using Unity 6 and C#. The player controls a snake, collects different food types, earns points, and tries to achieve the highest score without colliding with walls or its own body.

The game includes Normal Food, Bonus Food, and Shrink Food, each with different gameplay effects.

## 2. How to Run the Game

1. Open Unity Hub.
2. Select **Add Project** and choose the project folder.
3. Open the project using Unity 6000.3.9f1.
4. Open the Main Game Scene from the Scenes folder.
5. Press the **Play** button in the Unity Editor.

For a standalone build, open Build Profiles, select the target platform, and click Build.

## 3. Controls

| Input | Action |
|---|---|
| Arrow Keys | Move Up, Down, Left and Right |
| W, A, S, D | Alternative movement controls |
| Gamepad D-Pad | Change direction |
| Gamepad Left Stick | Change direction |
| Pause Button | Pause the game |

The snake moves automatically. The player changes its direction using the available controls.

## 4. Food Types

| Food | Effect | Score |
|---|---|---:|
| Normal Food (Blue) | Increases length by 1 segment | 10 |
| Bonus Food (Orange) | Disappears after 5 seconds | 50 |
| Shrink Food (Yellow) | Removes 2 segments; minimum length is 3 | Configurable |

The snake's movement speed increases after collecting a configurable number of food items.

## 5. Architecture Overview

The project follows a modular architecture to separate gameplay logic, input, rendering, UI, and saving.

- **GameConfig:** ScriptableObject containing grid and gameplay configuration.
- **GridManager:** Handles grid positions, boundaries, and coordinate conversion.
- **SnakeModel:** Manages snake segments, direction, movement, growth, and shrinking.
- **InputReader:** Handles keyboard and gamepad input.
- **SnakeController:** Controls movement, food collection, and collision detection.
- **SnakeRenderer:** Handles visual snake movement using pooled segment objects and interpolation.
- **FoodSpawner:** Spawns food in unoccupied grid cells and manages food lifetime.
- **FoodDefinition:** ScriptableObject containing configurable food properties.
- **GameManager:** Manages game states, score, High Score, restart, and UI.
- **ISaveService / JsonSaveService:** Separates save operations from JSON-based data storage.

## 6. Assumptions Made

- The game uses a grid-based movement system.
- The snake moves automatically at fixed logical time intervals.
- The player cannot immediately reverse the snake's direction.
- The minimum snake length is three segments.
- Bonus Food disappears after five seconds if not collected.
- Food spawning uses configurable spawn weights.
- High Score is stored locally on the device.
- The game uses solid-wall collision as its default behavior.

## 7. What Was Skipped and Why

- **Automated Test Coverage:** Additional automated tests were not completed within the available development time.
- **Advanced Visual Effects:** Kept minimal to prioritize core gameplay functionality.
- **Additional Game Modes:** Not included because the project focuses on the standard Snake gameplay experience.

*These items should be updated if any of them are implemented before submission.*

## 8. Improvements With More Time

- Add optional wall-wrapping gameplay.
- Add more food types and special effects.
- Improve visual animations and sound effects.
- Add difficulty levels and additional game modes.
- Expand automated Edit Mode testing.
- Improve UI responsiveness across different screen sizes.
- Perform additional performance profiling and optimization.

## 9. Development Time

**Total Time Spent:** [48hr]

## 10. AI Tools Used

**ChatGPT (OpenAI)**

Used for:
- Understanding Unity and C# concepts.
- Assistance with code structure and architecture.
- Debugging and resolving programming errors.
- Guidance on gameplay mechanics and implementation.
- Assistance with UI setup and project documentation.

All suggested code and implementation steps were reviewed and tested during development.

---

**Project:** Snake Game  
**Engine:** Unity 6  
**Version:** 6000.3.9f1  
**Programming Language:** C#  
**Game Type:** 2D Arcade