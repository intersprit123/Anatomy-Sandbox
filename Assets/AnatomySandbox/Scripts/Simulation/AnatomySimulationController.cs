using System;
using UnityEngine;

namespace AnatomySandbox.Simulation
{
    /// <summary>
    /// Lightweight, fictional physiology sandbox for educational demonstrations.
    /// This is deliberately not a medical model and must not be used for diagnosis.
    /// </summary>
    public sealed class AnatomySimulationController : MonoBehaviour
    {
        [SerializeField] private SimulationScenario scenario = SimulationScenario.Healthy;
        [SerializeField, Min(0.1f)] private float simulationSpeed = 1f;
        [SerializeField] private bool isPlaying = true;
        [SerializeField, Min(1f)] private float responseSeconds = 18f;

        public SimulationScenario Scenario => scenario;
        public VitalSigns Current { get; private set; } = VitalSigns.HealthyBaseline;
        public float ElapsedSeconds { get; private set; }
        public bool IsPlaying => isPlaying;
        public float SimulationSpeed => simulationSpeed;

        public event Action<VitalSigns> VitalSignsChanged;
        public event Action<string> TimelineEvent;

        private SimulationScenario lastScenario;

        private void Awake()
        {
            lastScenario = scenario;
            Current = VitalSigns.HealthyBaseline;
            Publish("Simulation ready. Fictional baseline loaded.");
        }

        private void Update()
        {
            if (!isPlaying) return;

            ElapsedSeconds += Time.deltaTime * simulationSpeed;
            float t = Mathf.Clamp01(ElapsedSeconds / responseSeconds);
            Current = Evaluate(scenario, t);
            VitalSignsChanged?.Invoke(Current);
        }

        public void SetScenario(SimulationScenario next)
        {
            if (scenario == next) return;
            scenario = next;
            lastScenario = next;
            ElapsedSeconds = 0f;
            Current = VitalSigns.HealthyBaseline;
            Publish("Scenario changed: " + next + ". Response restarts from baseline.");
            VitalSignsChanged?.Invoke(Current);
        }

        public void SetPlaying(bool playing) => isPlaying = playing;

        public void SetSpeed(float speed)
        {
            simulationSpeed = Mathf.Clamp(speed, 0.25f, 4f);
            Publish("Simulation speed: " + simulationSpeed.ToString("0.##") + "x.");
        }

        public void ResetSimulation()
        {
            ElapsedSeconds = 0f;
            scenario = SimulationScenario.Healthy;
            lastScenario = scenario;
            Current = VitalSigns.HealthyBaseline;
            isPlaying = true;
            Publish("Reset to healthy toy baseline.");
            VitalSignsChanged?.Invoke(Current);
        }

        /// <summary>
        /// Call when an interactable system is removed. The effect is illustrative only.
        /// </summary>
        public void SetSystemDisrupted(string systemName, bool disrupted)
        {
            if (!disrupted)
            {
                SetScenario(SimulationScenario.Healthy);
                Publish(systemName + " restored. Toy simulation returned to baseline.");
                return;
            }

            string normalized = (systemName ?? string.Empty).ToLowerInvariant();
            if (normalized.Contains("lung") || normalized.Contains("respir"))
                SetScenario(SimulationScenario.RespiratorySystemDisruption);
            else if (normalized.Contains("heart") || normalized.Contains("blood") || normalized.Contains("circul"))
                SetScenario(SimulationScenario.CirculatorySystemDisruption);
            else
                Publish(systemName + " changed. No dedicated toy response is defined yet.");
        }

        private VitalSigns Evaluate(SimulationScenario selected, float t)
        {
            VitalSigns v = VitalSigns.HealthyBaseline;
            float smooth = t * t * (3f - 2f * t);

            switch (selected)
            {
                case SimulationScenario.LowOxygen:
                case SimulationScenario.RespiratorySystemDisruption:
                    v.oxygenPercent = Mathf.Lerp(98f, 78f, smooth);
                    v.breathsPerMinute = Mathf.Lerp(14f, 30f, smooth);
                    v.heartRateBpm = Mathf.Lerp(72f, 112f, smooth);
                    v.circulationPercent = Mathf.Lerp(100f, 84f, smooth);
                    break;

                case SimulationScenario.ReducedBloodFlow:
                case SimulationScenario.CirculatorySystemDisruption:
                    v.circulationPercent = Mathf.Lerp(100f, 42f, smooth);
                    v.heartRateBpm = Mathf.Lerp(72f, 128f, smooth);
                    v.systolicPressure = Mathf.Lerp(120f, 88f, smooth);
                    v.diastolicPressure = Mathf.Lerp(80f, 58f, smooth);
                    v.oxygenPercent = Mathf.Lerp(98f, 91f, smooth);
                    break;

                case SimulationScenario.Dehydration:
                    v.hydrationPercent = Mathf.Lerp(100f, 48f, smooth);
                    v.heartRateBpm = Mathf.Lerp(72f, 104f, smooth);
                    v.systolicPressure = Mathf.Lerp(120f, 102f, smooth);
                    v.diastolicPressure = Mathf.Lerp(80f, 68f, smooth);
                    v.circulationPercent = Mathf.Lerp(100f, 74f, smooth);
                    break;
            }

            return v;
        }

        private void Publish(string message) => TimelineEvent?.Invoke(message);
    }
}
