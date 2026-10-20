# Anatomy Sandbox

An Android-first 3D anatomy learning sandbox. The current foundation is **Unity C# source code** intended for Unity 6.3 LTS; this repository is not yet a complete, tested Unity project.

> **Educational toy only.** Vital-sign values are fictional approximations and are not medically accurate, diagnostic, or suitable for real-world medical decisions.

## Current foundation

- A procedural, stylized human mannequin built from Unity primitive meshes.
- Basic visible internal organ placeholders and skeletal landmarks.
- Body-part selection and hide/restore behavior.
- A fictional physiology scenario controller and vital-sign data model.
- Touch/mouse drag rotation and pinch/scroll zoom.
- A Unity-oriented `.gitignore`.

## Important limitations

- The generated body is a **placeholder**, not a realistic medical-grade anatomy model.
- The repository does not yet contain Unity project settings, a scene, materials/prefabs authored in the editor, or a detailed anatomy asset pack.
- The scripts have been written to the repository but have **not been compiled or run in Unity yet**.
- **No visual animations are included at this stage**, as requested. We will add animations later when suitable assets or user-provided files are ready.
- UI panels, part labels/details, layer toggles, scenario selector, timeline, and a complete Android build still need to be implemented.

## Getting started (when Unity is available)

1. Create a new Unity 6.3 LTS project using URP.
2. Copy the `Assets/AnatomySandbox` folder into the project.
3. Create an empty GameObject and add `ProceduralAnatomyBuilder`.
4. Press Play to generate the placeholder model.
5. Check the Console, then test drag rotation, pinch zoom, and body-part selection on a touch device.

## Scripts

- `Scripts/Setup/ProceduralAnatomyBuilder.cs` — creates a procedural mannequin and placeholder organs.
- `Scripts/Interaction/BodyPartInteractable.cs` — selection event and hide/restore logic.
- `Scripts/Interaction/ModelOrbitController.cs` — basic mouse/touch rotation and zoom.
- `Scripts/Simulation/SimulationScenario.cs` — scenario options.
- `Scripts/Simulation/VitalSigns.cs` — fictional vital-sign data.
- `Scripts/Simulation/AnatomySimulationController.cs` — time-based educational scenario values.

## Suggested asset sources

- Unity Asset Store: https://assetstore.unity.com/
- Sketchfab downloadable models: https://sketchfab.com/search?features=downloadable&q=human%20anatomy
- Mixamo for humanoid character rigging and later animations: https://www.mixamo.com/

Before importing any asset, check its license, Unity/URP compatibility, file format, and whether individual organs are separate meshes. For this project, a model with separately selectable named organs is more useful than a single fused mesh.

## Safety

This project is intended to explain anatomy and relationships between body systems using non-graphic fictional demonstrations. It should not depict a real person's suffering or be used as a medical tool.
