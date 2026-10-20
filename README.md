# Anatomy Sandbox

An Android-first, interactive 3D anatomy learning sandbox built with **Unity 6.3 LTS**, C#, and URP.

> **Educational toy simulation only.** The physiology values and scenario responses are fictional approximations. They are not medically accurate, diagnostic, or suitable for real-world medical decisions.

## Planned experience

- A detailed rotatable human model with selectable body parts, skeleton, muscles, and organs.
- Animated breathing and heartbeat, plus illustrative circulation and oxygen visualizations.
- Play/pause, reset, simulation speed, scenario selection, vital signs, and an event timeline.
- Toy scenarios for healthy baseline, low oxygen, reduced blood flow, dehydration, and respiratory/circulatory disruption.
- Mobile-first controls and performance-conscious rendering.

## Current repository status

The initial C# foundation is being added. This repository is **not yet a complete Unity project**: it still needs the Unity project settings, scenes, prefabs/materials, UI, and an imported or authored detailed anatomy model. The scripts should be integrated and tested in Unity 6.3 LTS before calling the app runnable.

### Initial scripts

- `Assets/AnatomySandbox/Scripts/Simulation/SimulationScenario.cs` — scenario enum.
- `Assets/AnatomySandbox/Scripts/Simulation/VitalSigns.cs` — toy vital-sign data.
- `Assets/AnatomySandbox/Scripts/Simulation/AnatomySimulationController.cs` — time-based scenario responses and events.
- `Assets/AnatomySandbox/Scripts/Animation/OrganPulse.cs` — heartbeat pulse and illustrative emission.
- `Assets/AnatomySandbox/Scripts/Interaction/BodyPartInteractable.cs` — selectable/removable body-part component.
- `.gitignore` — ignores Unity-generated files.

## Next implementation steps

1. Create/open a Unity 6.3 LTS project using URP and copy these scripts into it.
2. Build the mobile-friendly scene and UI; connect the scenario selector, vital-sign display, timeline, and speed controls to `AnatomySimulationController`.
3. Add anatomical meshes, colliders, materials, labels, and organ animation.
4. Test touch interaction, lifecycle/reset behavior, and Android performance on a real device.
5. Add automated checks and document a repeatable Android build.

## Safety and scope

The simulation is designed for learning about relationships between body systems, not for depicting a real person's suffering. It should use non-graphic visual effects and clearly label every displayed value as simulated.
